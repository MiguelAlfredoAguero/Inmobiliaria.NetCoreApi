using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class LoginView
    {
        [Display(Name = "Usuario")]
        [Required, DataType(DataType.EmailAddress)]
        public string User { get; set; }

        [Display(Name = "Contraseña")]
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
