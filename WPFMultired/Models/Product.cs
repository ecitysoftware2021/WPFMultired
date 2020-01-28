using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFMultired.Models
{
    public class Product
    {
        public string idservicio { get; set; }
        public string matricula { get; set; }
        public string proponente { get; set; }
        public int cantidad { get; set; }
        public decimal basse { get; set; }
        public int porcentaje { get; set; }
        public decimal valorservicio { get; set; }
        public decimal valor { get; set; }
        public string anobase { get; set; }
        public string tipocertificado { get; set; }
        public string descripciontipocertificado { get; set; }
        public string descripcioncertificado { get; set; }
    }
}
