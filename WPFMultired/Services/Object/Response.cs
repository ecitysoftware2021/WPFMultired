using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using WPFMultired.Classes;
using WPFMultired.Models;

namespace WPFMultired.Services.Object
{
    public class Response
    {
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public class PaypadOperationControl : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Attributes
        public int ID { get; set; }

        public string DATE { get; set; }

        public string TIME { get; set; }

        public string ID_TRANSACTION { get; set; }

        public string CODE_AGENCY { get; set; }

        public ETypeAdministrator TYPE { get; set; }

        public string NAME_AGENCY { get; set; }

        public string NAME_BANCK { get; set; }

        public string SAFKEY { get; set; }

        public string ID_USER { get; set; }

        public int USER_ADMIN_ID { get; set; }

        public string NAME_USER { get; set; }

        public string NAME_COMPANY { get; set; }

        public decimal TOTAL_CURRENT;

        private decimal _TOTAL;

        private string _DESCRIPTION;

        private List<List> _DATALIST;

        private CollectionViewSource _viewList;

        public CollectionViewSource viewList
        {
            get { return _viewList; }
            set
            {
                _viewList = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(viewList)));
            }
        }

        public List<List> DATALIST
        {
            get { return _DATALIST; }
            set
            {
                _DATALIST = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DATALIST)));
            }
        }

        public decimal TOTAL
        {
            get { return _TOTAL; }
            set
            {
                _TOTAL = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOTAL)));
            }
        }

        public string DESCRIPTION
        {
            get { return _DESCRIPTION; }
            set
            {
                _DESCRIPTION = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DESCRIPTION)));
            }
        }
        #endregion

        //public List<DenominationMoney> DATALIST_FILTER()
        //{
        //    List<DenominationMoney> dataListsNew = new List<DenominationMoney>();
        //    try
        //    {
        //        foreach (var item in this.DATALIST)
        //        {
        //            if (((decimal)item.AMOUNT_NEW + (decimal)item.AMOUNT) > 0)
        //            {
        //                var itemUpdate = dataListsNew.Where(d => d.Denominacion == item.VALUE).FirstOrDefault();
        //                if (itemUpdate != null)
        //                {
        //                    itemUpdate.Quantity += (decimal)item.AMOUNT;
        //                    itemUpdate.Total = (int)itemUpdate.Quantity * (int)itemUpdate.Denominacion;
        //                }
        //                else
        //                {
        //                    dataListsNew.Add(new DenominationMoney
        //                    {
        //                        Denominacion = (decimal)item.VALUE,
        //                        Quantity = (decimal)item.AMOUNT_NEW + (decimal)item.AMOUNT,
        //                        Total = item.TOTAL_AMOUNT
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return dataListsNew;
        //}

        public List<DenominationMoney> DATALIST_FILTER(ETypeAdministrator type)
        {
            List<DenominationMoney> dataListsNew = new List<DenominationMoney>();
            try
            {
                foreach (var item in this.DATALIST)
                {
                    if (((decimal)item.AMOUNT_NEW + (decimal)item.AMOUNT) > 0)
                    {
                        if (type == ETypeAdministrator.Upload)
                        {
                            dataListsNew.Add(new DenominationMoney
                            {
                                Denominacion = (decimal)item.VALUE,
                                Quantity = (decimal)item.AMOUNT_NEW + (decimal)item.AMOUNT,
                                Total = item.TOTAL_AMOUNT,
                                Code = item.CASSETTE.ToString(),
                            });
                        }
                        else if (type == ETypeAdministrator.Balancing && (item.CODE == "MA" || item.CODE == "AP"))
                        {
                            dataListsNew.Add(new DenominationMoney
                            {
                                Denominacion = (decimal)item.VALUE,
                                Quantity = (decimal)item.AMOUNT_NEW + (decimal)item.AMOUNT,
                                Total = item.TOTAL_AMOUNT,
                                Code = item.CASSETTE.ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return dataListsNew;
        }
    }

    public class List : INotifyPropertyChanged
    {
        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private int? _AMOUNT_NEW;

        private int? _AMOUNT;

        public int? ID { get; set; }

        public int? DEVICE_PAYPAD_ID { get; set; }

        private string _DESCRIPTION;

        private string _IMAGE;

        private decimal? _VALUE;

        public int? CURRENCY_DENOMINATION_ID { get; set; }

        public int? DEVICE_TYPE_ID { get; set; }

        public string CODE { get; set; }

        public int CASSETTE { get; set; }

        private decimal _TOTAL_AMOUNT;

        public int? AMOUNT_NEW
        {
            get { return _AMOUNT_NEW; }
            set
            {
                _AMOUNT_NEW = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AMOUNT_NEW)));
            }
        }

        public string IMAGE
        {
            get { return _IMAGE; }
            set
            {
                _IMAGE = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IMAGE)));
            }
        }

        public int? AMOUNT
        {
            get { return _AMOUNT; }
            set
            {
                _AMOUNT = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AMOUNT)));
            }
        }

        public string DESCRIPTION
        {
            get { return _DESCRIPTION; }
            set
            {
                _DESCRIPTION = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DESCRIPTION)));
            }
        }

        public decimal? VALUE
        {
            get { return _VALUE; }
            set
            {
                _VALUE = value;
                TOTAL_AMOUNT = (int)_AMOUNT_NEW * (int)value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VALUE)));
            }
        }

        public decimal TOTAL_AMOUNT
        {
            get { return _TOTAL_AMOUNT; }
            set
            {
                _TOTAL_AMOUNT = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TOTAL_AMOUNT)));
            }
        }
    }

    public class DataQR
    {
        public string TIPTRN { get; set; }
        public string DESTRN { get; set; }
        public string TOKTRN { get; set; }
        public string REFTRN { get; set; }
        public string VLRTRN { get; set; }
        public string CTANRO { get; set; }
        public string CODSIS { get; set; }
        public string CODPRO { get; set; }
        public string PARSER { get; set; }
        public string NOMPRO { get; set; }
        public string CTAMAS { get; set; }
        public string COMISI { get; set; }
        public string TIPDOC { get; set; }
        public string NRONIT { get; set; }
        public string NOMCLI { get; set; }
        public string MONMAX { get; set; }
        public string MONMIN { get; set; }
        public string RESULT { get; set; }
        public string ENTDST { get; set; }
        public string NOMDST { get; set; }
    }

}
