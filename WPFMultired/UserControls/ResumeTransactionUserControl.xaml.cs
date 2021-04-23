using System;
using System.Linq;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para ResumeTransactionUserControl.xaml
    /// </summary>
    public partial class ResumeTransactionUserControl : UserControl
    {
        private Transaction transaction;
        public ResumeTransactionUserControl(Transaction transaction)
        {
            InitializeComponent();

            if (transaction != null)
            {
                this.transaction = transaction;
            }

            ConfigurateView();
        }

        private void ConfigurateView()
        {
            try
            {
                //tb_name_company.Text = AdminPayPlus.DataPayPlus.ListCompanies.Where(c => c.Item1 == transaction.CodeCompany).FirstOrDefault().Item2;
                tb_name_client.Text = transaction.payer.NAME;
                tb_amount_pay.Text = string.Format("{0:C0}", transaction.Payment.ValorIngresado);
                tb_extra_amount_pay.Text = string.Format("{0:C0}", transaction.Payment.ValorSobrante);
                tb_amount_return.Text = string.Format("{0:C0}", transaction.Payment.ValorDispensado);
                tb_id_transaction.Text = transaction.consecutive.ToString();
                tb_date.Text = DateTime.Now.ToString();
                tb_amount_tip.Text = string.Format("{0:C0}", (transaction.Payment.ValorIngresado - transaction.Payment.ValorDispensado));
                //tb_amount_tip.Text = string.Format("{0:C0}", transaction.AmountComission);
                tb_reference.Text = transaction.reference;
                tb_total.Text = string.Format("{0:C0}", transaction.Payment.ValorIngresado);
                ConfigurateList();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void ConfigurateList()
        {
            try
            {
                transaction.Payment.viewList.Source = transaction.Payment.Denominations.Where(d => d.Code == "MA" || d.Code == "AP").ToList();
                lv_denominations.DataContext = transaction.Payment.viewList;
                lv_denominations.Items.Refresh();

                InitTimer();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void InitTimer()
        {
            try
            {
                TimerService.Close();
                TimerService.CallBackTimerOut = response =>
                {
                    Utilities.navigator.Navigate(UserControlView.PaySuccess, false, this.transaction);
                };

                TimerService.Start(int.Parse(Utilities.GetConfiguration("DurationView")));
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Btn_acept_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                TimerService.Stop();
                TimerService.Close();
                Utilities.navigator.Navigate(UserControlView.PaySuccess, false, this.transaction);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}
