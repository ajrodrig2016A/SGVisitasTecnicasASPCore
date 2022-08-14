using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class cotizaciones
    {
        [Key]
        public int id_cotizacion { get; set; }

        [Required]
        [StringLength(10)]
        public string codigo { get; set; }

        [Required(ErrorMessage = "Seleccione el tipo de servicio.")]
        [StringLength(100)]
        public string servicio { get; set; }

        [Required(ErrorMessage = "Ingrese sector del inmueble.")]
        [StringLength(60)]
        public string sector_inmueble { get; set; }

        [Required(ErrorMessage = "Ingrese dirección del inmueble.")]
        [StringLength(90)]
        public string direccion_inmueble { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Seleccione la fecha de registro.")]
        public DateTime fecha_registro { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Seleccione la fecha de actualización.")]
        public DateTime fecha_actualizacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Ingrese el teléfono.")]
        [StringLength(20)]
        public string telefono { get; set; }

        [Required(ErrorMessage = "Seleccione estado de la cotización.")]
        [StringLength(20)]
        public string estado { get; set; }

        public virtual List<detalles_cotizacion> DetallesCotizacion { get; set; } = new List<detalles_cotizacion>();//detail very important


        [Required(ErrorMessage = "Ingrese tiempo de entrega.")]
        [StringLength(160)]
        public string tiempo_entrega { get; set; }

        [Required(ErrorMessage = "Ingrese procedimiento de instalación.")]
        [StringLength(160)]
        public string instalacion { get; set; }

        [Required(ErrorMessage = "Ingrese garantía de los equipos.")]
        [StringLength(255)]
        public string garantia_equipos { get; set; }

        [Required(ErrorMessage = "Ingrese forma de pago.")]
        [StringLength(60)]
        public string forma_pago { get; set; }

        public decimal subtotal { get; set; }

        [Required(ErrorMessage = "Ingrese validez de la cotización.")]
        [StringLength(160)]
        public string validez { get; set; }

        [Required(ErrorMessage = "Ingrese observaciones.")]
        [StringLength(int.MaxValue)]
        public string observaciones { get; set; }

        [Required(ErrorMessage = "Seleccione el empleado.")]
        [ForeignKey("Empleado")]//very important
        public int id_empleado { get; set; }
        public virtual empleados Empleado { get; set; } //very important 

        [Required(ErrorMessage = "Seleccione el cliente.")]
        [ForeignKey("Cliente")]//very important
        public int id_cliente { get; set; }
        public virtual clientes Cliente { get; set; } //very important 

        public enum Service
        {
            Proyectos_de_Ingeniería,
            Venta_de_dispositivos_de_seguridad,
            Instalación_y_mantenimiento_de_dispositivos_de_seguridad
        }

        public enum Status
        {
            Registrado,
            Actualizado,
            Aprobado,
            Cancelado
        }

    }

}
