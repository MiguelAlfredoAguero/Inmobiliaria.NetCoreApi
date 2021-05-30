using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class Inmueble
    {
        [Display(Name = "Código")]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Direccion { get; set; }
        [Required]
        public int Ambientes { get; set; }
        [Required]
        public decimal Precio { get; set; }
        
        public int UsoInmuebleId { get; set; }
        [ForeignKey("UsoInmuebleId")]
        [Display(Name = "Uso")]
        public UsoInmueble UsoInmueble { get; set; }

        public int TipoInmuebleId { get; set; }
        [ForeignKey("TipoInmuebleId")]
        [Display(Name = "Tipo")]
        public TipoInmueble TipoInmueble { get; set; }

        public bool Disponible { get; set; }
        [Display(Name = "Disponible")]

        public int PropietarioId { get; set; }
        [ForeignKey("PropietarioId")]
        public Propietario Propietario { get; set; }

        public int AgenciaId { get; set; }
        [ForeignKey("AgenciaId")]
        public Agencia Agencia { get; set; }

        public string Avatar { get; set; }
        [Display(Name = "Avatar")]
        [NotMapped]
        public IFormFile AvatarFile { get; set; }
    }
}
