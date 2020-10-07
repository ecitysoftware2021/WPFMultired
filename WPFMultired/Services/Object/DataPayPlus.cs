using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFMultired.Models;

namespace WPFMultired.Services.Object
{
    public class DataPayPlus
    {
        public bool State { get; set; }

        public bool StateAceptance { get; set; }

        public bool StateDispenser { get; set; }

        public string Message { get; set; }

        public bool StateBalanece { get; set; }

        public bool StateUpload { get; set; }

        public bool StateUpdate { get; set; }

        public bool StateDiminish { get; set; }

        public object ListImages { get; set; }

        public List<ItemList> ListCompanies { get; set; }

        public List<ItemList> ListTypeTransactions { get; set; }

        public int IdiomId { get; set; }

        public object ListIdioms { get; set; }

        public int ContTransactionsNotific { get; set; }

        public int ContTransactions { get; set; }
    }
}
