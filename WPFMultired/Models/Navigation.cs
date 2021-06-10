using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPFMultired.Classes;
using WPFMultired.Services.Object;
using WPFMultired.UserControls;
using WPFMultired.UserControls.Administrator;

namespace WPFMultired.Models
{
    public class Navigation : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private UserControl _view;

        public UserControl View
        {
            get
            {
                return _view;
            }
            set
            {
                _view = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(View)));
            }
        }

        public void Navigate(UserControlView newWindow, bool initTimer = false, object data = null, object complement = null) => Application.Current.Dispatcher.Invoke((Action)delegate
        {
            try
            {
                //Mouse.Synchronize();
                switch (newWindow)
                {
                    case UserControlView.Main:
                        View = data != null ? new MainUserControl((bool)data) : new MainUserControl();
                        break;
                    case UserControlView.Consult:
                        View = new ConsultUserControl((string)data, (string)complement);
                        break;
                    case UserControlView.PaySuccess:
                        View = new SussesUserControl((Transaction)data);
                        break;
                    case UserControlView.Pay:
                        View = new PaymentUserControl((Transaction)data);
                        break;
                    case UserControlView.ReturnMony:
                        View = new ReturnMonyUserControl((Transaction)data);
                        break;
                    case UserControlView.Login:
                        View = new LoginAdministratorUserControl((ETypeAdministrator)data);
                        break;
                    case UserControlView.Config:
                        View = new ConfigurateUserControl();
                        break;
                    case UserControlView.Admin:
                        View = new AdministratorUserControl((PaypadOperationControl)data, (ETypeAdministrator)complement);
                        break;
                    case UserControlView.Menu:
                        View = new MenuUserControl();
                        break;
                    case UserControlView.DataList:
                        View = new DataListUserControl((Transaction)data);
                        break;
                    case UserControlView.Detail:
                        View = new DetailUserControl((Transaction)data);
                        break;
                    case UserControlView.ResumeTransaction:
                        View = new ResumeTransactionUserControl((Transaction)data);
                        break;
                    case UserControlView.Fingerprint:
                        View = new LoginFingerprintUserControl((Transaction)data);
                        break;
                }

                TimerService.Close();

                if (initTimer)
                {
                    TimerService.CallBackTimerOut = response =>
                    {
                        try
                        {
                            Application.Current.Dispatcher.Invoke((Action)delegate
                            {
                                WPKeyboard.Keyboard.CloseKeyboard(View);
                                View = new MainUserControl();
                            });
                            GC.Collect();
                        }
                        catch (Exception ex)
                        {
                        }
                    };

                    TimerService.Start(int.Parse(Utilities.GetConfiguration("DurationView")));
                }
            }
            catch (Exception ex)
            {
                Error.SaveLogError(MethodBase.GetCurrentMethod().Name, "Navigate", ex);
            }
            GC.Collect();
        });
    }
}