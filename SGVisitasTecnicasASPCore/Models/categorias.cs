﻿using Microsoft.AspNetCore.Mvc.Rendering;
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

        [Required(ErrorMessage = "Ingrese el nombre.")]
        [StringLength(64)]
        public string nombre { get; set; }
        
        [Required(ErrorMessage = "Ingrese la descripción.")]
        [StringLength(int.MaxValue)]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "Seleccione el estado.")]
        [StringLength(64)]
        public string estado { get; set; }

        public enum Estados
        {
            Activa,
            Inactiva
        }
        [NotMapped]
        public SelectList CategoriasSelectList { get; set; }
    }
}
