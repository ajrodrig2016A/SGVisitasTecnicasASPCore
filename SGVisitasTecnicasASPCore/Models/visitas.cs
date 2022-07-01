using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class visitas
    {
        [Key]
        public int id_visita { get; set; }
        [Required]
        public DateTime fecha_agendada { get; set; }
        public DateTime fecha_cierre { get; set; }
        [StringLength(120)]
        public string requerimiento { get; set; }
        public string descripcion_req { get; set; }
        [StringLength(90)]
        public string tiempo_entrega { get; set; }
        [StringLength(120)]
        public string ubicacion_disp_seguridad { get; set; }
        public bool requiere_instalacion { get; set; }
        public bool requiere_mantenimiento { get; set; }
        public string descripcion_problema { get; set; }
        [StringLength(50)]
        public string estado { get; set; }
        public int empleado_id { get; set; }
        public virtual empleados Empleado { get; set; }
        public int cliente_id { get; set; }
        public virtual clientes Cliente { get; set; }
    }
}
