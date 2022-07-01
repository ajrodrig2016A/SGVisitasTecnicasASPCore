using System;
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

        [Required(ErrorMessage = "Por favor ingrese un número de identificación válido.")]
        [StringLength(13)]
        public string numero_documento { get; set; }
        
        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required(ErrorMessage = "Por favor ingrese los nombres.")]
        [StringLength(64)]
        public string nombres { get; set; }

        [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
        [Required(ErrorMessage = "Por favor ingrese los apellidos.")]
        [StringLength(64)]
        public string apellidos { get; set; }

        [Required(ErrorMessage = "Por favor seleccione la fecha de registro.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy - MM - dd}", ApplyFormatInEditMode = true)]
        public DateTime fecha_registro { get; set; }

        public bool es_activo { get; set; }
        
        [Required(ErrorMessage = "Por favor ingrese el email.")]
        [StringLength(64)]
        public string email { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el teléfono.")]
        [StringLength(20)]
        public string telefono { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el password.")]
        [StringLength(20)]
        public string password { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el perfil.")]
        [StringLength(3)]
        public string perfil { get; set; }

        [Required(ErrorMessage = "Por favor ingrese el cargo.")]
        [StringLength(32)]
        public string cargo { get; set; }

        public enum Profile
        {
            ADM,
            TEC,
            COM
        }
        public virtual ICollection<visitas> Visitas { get; set; }
    }
}
