using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFMultired.DataModel
{
    public partial class TRANSACTION_DESCRIPTION
    {
        public int TRANSACTION_DESCRIPTION_ID { get; set; }
        public int TRANSACTION_ID { get; set; }
        public string REFERENCE { get; set; }
        public Nullable<decimal> AMOUNT { get; set; }
        public string OBSERVATION { get; set; }
        public Nullable<bool> STATE { get; set; }
    }
}
