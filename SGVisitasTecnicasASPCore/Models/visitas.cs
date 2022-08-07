using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class visitas
    {
        [Key]
        public int id_visita { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy - MM - dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Seleccione la fecha agendada.")]
        public DateTime fecha_agendada { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy - MM - dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Seleccione la fecha de cierre.")]
        public DateTime fecha_cierre { get; set; }

        public bool esProblema { get; set; }
        public bool requiereInstalacion { get; set; }
        public bool requiereMantenimiento { get; set; }

        [Required(ErrorMessage = "Ingrese la descripción.")]
        [StringLength(int.MaxValue)]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "Ingrese ubicación del dispositivo de seguridad.")]
        [StringLength(120)]
        public string ubicacionDispSeguridad { get; set; }

        [Required(ErrorMessage = "Ingrese el tiempo de entrega.")]
        [StringLength(90)]
        public string tiempoEntrega { get; set; }

        [Required(ErrorMessage = "Seleccione el estado.")]
        [StringLength(50)]
        public string estado { get; set; }

        [Required(ErrorMessage = "Seleccione el empleado.")]
        [ForeignKey("Empleado")]
        public int id_empleado { get; set; }
        public virtual empleados Empleado { get; set; } //very important 

        [Required(ErrorMessage = "Seleccione el cliente.")]
        [ForeignKey("Cliente")]
        public int id_cliente { get; set; }
        public virtual clientes Cliente { get; set; } //very important
        public enum Status
        {
            Registrada,
            Actualizada,
            Aprobada,
            Cancelada
        }
    }
}
