using Ecity.DigitalPersona.ReaderUareU;
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
using WPFMultired.Services.Object;

namespace WPFMultired.UserControls
{
    /// <summary>
    /// Lógica de interacción para LoginFingerprintUserControl.xaml
    /// </summary>
    public partial class LoginFingerprintUserControl : UserControl
    {
        private bool huellaVerificada;

        private Transaction transaction;
        public LoginFingerprintUserControl(Transaction ts)
        {
            InitializeComponent();

            transaction = ts;

            txt_title.Text = "Por favor ubica tu dedo índice registrado en el lector de huella, en la parte derecha del dispositivo.";

            huellaVerificada = true;

            LoadReader();
        }

        private void LoadReader()
        {
            try
            {
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
                        EcityReader.callbackTemplate = null;
                        EcityReader.callbackError = null;

                        EcityReader.CancelCaptureAndCloseReader(EcityReader.OnCaptured);

                        txt_error.Visibility = Visibility.Hidden;
                        if (!string.IsNullOrEmpty(Template))
                        {
                            Dispatcher.BeginInvoke((Action)(() =>
                            {
                                ValidateUser(Template);
                            }));
                        }
                        else
                        {
                            txt_title.Text = "No fue posible leer tu huella, inténtalo nuevamente.";
                            txt_error.Visibility = Visibility.Visible;
                            LoadReader();
                        }
                    };

                    EcityReader.StartCaptureAsync(EcityReader.OnCaptured);
                }
                else
                {
                    Utilities.ShowModal("El huellero no se pudo habilitar, por favor intentalo de nuevo.", EModalType.Error, this);

                    Task.Run(async () =>
                    {
                        Utilities.navigator.Navigate(UserControlView.Menu, true);
                    });
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
            }
        }

        private void ValidateUser(string template)
        {
            try
            {
                Task.Run(async () =>
                {
                    //TODO: colocar la información necesaria para mandarlo al WS
                    AuthenticationBiomety biomety = new AuthenticationBiomety
                    {
                        CodSession = 0,
                        Identification = "",
                        TypeReader = 1,
                        Template = template
                    };

                    //TODO: si el WS respondió mal, colocarlo en false
                    huellaVerificada = false;
                    if (!huellaVerificada)
                    {
                        txt_title.Text = "No fue posible leer tu huella. Por favor realiza tu retiro en caja o en cualquiera de los cajeros automáticos.";
                        txt_error.Visibility = Visibility.Visible;

                        errorHuella_gif.Visibility = Visibility.Visible;
                        huella_gif.Visibility = Visibility.Hidden;
                    }
                    else
                    {

                    }
                });
                //TODO: Borrar esta modal si es necesario
                Utilities.ShowModal("Estamos procesando la información, por favor espera un momento.", EModalType.Preload, this);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
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

        private void grdInteract_TouchDown(object sender, TouchEventArgs e)
        {
            try
            {
                if (!huellaVerificada)
                {
                    Utilities.navigator.Navigate(UserControlView.Main);
                }
                else
                {
                    txt_title.Text = "Por favor ubica tu dedo índice registrado en el lector de huella, en la parte derecha del dispositivo.";
                    txt_error.Visibility = Visibility.Hidden;

                    errorHuella_gif.Visibility = Visibility.Hidden;
                    huella_gif.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }
    }
}
