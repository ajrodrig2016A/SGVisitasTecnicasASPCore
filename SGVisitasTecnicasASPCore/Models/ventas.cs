using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class ventas
    {
        [Key]
        public int id_venta { get; set; }

        [Required]
        [StringLength(10)]
        public string codigo_venta { get; set; }

        [Required]
        [StringLength(192)]
        public string numero_factura { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime fecha_creacion { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Seleccione la fecha de cierre.")]
        public DateTime fecha_cierre { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Seleccione estado de la venta.")]
        [StringLength(20)]
        public string estado { get; set; }

        [StringLength(int.MaxValue)]
        public string observaciones { get; set; }

        public virtual List<detalles_venta> DetallesVenta { get; set; } = new List<detalles_venta>();//detail very important

        public decimal subtotal { get; set; }

        public decimal Iva { get; set; }

        public decimal total { get; set; }

        [Required(ErrorMessage = "Seleccione el cliente.")]
        [ForeignKey("Cliente")]//very important
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
