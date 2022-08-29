using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class ventasReporte
    {
        public string fecha_creacion { get; set; }
        public string fecha_cierre { get; set; }
        public string cliente { get; set; }
        public string codigo { get; set; }
        public string nroFactura { get; set; }
        public decimal subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal total { get; set; }
        public string estado { get; set; }

    }
}

