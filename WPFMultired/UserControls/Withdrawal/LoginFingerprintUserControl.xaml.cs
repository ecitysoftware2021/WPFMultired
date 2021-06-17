using Ecity.DigitalPersona.ReaderUareU;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para LoginFingerprintUserControl.xaml
    /// </summary>
    public partial class LoginFingerprintUserControl : UserControl
    {
        private Transaction transaction;
        private ModalModel modalModel;
        private int fingerIntents = 0;
        private bool enableTouch = false;
        private bool goToInitial = false;
        DispatcherTimer timerRestart = new DispatcherTimer();
        int restartCounter = 0;
        public LoginFingerprintUserControl(Transaction ts)
        {
            InitializeComponent();

            grvPublicity.Content = Utilities.UCPublicityBanner;
            transaction = ts;
            modalModel = new ModalModel
            {
                Tittle = string.Format(Utilities.GetConfiguration("MsgHuella"), transaction.finger),
                ImageModal = ImagesUrlResource.Finger
            };

            grvFinger.DataContext = modalModel;

            LoadReader();
        }

        private void LoadReader()
        {
            try
            {
                enableTouch = false;
                modalModel.Tittle = string.Format(Utilities.GetConfiguration("MsgHuella"), transaction.finger);
                modalModel.Messaje = string.Empty;
                EcityReader.callbackError = Error =>
                {
                    EcityReader.callbackTemplate = null;
                    EcityReader.callbackError = null;

                    AdminPayPlus.SaveErrorControl("ERROR DEL HUELLERO: " + Error, "", EError.Aplication, ELevelError.Medium);
                };

                if (EcityReader.OpenReader())
                {
                    EcityReader.callbackTemplate = Template =>
                    {
                        Dispatcher.BeginInvoke((Action)(() =>
                        {
                            EcityReader.callbackTemplate = null;
                            EcityReader.callbackError = null;

                            EcityReader.CancelCaptureAndCloseReader(EcityReader.OnCaptured);
                            Task.Run(() =>
                            {
                                bool stateFinger = ValidateUser(Template).Result;
                                if (!stateFinger)
                                {
                                    Utilities.CloseModal();
                                    fingerIntents++;
                                    if (fingerIntents < 4)
                                    {
                                        modalModel.Tittle = "No fue posible leer tu huella, inténtalo nuevamente.";
                                        modalModel.Messaje = "Toca la pantalla para continuar";
                                        enableTouch = true;
                                    }
                                    else
                                    {
                                        modalModel.Tittle = Utilities.GetConfiguration("MsgFailFinger");
                                        modalModel.Messaje = "Toca la pantalla para continuar";
                                        modalModel.ImageModal = ImagesUrlResource.fingerFail;
                                        goToInitial = true;
                                        enableTouch = true;
                                    }
                                }
                                else
                                {
                                    Consult();
                                }
                            });
                            Utilities.ShowModal("Estamos validando la información, por favor espera un momento.", EModalType.Preload, this);
                        }));
                    };

                    EcityReader.StartCaptureAsync(EcityReader.OnCaptured);
                }
                else
                {
                    Utilities.ShowModal("El huellero no se pudo habilitar, por favor intentalo de nuevo.", EModalType.Error, this);

                    Task.Run(async () =>
                    {
                        Utilities.navigator.Navigate(UserControlView.Main);
                    });
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
            }
        }

        private void Consult()
        {

            try
            {
                var response = AdminPayPlus.ApiIntegration.CallService(ETypeService.Consult_Invoice, this.transaction).Result;
                Utilities.CloseModal();

                if (response != null && response.Data != null)
                {
                    transaction = (Transaction)response.Data;
                    Utilities.navigator.Navigate(UserControlView.DataList, true, transaction);
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

        private async Task<bool> ValidateUser(string template)
        {
            try
            {
                var response = new Services.Object.Response();
                this.transaction.Action = $"{(int)EFingerAction.Authenticate}";
                this.transaction.Finger_Byte = template;
                response = await AdminPayPlus.ApiIntegration.CallService(ETypeService.Validate_Finger, this.transaction);


                if (response != null && response.Data != null)
                {
                    transaction = (Transaction)response.Data;
                    return true;
                }
                else
                {
                    return false;
                    //Utilities.ShowModal(response.Message, EModalType.Error, this);
                }
            }
            catch (Exception ex)
            {
                Utilities.CloseModal();
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);

                return false;
            }
        }

        private void Btn_exit_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                EcityReader.callbackTemplate = null;
                EcityReader.callbackError = null;
                EcityReader.CancelCaptureAndCloseReader(EcityReader.OnCaptured);
                Utilities.navigator.Navigate(UserControlView.Main);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GoTime();
        }

        private void btn_Restart_TouchEnter(object sender, TouchEventArgs e)
        {
            TimeToRestart();
        }


        private void TimeToRestart()
        {
            try
            {

                timerRestart.Interval = TimeSpan.FromSeconds(1);
                timerRestart.Tick += RestartTimeTick;
                timerRestart.Start();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void RestartTimeTick(object sender, EventArgs e)
        {
            restartCounter++;
            if (restartCounter >= int.Parse(Utilities.GetConfiguration("TimeToRestart")))
            {
                Utilities.RestartApp();
            }
        }

        private void btn_Restart_TouchLeave(object sender, TouchEventArgs e)
        {
            restartCounter = 0;
            timerRestart.Tick -= RestartTimeTick;
            timerRestart.Stop();
        }

        private void grvFinger_TouchDown(object sender, TouchEventArgs e)
        {
            if (enableTouch)
            {
                if (!goToInitial)
                {
                    LoadReader();
                }
                else
                {
                    Utilities.navigator.Navigate(UserControlView.Main);
                }
            }

        }
    }
}
