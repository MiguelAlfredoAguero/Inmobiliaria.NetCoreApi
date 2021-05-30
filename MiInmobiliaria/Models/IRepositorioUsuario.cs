using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public interface IRepositorioUsuario : IRepositorio<Usuario>
    {
        public Usuario getByEmail(string email);
    }
}
