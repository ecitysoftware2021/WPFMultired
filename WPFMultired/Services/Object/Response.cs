﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using WPFMultired.Classes;

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

        public decimal TOTAL_NEW;

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

        public List<List> DATALIST_FILTER(ETypeAdministrator type)
        {
            try
            {
                if (type == ETypeAdministrator.Upload)
                {
                    return this.DATALIST.Where(i => (i.DEVICE_TYPE_ID == (int)ETypeDevice.DP) || (i.DEVICE_TYPE_ID == (int)ETypeDevice.MD)).ToList();
                }
                else if (type == ETypeAdministrator.Balancing)
                {
                    return this.DATALIST.Where(i => (i.DEVICE_TYPE_ID == (int)ETypeDevice.AP) || (i.DEVICE_TYPE_ID == (int)ETypeDevice.MA)).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return null;
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

        private int? _VALUE;

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

        public int? VALUE
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
    public class DATAINIT
    {
        public int O_CODIGOERROR { get; set; }
        public string O_MENSAJEERROR { get; set; }
        public int O_MOVIMIENTO { get; set; }
        public string O_DESCRIPCION { get; set; }
        public bool O_STATUSACEPTADOR { get; set; }
        public bool O_STATUSDISPENSER { get; set; }
    }
    public class DATOSADICIONALES
    {
        public string DESFOR { get; set; }
        public string FORPAG { get; set; }
        public string DESCLI { get; set; }
        public string NOMCLI { get; set; }
        public string DESDOC { get; set; }
        public string NRONIT { get; set; }
        public string DESPRO { get; set; }
        public string NROPRO { get; set; }
        public string ESTTRN { get; set; }
        public string DESEST { get; set; }
        public string DESAPR { get; set; }
        public string NROAPR { get; set; }
        public string DESAPL { get; set; }
        public string VLRAPL { get; set; }
        public bool FLGAPO { get; set; }
        public string DESAPO { get; set; }
        public string VLRAPO { get; set; }
        public bool FLGPAP { get; set; }
        public string DESPAP { get; set; }
        public string VLRPAP { get; set; }
        public string DESCOM { get; set; }
        public string VLRCOM { get; set; }
        public string DESEXC { get; set; }
        public string VLREXC { get; set; }
        public bool FLGHON { get; set; }
        public string DESHON { get; set; }
        public string VLRHON { get; set; }
        public string DESTOT { get; set; }
        public string VLRTOT { get; set; }
    }
}
