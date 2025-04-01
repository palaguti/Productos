using PG.Productos.EN;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Productos.DAL
{
    public class ProductosDAL
    {
        readonly ProductosDBContext _dbContext;
        private int Id;
        private object Nombre;

        public ProductosDAL(ProductosDBContext context)
        {
            _dbContext = context;

        }
        public async Task<int> CrearAsync(Producto pProducto)
        {
            Producto producto = new Producto()
            {
                Nombre = pProducto.Nombre,
                Precio  = pProducto.Precio,
                CantidadDisponible = pProducto.CantidadDisponible,
                FechaCreacion = pProducto.FechaCreacion,
            };
            _dbContext.Add(producto);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> EliminarAsync(Producto pProducto)
        {
            var producto = await _dbContext.producto.FirstOrDefaultAsync(s => s.Id == pProducto.Id);
            
            if (producto != null)
            {
                _dbContext.producto.Remove(producto);
                return await _dbContext.SaveChangesAsync();
            }
            else
                return 0;
        }
        public async Task<int> ModificarAsync(Producto pProducto)
        {
            var producto = await _dbContext.producto.FirstOrDefaultAsync(s => s.Id == pProducto.Id);
            if (producto != null && producto.Id > 0)
            {
                producto.Nombre = pProducto.Nombre;
                producto.Precio = pProducto.Precio;
                producto.CantidadDisponible = pProducto.CantidadDisponible;
                return await _dbContext.SaveChangesAsync();
            }
            else
                { return 0; }
        }
        public async Task<Producto> ObtenerPorIdAsync(Producto pProducto)
        {
            var producto = await _dbContext.producto.FirstOrDefaultAsync(s => s.Id == pProducto.Id);
            if (producto != null && producto.Id != 0)
            {
                return new Producto
                {
                    Id = producto.Id,
                    Nombre = producto.Nombre,
                    Precio = producto.Precio,
                    CantidadDisponible = producto.CantidadDisponible,
                    FechaCreacion = producto.FechaCreacion,
                };
            }
            else
                return new Producto();
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            var producto = await _dbContext.producto.ToListAsync();
            if (producto != null && producto.Count > 0)
            {
                var list = new List<Producto>();
                producto.ForEach(s => list.Add(new Producto
                {
                    Id = s.Id,
                    Nombre = s.Nombre,
                    Precio = s.Precio,
                    CantidadDisponible = s.CantidadDisponible,
                    FechaCreacion = s.FechaCreacion,
                }));
                return list;
            }
            else
                return new List<Producto>();
        }
        public async Task AgregarTodosAsync(List<Producto> pProducto)
        {
            await _dbContext.producto.AddRangeAsync(pProducto);
            await _dbContext.SaveChangesAsync();
        }
    }

}
