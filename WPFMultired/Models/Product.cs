using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFMultired.Models
{
    public class Product
    {
        public string Code { get; set; }
        public string CodeSystem { get; set; }
        public string AcountNumber { get; set; }
        public decimal AmountMax { get; set; }
        public decimal AmountMin { get; set; }
        public decimal Amount { get; set; }
        public decimal AmountUser { get; set; }
        public string TypeTransaction { get; set; }
        public decimal AmountTotal { get; set; }
        public decimal AmountCommission { get; set; }
        public string Description { get; set; }
        public string AcountNumberMasc { get; set; }
        public string img { get; set; }
        public AccountStateProduct AccountStateProduct { get; set; }
        public DataExtraTarjetaCredito ExtraTarjetaCredito { get; set; }
    }

    public class AccountStateProduct
    {
        public string ICONOS { get; set; }
        public string CTACRE { get; set; }
        public decimal VLRCRE { get; set; }
        public int NROSEG { get; set; }
        public string DESCRE { get; set; }
        public int FLGAPO { get; set; }
        public string CTAAPO { get; set; }
        public string SEGAPO { get; set; }
        public decimal VLRAPO { get; set; }
        public string DESAPO { get; set; }
        public int FLGPAP { get; set; }
        public string CTAPAP { get; set; }
        public string SEGPAP { get; set; }
        public decimal VLRPAP { get; set; }
        public string DESPAP { get; set; }
        public bool FLGHON { get; set; }
    }

    public class DataExtraTarjetaCredito
    {
        public string ICONOS { get; set; }
        public string NUMTAR { get; set; }
        public string SALMIN { get; set; }
        public int NROSEG { get; set; }
        public string DESTAR { get; set; }
        public bool FLGHON { get; set; }
    }

}
