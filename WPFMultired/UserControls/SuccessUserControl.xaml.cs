using Grabador.Transaccion;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.Services.Object;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para SuccessUserControl.xaml
    /// </summary>
    public partial class SuccessUserControl : UserControl
    {
        private Transaction transaction;

        public SuccessUserControl(Transaction transaction)
        {
            InitializeComponent();

            this.transaction = transaction;

            grvPublicity.Content = Utilities.UCPublicityBanner;

            finalizarGrabacion();

            presentarTransaccion();
        }

        private void finalizarGrabacion()
        {
            CLSGrabador.FinalizarGrabacion();
        }

        private void presentarTransaccion()
        {
            if(transaction.CodeTypeTransaction == Utilities.GetConfiguration("CodDepositos"))
            {
                var valorConsignacion = transaction.Payment.ValorIngresado - transaction.Payment.ValorDispensado;

                fechaTransaccion.Text = transaction.DateTransaction.ToString();
                nombreTitularTransaccion.Text = transaction.payer.NAME;
                valorTransaccion.Text = string.Format("{0:C0}", valorConsignacion);
                valorComisionTransaccion.Text = string.Format("{0:C0}", transaction.Product.AmountCommission);
                numeroAprobacionTransaccion.Text = transaction.DatosAdicionales.NROAPR;
            }
            else if (transaction.CodeTypeTransaction == Utilities.GetConfiguration("CodRetiros"))
            {
                fechaTransaccion.Text = transaction.DateTransaction.ToString();
                nombreTitularTransaccion.Text = transaction.payer.NAME;
                valorTransaccion.Text = string.Format("{0:C0}", transaction.Amount);
                valorComisionTransaccion.Text = string.Format("{0:C0}", transaction.Product.AmountCommission);
                numeroAprobacionTransaccion.Text = transaction.CodeTransactionAuditory;
            }
        }

        private void Btn_aceptar_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                if (transaction.State == ETransactionState.Success)
                {
                    AdminPayPlus.SaveLog(new RequestLog
                    {
                        Description = MessageResource.SussesTransaction,
                        Reference = transaction.reference
                    }, ELogType.General);
                }
                else
                {
                    AdminPayPlus.SaveLog(new RequestLog
                    {
                        Description = MessageResource.NoveltyTransation,
                        Reference = transaction.reference
                    }, ELogType.General);
                }

                Task.Run(() =>
                {
                    AdminPayPlus.ApiIntegration.CallService(ETypeService.Report_Cash, transaction);

                    AdminPayPlus.UpdateTransaction(this.transaction);

                    Dispatcher.BeginInvoke((Action)delegate
                    {
                        if (transaction.State == ETransactionState.Error)
                        {
                            Utilities.RestartApp();
                        }
                        else
                        {
                            Utilities.navigator.Navigate(UserControlView.Voucher, false, this.transaction);
                        }
                    });

                    GC.Collect();
                });
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void ShowModal()
        {
            try
            {
                if (Utilities.ShowModal("¿Desea realizar cashback?", EModalType.Information, this))
                {
                    transaction.IsCashBack = true;
                    this.transaction.Type = ETransactionType.Withdrawal;
                    transaction.CodeTypeTransaction = AdminPayPlus.DataPayPlus.ListTypeTransactions[1].Item3;
                    transaction.Payment = null;

                    if (!Utilities.ShowModalDetails(transaction, ETypeDetailModel.Withdrawal))
                    {
                        Utilities.navigator.Navigate(UserControlView.Main);
                    }
                }
                else
                {
                    Utilities.navigator.Navigate(UserControlView.Main);
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}
