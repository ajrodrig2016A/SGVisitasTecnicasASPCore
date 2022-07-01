using System;
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

        [Required(ErrorMessage = "Por favor ingrese un número de identificación válido.")]
        [StringLength(13)]
        public string numero_documento { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [Required(ErrorMessage = "Por favor ingrese los nombres.")]
        [StringLength(64)]
        public string nombres { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z0-9""'\s-]*$")]
        [Required(ErrorMessage = "Por favor ingrese los apellidos.")]
        [StringLength(64)]
        public string apellidos { get; set; }

        [Required(ErrorMessage = "Por favor seleccione la fecha de registro.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy - MM - dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_registro { get; set; }

        [Required(ErrorMessage = "Por favor seleccione el género.")]
        [StringLength(16)]
        public string genero { get; set; }

        [Required(ErrorMessage = "Por favor ingrese la dirección.")]
        [StringLength(128)]
        public string direccion { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el email.")]
        [StringLength(64)]
        public string email { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el password.")]
        [StringLength(20)]
        public string password { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el número de contacto.")]
        [StringLength(20)]
        public string numero_contacto { get; set; }

        public enum Gender
        {
            Masculino,
            Femenino
        }

        public virtual ICollection<visitas> Visitas { get; set; }
    }
}
