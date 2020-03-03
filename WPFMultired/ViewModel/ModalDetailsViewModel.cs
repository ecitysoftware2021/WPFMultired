using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WPFMultired.Classes;
using WPFMultired.Models;
using WPFMultired.Resources;
using WPFMultired.Services.Object;

namespace WPFMultired.ViewModel
{
    class ModalDetailsViewModel : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private string _message;

        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                if (_message != value)
                {
                    _message = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Message)));
                }
            }
        }

        private decimal _amount;

        public decimal Amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Amount)));
            }
        }


        private decimal _commission;

        public decimal Commission
        {
            get
            {
                return _commission;
            }
            set
            {
                _commission = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Commission)));
            }
        }

        private string _tittle;

        public string Tittle
        {
            get
            {
                return _tittle;
            }
            set
            {
                if (_tittle != value)
                {
                    _tittle = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Tittle)));
                }
            }
        }

        private Visibility _visibilityComision;

        public Visibility VisibilityComision
        {
            get
            {
                return _visibilityComision;
            }
            set
            {
                if (_visibilityComision != value)
                {
                    _visibilityComision = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityComision)));
                }
            }
        }

        private Visibility _visibilityInput;

        public Visibility VisibilityInput
        {
            get
            {
                return _visibilityInput;
            }
            set
            {
                if (_visibilityInput != value)
                {
                    _visibilityInput = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityInput)));
                }
            }
        }

        private Visibility _visibilityQr;

        public Visibility VisibilityQr
        {
            get
            {
                return _visibilityQr;
            }
            set
            {
                if (_visibilityQr != value)
                {
                    _visibilityQr = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityQr)));
                }
            }
        }

        private Visibility _visibilityAcept;

        public Visibility VisibilityAcept
        {
            get
            {
                return _visibilityAcept;
            }
            set
            {
                if (_visibilityAcept != value)
                {
                    _visibilityAcept = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityAcept)));
                }
            }
        }

        private Visibility _visibilityAmount;

        public Visibility VisibilityAmount
        {
            get
            {
                return _visibilityAmount;
            }
            set
            {
                if (_visibilityAmount != value)
                {
                    _visibilityAmount = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityAmount)));
                }
            }
        }

        private Visibility _visibilityTxtImput;

        public Visibility VisibilityTxtImput
        {
            get
            {
                return _visibilityTxtImput;
            }
            set
            {
                if (_visibilityTxtImput != value)
                {
                    _visibilityTxtImput = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VisibilityTxtImput)));
                }
            }
        }

        private string _txtInput;

        public string TxtInput
        {
            get
            {
                return _txtInput;
            }
            set
            {
                if (_txtInput != value)
                {
                    _txtInput = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TxtInput)));
                }
            }
        }

        private string _lblInput;

        public string LblInput
        {
            get
            {
                return _lblInput;
            }
            set
            {
                if (_lblInput != value)
                {
                    _lblInput = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LblInput)));
                }
            }
        }

        private string _txtQr;

        public string TxtQr
        {
            get
            {
                return _txtQr;
            }
            set
            {
                if (_txtQr != value)
                {
                    _txtQr = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TxtQr)));
                }
            }
        }

        private ETypeDetailModel _type;

        public ETypeDetailModel Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (_type != value)
                {
                    _type = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Type)));
                }
            }
        }

        public bool IsReadQr { get; internal set; }

        public bool CallService(Transaction transaction)
        {
            Response response = null;
            try
            {
                Task.Run(async () =>
                {
                    if (this.Type == ETypeDetailModel.Withdrawal)
                    {
                        response = await AdminPayPlus.ApiIntegration.CallService(ETypeService.Generate_OTP, transaction);
                    }
                    else if (this.Type == ETypeDetailModel.CodeOTP)
                    {
                         response = await AdminPayPlus.ApiIntegration.CallService(ETypeService.Validate_OTP, transaction);
                    }
                    else
                    {
                        response = await AdminPayPlus.ApiIntegration.CallService(ETypeService.Consult_QR, transaction);
                    }

                    if (response != null)
                    {
                        transaction = (Transaction)response.Data;
                    }
                    

                    Utilities.CloseModal();
                });

                Utilities.ShowModal(MessageResource.LoadInformation, EModalType.Preload);
            }
            catch (Exception ex)
            {

            }
            return response != null ? true : false;
        }
    }
}