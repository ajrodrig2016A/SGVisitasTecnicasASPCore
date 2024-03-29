﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class clientes
    {
        [Key]
        public int id_cliente { get; set; }

        [Required(ErrorMessage = "Ingrese un número de identificación válido.")]
        [StringLength(13)]
        public string numero_documento { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [Required(ErrorMessage = "Ingrese los nombres.")]
        [StringLength(128)]
        public string nombres { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [StringLength(128)]
        public string apellidos { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Seleccione la fecha de registro.")]
        public DateTime fecha_registro { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Seleccione la fecha de actualización.")]
        public DateTime fecha_actualizacion { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Seleccione el género.")]
        [StringLength(16)]
        public string genero { get; set; }

        [Required(ErrorMessage = "Ingrese la dirección.")]
        [StringLength(128)]
        public string direccion { get; set; }

        [Required(ErrorMessage = "Ingrese el email.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Ingrese el password.")]
        public string password { get; set; }

        [Required(ErrorMessage = "Ingrese el número de contacto.")]
        [StringLength(20)]
        public string numero_contacto { get; set; }

        public enum Gender
        {
            Masculino,
            Femenino
        }

    }
}
