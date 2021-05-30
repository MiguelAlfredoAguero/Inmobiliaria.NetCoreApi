using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiInmobiliaria.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<TipoPersona> TipoPersona { get; set; }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<Propietario> Propietario { get; set; }
        public DbSet<Agencia> Agencia { get; set; }
        public DbSet<Inquilino> Inquilino { get; set; }
        public DbSet<Garante> Garante { get; set; }
        public DbSet<TipoInmueble> TipoInmueble { get; set; }
        public DbSet<UsoInmueble> UsoInmueble { get; set; }
        public DbSet<Inmueble> Inmueble { get; set; }
        public DbSet<Contrato> Contrato { get; set; }
        public DbSet<Pago> Pago { get; set; }
    }
}
