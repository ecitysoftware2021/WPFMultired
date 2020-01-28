using System;
using System.Reflection;
using System.Windows.Controls;
using WPFMultired.Classes;
using WPFMultired.Resources;

namespace WPFMultired.UserControls.Administrator
{
    /// <summary>
    /// Lógica de interacción para ConfigurateUserControl.xaml
    /// </summary>
    public partial class ConfigurateUserControl : UserControl
    {
        private AdminPayPlus init;

        public ConfigurateUserControl()
        {
            try
            {
                InitializeComponent();

                if (init == null)
                {
                    init = new AdminPayPlus();
                }

                Initial();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private async void Initial()
        {
            try
            {
                this.DataContext = init;

                init.callbackResult = result =>
                {
                    ProccesResult(result);
                };

                init.Start();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private async void ProccesResult(bool result)
        {
            try
            {
                if (AdminPayPlus.DataPayPlus.StateBalanece)
                {
                    Utilities.navigator.Navigate(UserControlView.Login, false, 1);
                }
                else if (AdminPayPlus.DataPayPlus.StateUpload)
                {
                    Utilities.navigator.Navigate(UserControlView.Login, false, 2);
                }
                else
                {
                    if (result)
                    {
                        if (!AdminPayPlus.DataPayPlus.StateBalanece && !AdminPayPlus.DataPayPlus.StateUpload)
                        {
                            Utilities.navigator.Navigate(UserControlView.Main);
                        }
                    }
                    else
                    {
                        Utilities.ShowModal(MessageResource.NoService, EModalType.Error, false);
                        Initial();
                    }
                }
            }
            catch (Exception ex)
            {
                Utilities.ShowModal(MessageResource.NoService, EModalType.Error, false);
                Initial();
            }
        }
    }
}
