using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class Propietario
    {
        [Key, Display(Name = "Código")]
        public int Id { get; set; }
        public int PersonaId { get; set; }
        [ForeignKey("PersonaId")]
        public Persona Persona { get; set; }
        public bool Activo { get; set; }
    }
}
