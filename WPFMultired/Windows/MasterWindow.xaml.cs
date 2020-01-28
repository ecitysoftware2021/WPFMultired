using System;
using System.Reflection;
using System.Windows;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;

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
