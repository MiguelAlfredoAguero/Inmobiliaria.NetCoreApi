using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public interface IRepositorioPago : IRepositorio<Pago>
    {
        public int numeroUltimoPago(int ContratoId);
        public IList<Pago> getAll(int ContratoId);
    }
}
