using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string email { get; set; }
    }
}
