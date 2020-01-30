using System;
using System.Reflection;
using System.Windows.Controls;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Resources;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para MenuUserControl.xaml
    /// </summary>
    public partial class MenuUserControl : UserControl
    {
        public MenuUserControl()
        {
            InitializeComponent();
        }

        private void Btn_back_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                Utilities.navigator.Navigate(UserControlView.Main);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void BtnOptionSelect_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                switch (((Image)sender).Tag.ToString())
                {
                    case "1":
                        Utilities.navigator.Navigate(UserControlView.SubMenu, true, ETransactionType.Pay);
                        break;
                    case "2":
                        Utilities.navigator.Navigate(UserControlView.SubMenu, true, ETransactionType.Withdrawal);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}
