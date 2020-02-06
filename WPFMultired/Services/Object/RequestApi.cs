using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using WPFMultired.Classes;
using WPFMultired.Models;

namespace WPFMultired.Services.Object
{
    class RequestApi
    {
        public int Session { get; set; }

        public int User { get; set; }

        public object Data { get; set; }
    }

    public class RequestAuth
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public int Type { get; set; }
    }

    public class RequestTransactionDetails
    {
        public int TransactionId { get; set; }

        public int Denomination { get; set; }

        public int Operation { get; set; }

        public int Quantity { get; set; }

        public string Code { get; set; }

        public string Description { get; set; }
    }

}
