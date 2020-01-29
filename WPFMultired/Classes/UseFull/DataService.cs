using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFMultired.Classes.UseFull
{
    public class DataService
    {
        public EServicio etypeService { get; set; }
        public string Language { get; set; }
        public string EntidadDestino { get; set; }
        public ETramite TypeTramite { get; set; }
        public string I_NUMDOC { get; set; }
        public string I_TIPDOC { get; set; }
        public string I_CTANRO { get; set; }
        public string I_CODSIS { get; set; }
        public string I_CODPRO { get; set; }
        public string I_REFTRN { get; set; }
        public string I_TOKTRN { get; set; }
        public string I_VLRTRN { get; set; }
        public string CodOTP { get; set; }
        public string Text_QR { get; set; }
        public object Objeto { get; set; }
    }
}
