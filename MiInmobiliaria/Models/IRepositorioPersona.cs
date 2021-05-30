using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public interface IRepositorioPersona : IRepositorio<Persona>
    {
        public Persona getByDniEmail(string dni, string email);
    }
}
