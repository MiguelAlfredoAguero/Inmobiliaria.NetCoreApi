using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class InmuebleView
    {
        public int Id { get; set; }
        public String Direccion { get; set; }
        public String UsoInmueble { get; set; }
        public String TipoInmueble { get; set; }
        public int Ambientes { get; set; }
        public double Precio { get; set; }
        public PropietarioView Propietario { get; set; }
        public Boolean Estado { get; set; }
        public String Avatar { get; set; }


        public InmuebleView(Inmueble inmueble)
        {
            this.Id = inmueble.Id;
            this.Direccion = inmueble.Direccion;
            this.UsoInmueble = inmueble.UsoInmueble.Nombre;
            this.TipoInmueble = inmueble.TipoInmueble.Nombre;
            this.Ambientes = inmueble.Ambientes;
            this.Precio = long.Parse(inmueble.Precio+"");
            this.Propietario.Id = inmueble.PropietarioId;
            this.Estado = inmueble.Disponible;
            this.Avatar = inmueble.Avatar;
        }


    }
}
