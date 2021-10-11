using Grabador.Transaccion;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.UserControls;

namespace WPFMultired.Windows
{
    /// <summary>
    /// Lógica de interacción para MasterWindow.xaml
    /// </summary>
    public partial class MasterWindow : Window
    {
        public MasterWindow()
        {
            InitializeComponent();

            SetUserControl();
        }

        private void SetUserControl()
        {
            try
            {
                if (Utilities.navigator == null)
                {
                    Utilities.navigator = new Navigation();
                }

                if (Utilities.UCPublicityBanner == null)
                {
                    Utilities.UCPublicityBanner = new PublicityUserControl();
                }

                WPKeyboard.Keyboard.ConsttrucKeyyboard(WPKeyboard.Keyboard.EStyle.style_2);

                string a = Encryptor.Encrypt("usrapli");
                string b = Encryptor.Encrypt("1Cero12019$/*");
                string c = Encryptor.Encrypt("Ecity.Software");
                string d = Encryptor.Encrypt("Ecitysoftware2019#");
                string cA = Encryptor.Encrypt("Pay+ Comultrasan 2");
                string dA = Encryptor.Encrypt("Comultrasan2021*");

                Utilities.navigator.Navigate(UserControlView.Config);

                DataContext = Utilities.navigator;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}
