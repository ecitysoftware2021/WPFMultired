using System;
using System.IO;
using System.Reflection;
using System.Timers;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.ViewModel;

namespace WPFMultired.UserControls.Withdrawal
{
    /// <summary>
    /// Interaction logic for PhotoUserControl.xaml
    /// </summary>
    public partial class PhotoUserControl : UserControl
    {
        Transaction transaction;
        private Timer timerCam;
        private Timer timerCountDown;
        private int Count;
        private int IntervalCamera;
        public PhotoUserControl(Transaction ts)
        {
            InitializeComponent();
            this.transaction = ts;
            grvPublicity.Content = Utilities.UCPublicityBanner;

            lblTitle.Text = Utilities.GetConfiguration("MsgFoto");
        }

        #region Photo Controller
        private void InitialCam()
        {
            try
            {
                StartCamera();
                Restart();
                MainWindowViewModel.PhotoName = transaction.reference;

                MainWindowViewModel.CallBackPotho = photo =>
                {
                    Dispatcher.BeginInvoke((Action)delegate
                    {
                        StopCamera();
                        PhotoImagen(Path.Combine(Directory.GetCurrentDirectory(), "Photos", transaction.reference + ".png"));

                    });
                };
                timerCam.Interval = IntervalCamera;
                timerCam.Elapsed += new System.Timers.ElapsedEventHandler(TimerTick);
                timerCam.Start();

                InitialCountDown();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
            }
        }
        private void Restart()
        {
            try
            {
                timerCam = new Timer();
                timerCountDown = new Timer();
                MainWindowViewModel.CallBackPotho = null;
                MainWindowViewModel.PhotoName = null;
                IntervalCamera = Convert.ToInt32(Utilities.GetConfiguration("IntervalCamera"));
                Count = (IntervalCamera / 1000) - 1;
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
            }
        }

        private void TimerTick(object sender, ElapsedEventArgs e)
        {
            try
            {
                TakePhoto();
                timerCam.Stop();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
            }
        }

        public void InitialCountDown()
        {
            try
            {
                timerCountDown = new Timer();
                timerCountDown.Interval = 1000;
                timerCountDown.Enabled = true;

                timerCountDown.Elapsed += new System.Timers.ElapsedEventHandler(myTimer_Tick);

                timerCountDown.Start();
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
            }
        }

        private void myTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                Dispatcher.BeginInvoke((Action)delegate
                {
                    lblNum.Visibility = Visibility.Visible;
                    lblNum.Text = Count.ToString();

                    Count -= 1;

                    if (Count <= 0)
                    {
                        timerCountDown.Stop();
                        lblNum.Visibility = Visibility.Hidden;
                    }
                });
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
            }
        }

        void StartCamera()
        {
            Dispatcher.Invoke(() =>
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(btnActivarCamara);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            });
        }

        void StopCamera()
        {
            Dispatcher.Invoke(() =>
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(btnStopVideo);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            });
        }

        void TakePhoto()
        {
            Dispatcher.Invoke(() =>
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(BtnTakePhoto);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            });
        }

        private void PhotoImagen(string uriImagen)
        {
            try
            {
                Byte[] bytes = File.ReadAllBytes(uriImagen);
                String file = Convert.ToBase64String(bytes);
                transaction.Image = file;
                transaction.ImageName = transaction.reference + ".png";
                transaction.ImageLength = file.Length;
                Utilities.navigator.Navigate(UserControlView.TOTPValidator, false, transaction);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, ex.ToString());
            }
        }
        #endregion

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
                StopCamera();
                Utilities.navigator.Navigate(UserControlView.DataList, true, transaction);
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, this.GetType().Name, ex, MessageResource.StandarError);
            }
        }

        private void BtnCancell_TouchDown(object sender, TouchEventArgs e)
        {
            StopCamera();
            Utilities.navigator.Navigate(UserControlView.Main);
        }

        private void btn_Accept_TouchDown(object sender, TouchEventArgs e)
        {
            btn_Accept.Visibility = Visibility.Hidden;
            btnCancell.Visibility = Visibility.Hidden;
            InitialCam();
        }
    }
}
