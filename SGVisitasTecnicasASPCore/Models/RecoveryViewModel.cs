using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class RecoveryViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "Escriba su email.")]
        public string email { get; set; }
    }
}
