using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class Pago
    {
        [Key, Display(Name = "Código")]
        public int Id { get; set; }
        public int Numero { get; set; } = 1;

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; } = System.DateTime.Today;
        public decimal Importe { get; set; }
        public int ContratoId { get; set; }

        [ForeignKey("ContratoId")]
        public Contrato Contrato { get; set; }
    }
}
