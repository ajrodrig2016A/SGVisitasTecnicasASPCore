using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class productos
    {
        [Key]
        public int id_producto { get; set; }

        [Required(ErrorMessage = "Ingrese el nombre.")]
        [StringLength(90)]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Ingrese la marca.")]
        [StringLength(60)]
        public string marca { get; set; }

        [Required(ErrorMessage = "Ingrese la descripción.")]
        [StringLength(int.MaxValue)]
        public string descripcion { get; set; }

        [DisplayName("Image Name")]
        public string ImageName { get; set; }

        [NotMapped]
        [DisplayName("Subir imagen")]
        public IFormFile ImageFile { get; set; }

        [Required(ErrorMessage = "Ingrese la unidad.")]
        [StringLength(8)]
        public string unidad { get; set; }

        public decimal cantidad { get; set; }

        public decimal precioUnitario { get; set; }

        public decimal stock { get; set; }

        public decimal descuento { get; set; }

        public decimal porcentaje { get; set; }
        [Required(ErrorMessage = "Seleccione la categoría.")]
        public int categoria_id { get; set; }

        public categorias Categoria { get; set; }

    }
}
