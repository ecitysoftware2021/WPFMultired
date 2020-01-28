using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFMultired.Models;

namespace WPFMultired.Services
{
    public class Request
    {
        public string codigoempresa { get; set; }
        public string usuariows { get; set; }
        public string clavews { get; set; }
    }

    public class RequestSearch
    {
        public string codigoempresa { get; set; }
        public string usuariows { get; set; }
        public string token { get; set; }
        public string identificacion { get; set; }
        public int matriculainicial { get; set; }
        public string nombreinicial { get; set; }
        public string semilla { get; set; }
    }

    public class RequestTransaction
    {
        public string codigoempresa { get; set; }
        public string usuariows { get; set; }
        public string token { get; set; }
        public string operador { get; set; }
        public string emailcontrol { get; set; }
        public string identificacioncontrol { get; set; }
        public string nombrecontrol { get; set; }
        public string celularcontrol { get; set; }
        public string codificacionservicios { get; set; }
        public string tipoidentificacioncliente { get; set; }
        public string identificacioncliente { get; set; }
        public string razonsocialcliente { get; set; }
        public string nombre1cliente { get; set; }
        public string nombre2cliente { get; set; }
        public string apellido1cliente { get; set; }
        public string apellido2cliente { get; set; }
        public string emailcliente { get; set; }
        public string direccioncliente { get; set; }
        public string telefonocliente { get; set; }
        public string celularcliente { get; set; }
        public string municipiocliente { get; set; }
        public decimal valorbruto { get; set; }
        public decimal Valorbaseiva { get; set; }
        public decimal Valoriva { get; set; }
        public decimal valortotal { get; set; }
        public string tipotramite { get; set; }
        public string subtipotramite { get; set; }
        public string proyecto { get; set; }
        public List<Product> servicios { get; set; }

    }

    public class RequestPayment
    {
        public string codigoempresa { get; set; }
        public string usuariows { get; set; }
        public string token { get; set; }
        public string operador { get; set; }
        public string identificacioncontrol { get; set; }
        public string nombrecontrol { get; set; }
        public string emailcontrol { get; set; }
        public string celularcontrol { get; set; }
        public string idliquidacion { get; set; }
        public string numerorecuperacion { get; set; }
        public string valorpagado { get; set; }
        public string fechapago { get; set; }
        public string horapago { get; set; }
        public string formapago { get; set; }
        public string numeroautorizacion { get; set; }
        public string idbanco { get; set; }
        public string idfranquicia { get; set; }
        public string codigofirmapdf { get; set; }
    }
}
