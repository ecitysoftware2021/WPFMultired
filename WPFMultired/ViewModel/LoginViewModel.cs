using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFMultired.Classes;

namespace WPFMultired.ViewModel
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private string _messageError;

        public string MessageError
        {
            get
            {
                return _messageError;
            }
            set
            {
                if (_messageError != value)
                {
                    _messageError = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MessageError)));
                }
            }
        }

        private string _user;

        public string User
        {
            get
            {
                return _user;
            }
            set
            {
                if (_user != value)
                {
                    _user = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(User)));
                }
            }
        }

        private string _pass;

        public string Pass
        {
            get
            {
                return _pass;
            }
            set
            {
                if (_pass != value)
                {
                    _pass = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Pass)));
                }
            }
        }

        private string _qr;

        public string Qr
        {
            get
            {
                return _qr;
            }
            set
            {
                if (_qr != value)
                {
                    _qr = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Qr)));
                }
            }
        }

        private ETypeAdministrator _typeOperation;

        public ETypeAdministrator TypeOperation
        {
            get
            {
                return _typeOperation;
            }
            set
            {
                if (_typeOperation != value)
                {
                    _typeOperation = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TypeOperation)));
                }
            }
        }

        private int _typeLogin;

        public int TypeLogin
        {
            get
            {
                return _typeLogin;
            }
            set
            {
                if (_typeLogin != value)
                {
                    _typeLogin = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TypeLogin)));
                }
            }
        }

        private Visibility _visibleGdLogin;

        public Visibility VisibleGdLogin
        {
            get
            {
                return _visibleGdLogin;
            }
            set
            {
                if (_visibleGdLogin != value)
                {
                    _visibleGdLogin = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibleGdLogin)));
                }
            }
        }

        private Visibility _visibleGdQr;

        public Visibility VisibleGdQr
        {
            get
            {
                return _visibleGdQr;
            }
            set
            {
                if (_visibleGdQr != value)
                {
                    _visibleGdQr = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibleGdQr)));
                }
            }
        }

        private Visibility _visibleBtnAcept;

        public Visibility VisibleBtnAcept
        {
            get
            {
                return _visibleBtnAcept;
            }
            set
            {
                if (_visibleBtnAcept != value)
                {
                    _visibleBtnAcept = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibleBtnAcept)));
                }
            }
        }
    }
}
