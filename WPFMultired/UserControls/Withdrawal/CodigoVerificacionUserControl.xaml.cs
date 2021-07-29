using Grabador.Transaccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;

namespace WPFMultired.UserControls.Withdrawal
{
    /// <summary>
    /// Lógica de interacción para CodigoVerificacionUserControl.xaml
    /// </summary>
    public partial class CodigoVerificacionUserControl : UserControl
    {
        private Transaction transaction;
        private int totpIntents = int.Parse(Utilities.GetConfiguration("totpIntents"));
        public CodigoVerificacionUserControl(Transaction ts)
        {
            InitializeComponent();

            this.transaction = ts;

            grvPublicity.Content = Utilities.UCPublicityBanner;

            GoTime();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                GenerateTOTP();
            });
            Utilities.ShowModal("Estamos generando el código OTP, espera un momento por favor.", EModalType.Preload, this);
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

        private void Btn_exit_TouchDown(object sender, TouchEventArgs e)
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

        private void Btn_back_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                Utilities.navigator.Navigate(UserControlView.DataList, true, transaction);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void PassBoxIdentification_TouchDown(object sender, TouchEventArgs e)
        {
            Utilities.OpenKeyboard(true, sender, this, 450);
        }

        private void Btn_show_id_TouchEnter(object sender, TouchEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(PassBoxIdentification.Password))
                {
                    TbxIdentification.Text = PassBoxIdentification.Password;
                    PassBoxIdentification.Visibility = System.Windows.Visibility.Hidden;
                    TbxIdentification.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void Btn_show_id_TouchLeave(object sender, TouchEventArgs e)
        {
            try
            {
                PassBoxIdentification.Visibility = System.Windows.Visibility.Visible;
                TbxIdentification.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void btn_Accept_TouchDown(object sender, TouchEventArgs e)
        {
            Task.Run(() =>
            {
                ValidateTOTP();
            });
            Utilities.ShowModal("Estamos validando el código, espera un momento por favor.", EModalType.Preload, this);

        }

        private void btnNewTOTP_TouchDown(object sender, TouchEventArgs e)
        {

            if (totpIntents > 0)
            {
                totpIntents--;
                txt_IntentsTOTP.Text = string.Format(Utilities.GetConfiguration("MsgTOTP"), totpIntents);
                Task.Run(() =>
                {
                    GenerateTOTP();
                });
                Utilities.ShowModal("Estamos generando el código OTP, espera un momento por favor.", EModalType.Preload, this);
            }
            else
            {
                Utilities.ShowModal(Utilities.GetConfiguration("MsgTOTPLimit"), EModalType.Error, this);
                Utilities.navigator.Navigate(UserControlView.Main);
            }
        }
        private void GenerateTOTP()
        {

            try
            {
                var response = AdminPayPlus.ApiIntegration.CallService(ETypeService.Generate_TOTP, this.transaction).Result;
                Utilities.CloseModal();

                if (response != null && response.Data != null)
                {
                    transaction = (Transaction)response.Data;


                }
                else
                {
                    Utilities.ShowModal(response.Message, EModalType.Error, this);
                }
            }
            catch (Exception ex)
            {
                Utilities.CloseModal();
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
        private void ValidateTOTP()
        {

            try
            {
                transaction.TOTP = PassBoxIdentification.Password;
                var response = AdminPayPlus.ApiIntegration.CallService(ETypeService.Validate_TOTP, this.transaction).Result;
                Utilities.CloseModal();

                if (response != null && response.Data != null)
                {
                    transaction = (Transaction)response.Data;
                    SendData();
                }
                else
                {
                    Utilities.ShowModal(response.Message, EModalType.Error, this);
                    Dispatcher.BeginInvoke((Action)delegate
                    {
                        PassBoxIdentification.Password = string.Empty;
                        TbxIdentification.Text = string.Empty;
                    });
                }
            }
            catch (Exception ex)
            {
                Utilities.CloseModal();
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private async void SendData()
        {
            try
            {
                if (transaction.Amount > 0)
                {
                    Task.Run(async () =>
                    {
                        await AdminPayPlus.SaveTransactions(this.transaction, false);

                        if (this.transaction.IdTransactionAPi == 0)
                        {
                            Utilities.ShowModal(MessageResource.NoProccessInformation, EModalType.Error, this);
                            Utilities.navigator.Navigate(UserControlView.Main);
                        }
                        else
                        {
                            NotificarRetiro();
                        }
                    });
                    Utilities.ShowModal("Estamos procesando la información, por favor espera un momento.", EModalType.Preload, this);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void NotificarRetiro()
        {
            try
            {
                var response = AdminPayPlus.ApiIntegration.CallService(ETypeService.Create_Transaction_Retiro, this.transaction).Result;
                Utilities.CloseModal();

                if (response != null && response.Data != null)
                {
                    transaction = (Transaction)response.Data;

                    Task.Run(() =>
                    {
                        CLSGrabador.IniciarGrabacion(new DataVidio
                        {
                            paypadID = AdminPayPlus.DataConfiguration.ID_PAYPAD.Value,
                            mailAlert = $"'{Utilities.GetConfiguration("Email")}'",
                            transactionID = transaction.IdTransactionAPi,
                            RecorderRoute = Utilities.GetConfiguration("RecorderRoute"),
                            selectedCamera = 0,
                            videoPath = $"'{Utilities.GetConfiguration("VideoRoute")}'"
                        });
                        Thread.Sleep(500);
                        CLSGrabador.IniciarGrabacion(new DataVidio
                        {
                            paypadID = AdminPayPlus.DataConfiguration.ID_PAYPAD.Value,
                            mailAlert = $"'{Utilities.GetConfiguration("Email")}'",
                            transactionID = transaction.IdTransactionAPi,
                            RecorderRoute = Utilities.GetConfiguration("RecorderRoute"),
                            selectedCamera = 1,
                            videoPath = $"'{Utilities.GetConfiguration("VideoRoute")}'"
                        });
                    });
                    Utilities.navigator.Navigate(UserControlView.ReturnMony, false, transaction);
                }
                else
                {
                    Utilities.ShowModal(response.Message, EModalType.Error, this);

                    Utilities.navigator.Navigate(UserControlView.Main);

                }
            }
            catch (Exception ex)
            {
                Utilities.CloseModal();
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}
