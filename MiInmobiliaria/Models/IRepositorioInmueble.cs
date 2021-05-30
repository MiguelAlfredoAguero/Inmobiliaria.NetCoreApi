using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public interface IRepositorioInmueble : IRepositorio<Inmueble>
    {
        public IList<Inmueble> getAllDisponibles();
        public IList<Inmueble> getAll(Propietario p);
        public IList<Inmueble> getAll(Agencia a);
        public IList<Inmueble> getDesocupados(DateTime desde, DateTime hasta);
        public Inmueble getDesocupado(DateTime desde, DateTime hasta, int InmuebleId);
        public IList<Inmueble> Busqueda(int UsoInmuebleId, int TipoInmuebleId, int ambientes, DateTime desde, DateTime hasta, decimal minimo, decimal maximo);
    }
}
