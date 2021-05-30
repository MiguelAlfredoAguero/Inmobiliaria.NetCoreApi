using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class Contrato
    {
        [Key, Display(Name = "Código")]
        public int Id { get; set; }
        public int InmuebleId { get; set; }
        [ForeignKey("InmuebleId")]
        public Inmueble Inmueble { get; set; }
        public int InquilinoId { get; set; }
        [ForeignKey("InquilinoId")]
        public Inquilino Inquilino { get; set; }
        public int GaranteId { get; set; }
        [ForeignKey("GaranteId")]
        public Garante Garante { get; set; }
        [DataType(DataType.Date)]
        public DateTime Desde { get; set; }
        [DataType(DataType.Date)]
        public DateTime Hasta { get; set; }
        [NotMapped]
        public int CantidadMeses { get; set; }
        public decimal Precio { get; set; }
    }
}
