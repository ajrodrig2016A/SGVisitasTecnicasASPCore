using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class visitasReporte
    {
        public string fecha_agendada { get; set; }
        public string fecha_cierre { get; set; }
        public string cliente { get; set; }
        public string descripcion { get; set; }
        public string ubicacionDispSeguridad { get; set; }
        public string tiempoEntrega { get; set; }
        public string estado { get; set; }
        public string observaciones { get; set; }

    }
}
