﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class empleados
    {
        [Key]
        public int id_empleado { get; set; }

        [Required(ErrorMessage = "Ingrese un número de identificación válido.")]
        [StringLength(13)]
        public string numero_documento { get; set; }
        
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required(ErrorMessage = "Ingrese los nombres.")]
        [StringLength(64)]
        public string nombres { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required(ErrorMessage = "Ingrese los apellidos.")]
        [StringLength(64)]
        public string apellidos { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy - MM - dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Seleccione la fecha de registro.")]
        public DateTime fecha_registro { get; set; } //= DateTime.Now;

        public bool es_activo { get; set; }
        
        [Required(ErrorMessage = "Ingrese el email.")]
        [StringLength(64)]
        public string email { get; set; }

        [Required(ErrorMessage = "Ingrese el teléfono.")]
        [StringLength(20)]
        public string telefono { get; set; }

        [Required(ErrorMessage = "Ingrese el password.")]
        [StringLength(20)]
        public string password { get; set; }

        [Required(ErrorMessage = "Seleccione el perfil.")]
        [StringLength(3)]
        public string perfil { get; set; }

        [Required(ErrorMessage = "Ingrese el cargo.")]
        [StringLength(32)]
        public string cargo { get; set; }

        public enum Profile
        {
            ADM,
            TEC,
            COM
        }

    }
}
