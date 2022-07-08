using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class Usuario
    {
        [Key]
        public int id_usuario { get; set; }
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingrese su email.")]
        public string Correo { get; set; }
        [Required(ErrorMessage = "Ingrese su contraseña.")]
        public string Clave { get; set; }

        public string token_recovery { get; set; }
        public string Rol { get; set; }
    }
}
