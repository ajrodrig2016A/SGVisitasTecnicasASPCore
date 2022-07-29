using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class detalles_cotizacion
    {
        public detalles_cotizacion()
        {

        }

        [Key]
        public int id_detalle_cotización { get; set; }
                
        [ForeignKey("Cotizacion")]//very important
        public int id_cotizacion { get; set; }
        public virtual cotizaciones Cotizacion { get; private set; } //very important 

        //[ForeignKey("id_producto")]//very important
        //public virtual productos Producto { get; set; } //very important 

        [Required(ErrorMessage = "Ingrese la descripción.")]
        [StringLength(int.MaxValue)]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "Ingrese la ubicación.")]
        [StringLength(90)]
        public string ubicación { get; set; }

        [Required(ErrorMessage = "Ingrese la marca.")]
        [StringLength(60)]
        public string marca { get; set; }

        [Required(ErrorMessage = "Ingrese la unidad.")]
        [StringLength(8)]
        public string unidad { get; set; }
        [Range(0.01, Double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public decimal cantidad { get; set; }
        [Range(0.01, Double.MaxValue, ErrorMessage = "El valor unitario debe ser mayor que cero.")]
        public decimal valorUnitario { get; set; }
        [Range(0.01, Double.MaxValue, ErrorMessage = "El valor total debe ser mayor que cero.")]
        public decimal valorTotal { get; set; }

        //[NotMapped]
        //public bool IsDeleted { get; set; } = false;
    }
}
