using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public interface IRepositorio<T>
    {
        int Create(T p);
        int Edit(T p);
        int Delete(int id);
        IList<T> getAll();
        T getById(int id);

    }
}
