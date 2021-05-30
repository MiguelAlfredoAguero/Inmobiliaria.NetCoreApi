using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public interface IRepositorioPropietario : IRepositorio<Propietario>
    {
        public IList<Propietario> getAll(int id);
    }
}
