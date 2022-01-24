using Grabador.Transaccion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.Services.Object;
using WPFMultired.ViewModel;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para PaymentUserControl.xaml
    /// </summary>
    public partial class PaymentUserControl : UserControl
    {
        private Transaction transaction;

        private PaymentViewModel paymentViewModel;

        public PaymentUserControl(Transaction transaction)
        {
            InitializeComponent();

            this.transaction = transaction;

            grvPublicity.Content = Utilities.UCPublicityBanner;

            GoTime();

            OrganizeValues();
        }

        private void OrganizeValues()
        {
            try
            {
                this.paymentViewModel = new PaymentViewModel
                {
                    PayValue = transaction.Amount,
                    ValorFaltante = transaction.Amount,
                    ImgContinue = Visibility.Hidden,
                    ImgCancel = Visibility.Visible,
                    ImgCambio = Visibility.Hidden,
                    ValorSobrante = 0,
                    ValorIngresado = 0,
                    viewList = new CollectionViewSource(),
                    Denominations = new List<DenominationMoney>(),
                    ValorDispensado = 0,
                    ValorComision = transaction.Product.AmountCommission
                };

                if (transaction.eTypeService == ETypeServiceSelect.Deposito)
                {
                    txtPayValue.Visibility = Visibility.Hidden;
                    txtPayValueData.Visibility = Visibility.Hidden;
                }

                this.DataContext = this.paymentViewModel;
                AdminPayPlus.ControlPeripherals.ClearValues();
                ActivateWallet();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }


        private void ActivateWallet()
        {
            try
            {
                Task.Run(() =>
                {
                    AdminPayPlus.ControlPeripherals.callbackValueIn = enterValue =>
                    {
                        if (enterValue.Item1 > 0)
                        {
                            if (!this.paymentViewModel.StatePay)
                            {
                                paymentViewModel.ValorIngresado += enterValue.Item1;
                                if (paymentViewModel.ValorIngresado >= paymentViewModel.PayValue)
                                {
                                    this.paymentViewModel.ImgContinue = Visibility.Hidden;
                                    this.paymentViewModel.ImgCancel = Visibility.Hidden;
                                }
                                else if (paymentViewModel.ValorIngresado >= transaction.Product.AmountMin)
                                {
                                    this.paymentViewModel.ImgContinue = Visibility.Visible;
                                    this.paymentViewModel.ImgCancel = Visibility.Hidden;
                                }

                                paymentViewModel.RefreshListDenomination(int.Parse(enterValue.Item1.ToString()), 1, enterValue.Item2);

                                AdminPayPlus.SaveDetailsTransaction(transaction.IdTransactionAPi, enterValue.Item1, 2, 1, enterValue.Item2, string.Empty);
                                LoadView();
                            }
                        }
                    };

                    AdminPayPlus.ControlPeripherals.callbackTotalIn = async enterTotal =>
                    {
                        AdminPayPlus.ControlPeripherals.callbackTotalIn = null;
                        if (!this.paymentViewModel.StatePay)
                        {
                            this.paymentViewModel.ImgContinue = Visibility.Hidden;

                            this.paymentViewModel.ImgCancel = Visibility.Hidden;

                            if (enterTotal > 0 && paymentViewModel.ValorSobrante > 0)
                            {
                                this.paymentViewModel.ImgCambio = Visibility.Visible;

                                if (paymentViewModel.ValorSobrante >= 100)
                                {
                                    var decimas = paymentViewModel.ValorSobrante % 100;
                                    if (decimas > 0)
                                    {
                                        Utilities.ShowModal($"¡Gracias por usar nuestros servicios! Solo te podemos devolver {string.Format("{0:C0}", paymentViewModel.ValorSobrante - decimas)}, se abonará {string.Format("{0:C0}", decimas)} al mismo producto, no es posible entregar valores menores de $100.", EModalType.Error, this);
                                    }

                                    if (transaction.eTypeService == ETypeServiceSelect.EstadoCuenta
                                     || transaction.eTypeService == ETypeServiceSelect.TarjetaCredito)
                                    {
                                        await ReturnMoney(paymentViewModel.ValorSobrante - decimas, true);
                                    }
                                    else
                                    {
                                        string ms = string.Concat("Solicita vueltos a la máquina o abona el vuelto a la misma cuenta de ahorros a la que estas consignando.", Environment.NewLine,
                                            "Si tu vuelto es inferior a $100 se abonará automáticamente a la cuenta que estás consignando.");
                                        string tittle = "";

                                        if (!Utilities.ShowModal(ms, EModalType.ReturnMoney, this, false, tittle))
                                        {
                                            await ReturnMoney(paymentViewModel.ValorSobrante - decimas, true);
                                        }
                                        else
                                        {
                                            SavePay();
                                        }
                                    }
                                }
                                else
                                {
                                    Utilities.ShowModal($"No se puede devolver el valor restante, se abonará el excedente {string.Format("{0:C0}", paymentViewModel.ValorSobrante)} al mismo producto.", EModalType.Error, this);
                                    SavePay();
                                }
                            }
                            else
                            {
                                SavePay();
                            }
                        }
                    };

                    AdminPayPlus.ControlPeripherals.callbackError = error =>
                    {
                        var log = new RequestLogDevice
                        {
                            Code = error.Item1,
                            Date = DateTime.Now,
                            Description = error.Item2,
                            Level = ELevelError.Medium,
                            TransactionId = transaction.IdTransactionAPi
                        };

                        if (error.Item1.Equals("Info"))
                        {
                            log.Level = ELevelError.Mild;
                            log.Code = "";
                            AdminPayPlus.SaveLog(log, ELogType.Device);
                        }
                        else
                        {
                            AdminPayPlus.SaveLog(log, ELogType.Device);
                        }

                        if (error.Item2.Contains("FATAL"))
                        {
                            transaction.State = ETransactionState.Error;
                            if (error.Item1.Equals("AP"))
                            {
                                Utilities.ShowModal(MessageResource.ErrorPayment, EModalType.Error, this);
                                AdminPayPlus.ControlPeripherals.StopAceptance();
                                if (paymentViewModel.ValorIngresado > 0)
                                {
                                    transaction.Payment = paymentViewModel;
                                    Utilities.navigator.Navigate(UserControlView.ReturnMony, false, this.transaction);
                                }
                            }
                        }
                    };

                    AdminPayPlus.ControlPeripherals.StartAceptance(paymentViewModel.PayValue);
                });
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private async Task ReturnMoney(decimal returnValue, bool state)
        {
            try
            {
                await Task.Run(() =>
                {
                    AdminPayPlus.ControlPeripherals.StartDispenser(returnValue);

                    AdminPayPlus.ControlPeripherals.callbackOut = valueOut =>
                    {
                        AdminPayPlus.ControlPeripherals.callbackOut = null;
                        AdminPayPlus.ControlPeripherals.callbackTotalOut = null;

                        paymentViewModel.ValorDispensado += valueOut;

                        if (paymentViewModel.ValorDispensado == returnValue)
                        {
                            SavePay();
                        }
                        else
                        {
                            Utilities.ShowModal(MessageResource.IncompleteMony, EModalType.Error, this);
                            SavePay(ETransactionState.Error);
                        }
                    };

                    AdminPayPlus.ControlPeripherals.callbackTotalOut = totalOut =>
                    {
                        AdminPayPlus.ControlPeripherals.callbackTotalOut = null;
                        AdminPayPlus.ControlPeripherals.callbackOut = null;
                        if (state)
                        {
                            paymentViewModel.ValorDispensado += totalOut;
                            SavePay();
                        }
                    };

                    AdminPayPlus.ControlPeripherals.callbackLog = (log, isBK) =>
                    {
                        paymentViewModel.SplitDenomination(log, isBK);
                        AdminPayPlus.SaveDetailsTransaction(transaction.IdTransactionAPi, 0, 0, 0, string.Empty, log);
                    };
                });
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void LoadView()
        {
            try
            {
                Dispatcher.BeginInvoke((Action)delegate
                {
                    paymentViewModel.viewList.Source = paymentViewModel.Denominations;
                    lv_denominations.DataContext = paymentViewModel.viewList;
                    lv_denominations.Items.Refresh();
                });
                GC.Collect();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private async void SavePay(ETransactionState statePay = ETransactionState.Initial)
        {
            try
            {
                if (!this.paymentViewModel.StatePay)
                {
                    this.paymentViewModel.StatePay = true;
                    transaction.Payment = paymentViewModel;
                    transaction.State = statePay;
                    transaction.DateTransaction = DateTime.Now;

                    AdminPayPlus.ControlPeripherals.ClearValues();

                    if (transaction.IdTransactionAPi > 0)
                    {
                        await Task.Run(async () =>
                        {
                            this.transaction.Amount = paymentViewModel.ValorIngresado;
                            var response = await AdminPayPlus.ApiIntegration.CallService(ETypeService.Report_Invoice, transaction);

                            if (response.Data != null)
                            {
                                transaction = (Transaction)response.Data;

                                transaction.State = ETransactionState.Success;
                            }
                            else
                            {
                                transaction.State = ETransactionState.ErrorService;
                            }

                            Utilities.navigator.Navigate(UserControlView.PaySuccess, false, this.transaction);
                        });
                    }
                    else
                    {
                        AdminPayPlus.SaveErrorControl(MessageResource.NoInsertTransaction, this.transaction.TransactionId.ToString(), EError.Api, ELevelError.Strong);
                        Utilities.ShowModal(MessageResource.NoInsertTransaction, EModalType.Error, this);

                        if (this.paymentViewModel.ValorIngresado > 0)
                        {
                            Utilities.navigator.Navigate(UserControlView.ReturnMony, false, this.transaction);
                        }
                        else
                        {
                            Utilities.navigator.Navigate(UserControlView.Main);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void BtnCancell_TouchDown(object sender, System.Windows.Input.TouchEventArgs e)
        {
            try
            {

                this.paymentViewModel.ImgContinue = Visibility.Hidden;

                this.paymentViewModel.ImgCancel = Visibility.Hidden;

                if (Utilities.ShowModal(MessageResource.CancelTransaction, EModalType.Information, this))
                {
                    AdminPayPlus.ControlPeripherals.StopAceptance();
                    AdminPayPlus.ControlPeripherals.callbackLog = null;

                    CLSGrabador.FinalizarGrabacion();
                    if (!this.paymentViewModel.StatePay)
                    {
                        if (paymentViewModel.ValorIngresado > 0)
                        {
                            transaction.Payment = paymentViewModel;
                            Utilities.navigator.Navigate(UserControlView.ReturnMony, false, this.transaction);
                        }
                        else
                        {
                            Utilities.navigator.Navigate(UserControlView.Main);
                        }
                    }
                }
                else
                {
                    if (paymentViewModel.ValorIngresado > 0)
                    {
                        this.paymentViewModel.ImgContinue = Visibility.Visible;
                    }

                    this.paymentViewModel.ImgCancel = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void BtnConsign_StylusDown(object sender, System.Windows.Input.StylusDownEventArgs e)
        {

            this.paymentViewModel.ImgContinue = Visibility.Hidden;

            this.paymentViewModel.ImgCancel = Visibility.Hidden;

            if (!Utilities.ShowModal("¿Desea ingresar dinero adicional?", EModalType.Information, this))
            {
                this.paymentViewModel.PayValue = this.paymentViewModel.ValorIngresado;

                AdminPayPlus.ControlPeripherals.StopAceptance();
                AdminPayPlus.ControlPeripherals.callbackLog = null;

                SavePay();
            }
            else
            {
                this.paymentViewModel.ImgContinue = Visibility.Visible;

                this.paymentViewModel.ImgCancel = Visibility.Hidden;
            }
        }

        private void GoTime()
        {
            try
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(1);
                timer.Tick += UpdateTime;
                timer.Start();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void UpdateTime(object sender, EventArgs e)
        {
            try
            {
                txtHoraActual.Text = DateTime.Now.ToString("HH:mm tt");
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}
