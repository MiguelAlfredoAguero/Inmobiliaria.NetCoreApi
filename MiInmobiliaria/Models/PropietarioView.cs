using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class PropietarioView
    {
        public int Id { get; set; }
        public long Dni { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Contraseña { get; set; }
        public string Telefono { get; set; }
        public String FechaNac { get; set; }




        public PropietarioView(Propietario propietario)
        {
            this.Id = propietario.Id;
            this.Dni = long.Parse(propietario.Persona.Dni);
            this.Nombre = propietario.Persona.Nombre;
            this.Apellido = propietario.Persona.Apellido;
            this.Email = propietario.Persona.Email;
            this.Contraseña = "";
            this.Telefono = propietario.Persona.Telefono;
            this.FechaNac = propietario.Persona.FechaNac.ToString();
        }

        public PropietarioView()
        {

        }


    }

}
