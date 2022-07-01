using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class categorias
    {
        [Key]
        public int id_categoria { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el nombre.")]
        [StringLength(64)]
        public string nombre { get; set; }
        
        [Required(ErrorMessage = "Por favor ingrese la descripción.")]
        [StringLength(int.MaxValue)]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el estado.")]
        [StringLength(64)]
        public string estado { get; set; }

        public enum Estados
        {
            Activa,
            Desactiva,
            No_disponible
        }
        [NotMapped]
        public SelectList CategorySelectList { get; set; }
        public virtual ICollection<productos> Productos { get; set; }
    }
}
