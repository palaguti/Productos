using Microsoft.EntityFrameworkCore;
using PG.Productos.EN.Filtros;
using PG.Productos.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Productos.DAL
{
    public class VentaDAL
    {
        readonly ProductosDBContext dbContext;
        public VentaDAL(ProductosDBContext productosDB)
        {
            dbContext = productosDB;
        }

        public async Task<int> CrearAsync(Venta pVenta)
        {
            dbContext.venta.Add(pVenta);
            int result = await dbContext.SaveChangesAsync();
            if (result > 0)
            {
                foreach (var detalle in pVenta.DetalleVentas)
                {
                    var productos = await dbContext.producto.FirstOrDefaultAsync(p => p.Id == detalle.IdProducto);
                    if (productos != null)
                    {
                        productos.CantidadDisponible -= detalle.Cantidad;
                    }
                }
            }
            return await dbContext.SaveChangesAsync();
        }
        public async Task<int> AnularAsync(int idVenta)
        {
            var venta = await dbContext.venta
                .Include(c => c.DetalleVentas)
                .FirstOrDefaultAsync(c => c.Id == idVenta);
            if (venta != null && venta.Estado != (byte)Venta.EnumEstadoVentas.Anulada)
            {
                venta.Estado = (byte)Venta.EnumEstadoVentas.Anulada;

                foreach (var detalle in venta.DetalleVentas)
                {
                    var producto = await dbContext.producto.FirstOrDefaultAsync(p => p.Id == detalle.IdProducto);
                    if (producto != null)
                    {
                        producto.CantidadDisponible += detalle.Cantidad;
                    }
                }
                return await dbContext.SaveChangesAsync();
            }
            return 0;
        }
        public async Task<Venta> ObtenerPorIdAsync(int idVenta)
        {
            var venta = await dbContext.venta
                .Include(c => c.DetalleVentas).Include(c => c.Cliente)
                .FirstOrDefaultAsync(c => c.Id == idVenta);
            return venta ?? new Venta();
        }
        public async Task<List<Venta>> ObtenerTodosAsync()
        {
            var venta = await dbContext.venta
                .Include(c => c.DetalleVentas)
                .Include(c => c.Cliente).ToListAsync();
            return venta ?? new List<Venta>();
        }
        public async Task<List<Venta>> ObtenerPorEstadoAsync(byte estado)
        {
            var ventasQuery = dbContext.venta.AsQueryable();
            if (estado != 0)
            {
                ventasQuery = ventasQuery.Where(c => c.Estado == estado);
            }
            ventasQuery = ventasQuery
                .Include(c => c.DetalleVentas)
                .Include(c => c.Cliente);
            var venta = await ventasQuery.ToListAsync();

            return venta ?? new List<Venta>();

        }
        public async Task<List<Venta>> ObtenerReporteComprasAsync(CompraFiltros filtro)
        {
            var comprasQuery = dbContext.venta
                .Include(c => c.DetalleVentas)
                    .ThenInclude(dc => dc.Producto)
                .Include(c => c.Cliente)
                .AsQueryable();

            if (filtro.FechaInicio.HasValue)
            {

                DateTime fechaInicio = filtro.FechaInicio.Value.Date.ToUniversalTime();
                comprasQuery = comprasQuery.Where(c => c.FechaVenta >= fechaInicio);
            }
            if (filtro.FechaFin.HasValue)
            {
                DateTime fechaFin = filtro.FechaFin.Value.Date.AddDays(1).AddSeconds(-1).ToUniversalTime();
                comprasQuery = comprasQuery.Where(c => c.FechaVenta >= fechaFin);
            }
            return await comprasQuery.ToListAsync();
        }
    }
}
