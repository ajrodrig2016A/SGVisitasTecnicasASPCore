using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class RecoveryPasswordViewModel
    {
        public string token { get; set; }
        [Required(ErrorMessage = "Escriba su nueva contraseña.")]
        public string Password { get; set; }
        [Compare("Password")]
        [Required(ErrorMessage = "Ingrese nuevamente su nueva contraseña.")]
        public string Password2 { get; set; }
    }
}
