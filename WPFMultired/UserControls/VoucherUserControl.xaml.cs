using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFMultired.Classes;
using WPFMultired.Models;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para VoucherUserControl.xaml
    /// </summary>
    public partial class VoucherUserControl : UserControl
    {
        private Transaction transaction;

        public VoucherUserControl(Transaction transaction)
        {
            InitializeComponent();

            this.transaction = transaction;

            grvPublicity.Content = Utilities.UCPublicityBanner;
        }

        private void btn_Yes_TouchDown(object sender, TouchEventArgs e)
        {
            Utilities.PrintVoucher(this.transaction);

            presentarPaginaPrincipal();
        }

        private void Btn_No_TouchDown(object sender, TouchEventArgs e)
        {
            presentarPaginaPrincipal();
        }

        private void presentarPaginaPrincipal()
        {
            Utilities.navigator.Navigate(UserControlView.Main);
        }
    }
}
