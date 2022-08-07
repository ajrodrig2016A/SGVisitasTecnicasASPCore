using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Models
{
    public class EmailRequestQuoteModel
    {
        [Required(ErrorMessage = "Ingrese los nombres.")]
        public string nombres { get; set; }

        [Required(ErrorMessage = "Ingrese los apellido.")]
        public string apellidos{ get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Ingrese su email.")]
        public string email { get; set; }

        [Required(ErrorMessage = "Ingrese el teléfono.")]
        [StringLength(20)]
        public string telefono { get; set; }

        [Required(ErrorMessage = "Seleccione el servicio que necesite solicitar.")]
        [StringLength(180)]
        public string servicio { get; set; }

        [Required(ErrorMessage = "Ingrese el mensaje.")]
        [StringLength(255)]
        public string mensaje { get; set; }

    }
}
