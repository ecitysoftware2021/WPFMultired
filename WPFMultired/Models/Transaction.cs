using System;
using System.Collections.Generic;
using System.ComponentModel;
using WPFMultired.Classes;
using WPFMultired.DataModel;
using WPFMultired.Services.Object;
using WPFMultired.ViewModel;

namespace WPFMultired.Models
{
    public class Transaction : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyRaised(string propertyname)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        public string nameentity { get; set; }

        public string consecutive { get; set; }

        public string reference { get; set; }

        public string Enrollment { get; set; }

        public string enrolled { get; set; }
        public string hand { get; set; }

        public string finger { get; set; }

        public string is_valid { get; set; }

        public ETypeServiceSelect eTypeService { get; set; }

        public bool IsCashBack { get; set; }

        public string CodeCompany { get; set; }

        public string CodeTOTP { get; set; }

        public string CodeTransactionAuditory { get; set; }
        public string Action { get; set; }
        public string TypeDocument { get; set; }

        public DateTime DateTransaction { get; set; }

        public PaymentViewModel Payment { get; set; }

        public bool IsReturn { get; set; }

        public ETransactionState State { get; set; }

        public string Observation { get; set; }

        public ETransactionType Type { get; set; }

        public string CodeTypeTransaction { get; set; }

        public PAYER payer { get; set; }

        public int StateNotification { get; set; }

        public List<Product> Products { get; set; }
        public DATOSADICIONALES DatosAdicionales { get; set; }

        public Product Product { get; set; }

        public decimal AmountComission { get; set; }

        private decimal _Amount;

        public decimal Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
                OnPropertyRaised("Amount");
            }
        }

        private int _transactionId { get; set; }

        public int TransactionId
        {
            get
            {
                return _transactionId;
            }
            set
            {
                _transactionId = value;
                OnPropertyRaised("TransactionId");
            }
        }

        private int _idTransactionAPi { get; set; }

        public int IdTransactionAPi
        {
            get
            {
                return _idTransactionAPi;
            }
            set
            {
                _idTransactionAPi = value;
                OnPropertyRaised("IdTransactionAPi");
            }
        }

        public string Finger_Byte { get; set; }
        public string TOTP { get; set; }
        public string Image { get; set; }
        public int ImageLength { get; set; }
        public string ImageName { get; set; }
    }
}
