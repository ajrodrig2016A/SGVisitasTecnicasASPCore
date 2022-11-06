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

        [Required]                
        [ForeignKey("Cotizacion")]//very important
        public int id_cotizacion { get; set; }
        public virtual cotizaciones Cotizacion { get; private set; } //very important 

        [Required(ErrorMessage = "Seleccione un producto.")]
        [ForeignKey("Producto")]
        public int id_producto { get; set; }
        public virtual productos Producto { get; private set; } //very important 

        [NotMapped]
        [StringLength(10)]
        public string idProducto { get; set; }

        [NotMapped]
        [StringLength(10)]
        public string codigoProducto { get; set; }

        [NotMapped]
        [StringLength(int.MaxValue)]
        public string descripcion { get; set; }

        [StringLength(90)]
        public string ubicacion { get; set; }

        [NotMapped]
        //[Required(ErrorMessage = "Ingrese la marca.")]
        [StringLength(60)]
        public string marca { get; set; }

        [NotMapped]
        //[Required(ErrorMessage = "Ingrese la unidad.")]
        [StringLength(8)]
        public string unidad { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode =true)]
        [Range(0.01, Double.MaxValue, ErrorMessage = "La cantidad debe ser mayor que cero.")]
        public decimal cantidad { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        [Range(0.01, Double.MaxValue, ErrorMessage = "El valor unitario debe ser mayor que cero.")]
        public decimal valorUnitario { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Range(0.00, Double.MaxValue, ErrorMessage = "El descuento del detalle debe ser mayor o igual que cero y menor que uno.")]
        public decimal descuento { get; set; }

        [DisplayFormat(DataFormatString = "{0:0.0000}", ApplyFormatInEditMode = true)]
        [Range(0.01, Double.MaxValue, ErrorMessage = "El valor total debe ser mayor que cero.")]
        public decimal valorTotal { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; } = false;
        [NotMapped]
        public bool IsSelected { get; set; } = false;
    }
}
