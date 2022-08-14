using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class detalles_venta
    {
        public detalles_venta()
        {

        }

        [Key]
        public int id_detalle_venta { get; set; }

        [Required]                
        [ForeignKey("Venta")]//very important
        public int id_venta { get; set; }
        public virtual ventas Venta { get; private set; } //very important 

        [Required(ErrorMessage = "Seleccione un producto.")]
        [ForeignKey("Producto")]
        public int id_producto { get; set; }
        public virtual productos Producto { get; private set; } //very important 

        [NotMapped]
        [StringLength(10)]
        public string codigoProductoVta { get; set; }

        [NotMapped]
        [StringLength(int.MaxValue)]
        public string descripcion { get; set; }


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
    }
}
