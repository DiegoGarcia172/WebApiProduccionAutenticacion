using Microsoft.EntityFrameworkCore;
using WebApiProduccionAutenticacion.Data;
using WebApiProduccionAutenticacion.Models;
using System.Diagnostics;
using System.Collections.Generic;

namespace WebApiProduccionAutenticacion.Data
{
    public class ProduccionDbContext : DbContext
    {
        public ProduccionDbContext(DbContextOptions<ProduccionDbContext> options) : base(options) { }

        public DbSet<Producto> Producto { get; set; }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Departamento> Departamento { get; set; }
        public DbSet<OrdenProduccion> OrdenProduccion { get; set; }
        public DbSet<Almacen> Almacen { get; set; }
        public DbSet<Rechazo> Rechazo { get; set; }
        public DbSet<Maquina> Maquina { get; set; }
        public DbSet<MateriaPrima> MateriaPrima { get; set; }
        public DbSet<Proceso> Proceso { get; set; }
        public DbSet<Inventario> Inventario { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
