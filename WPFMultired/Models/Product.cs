﻿using System;
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
        public decimal AmountCommission { get; set; }
        public string Description { get; set; }
        public string AcountNumberMasc { get; set; }
    }
}
