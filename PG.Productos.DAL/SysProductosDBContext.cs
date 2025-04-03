using Microsoft.EntityFrameworkCore;
using PG.Productos.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Productos.DAL
{
    public class ProductosDBContext : DbContext
    {
        public ProductosDBContext(DbContextOptions<ProductosDBContext> options) : base(options)
        {
        }

        public DbSet<Producto> producto { get; set; }
        public DbSet<Proveedor>proveedores { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Venta> venta { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<DetalleCompra> DetalleCompras { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DetalleCompra>()
                .HasOne(d => d.Compra)
                .WithMany(c => c.DetalleCompras)
                .HasForeignKey(d => d.IdCompra);

            base.OnModelCreating(modelBuilder);
        }

    }


}
