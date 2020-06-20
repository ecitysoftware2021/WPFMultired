using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Models;

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
                tb_name_company.Text = AdminPayPlus.DataPayPlus.ListCompanies.Where(c => c.Item1 == transaction.CodeCompany).FirstOrDefault().Item2;
                tb_name_client.Text = transaction.payer.NAME;
                tb_amount_pay.Text = string.Format("{0:C0}", transaction.Amount);
                tb_extra_amount_pay.Text = string.Format("{0:C0}", transaction.Payment.ValorSobrante);
                tb_amount_return.Text = string.Format("{0:C0}", transaction.Payment.ValorDispensado);
                tb_id_transaction.Text = transaction.consecutive.ToString();
                tb_date.Text = DateTime.Now.ToString();
                tb_amount_tip.Text = string.Format("{0:C0}", transaction.AmountComission);
                tb_reference.Text = transaction.reference;
                tb_total.Text = string.Format("{0:C0}", transaction.Payment.ValorIngresado);
                ConfigurateList();
            }
            catch (Exception ex)
            {

            }
        }

        private void ConfigurateList()
        {
            try
            {
                transaction.Payment.viewList.Source = transaction.Payment.Denominations.Where(d => d.Code == "MA" || d.Code == "AP").ToList();
                lv_denominations.DataContext = transaction.Payment.viewList;
                lv_denominations.Items.Refresh();
            }
            catch (Exception ex)
            {

            }
        }

        private void Btn_acept_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                Utilities.navigator.Navigate(UserControlView.PaySuccess, false, this.transaction);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
