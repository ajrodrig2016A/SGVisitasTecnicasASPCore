using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class unidades
    {
        [Key]
        public int id_unidad { get; set; }

        [Required(ErrorMessage = "Ingrese el nombre.")]
        [StringLength(64)]
        public string nombre { get; set; }
        
        [Required(ErrorMessage = "Ingrese la descripción.")]
        [StringLength(192)]
        public string descripcion { get; set; }

    }
}
