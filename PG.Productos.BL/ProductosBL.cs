using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PG.Productos.DAL;
using PG.Productos.EN;

namespace PG.Productos.BL
{
    public class ProductosBL
    {
        private readonly ProductosDAL _productosDAL;

        public ProductosBL(ProductosDAL productosDAL)
        {
            _productosDAL = productosDAL;
        }
        public async Task<int> CrearAsync(Producto pProducto)
        {
            return await _productosDAL.CrearAsync(pProducto);
        }
        public async Task<int> ModificarAsync(Producto pProducto)
        {
            return await _productosDAL.ModificarAsync(pProducto);
        }
        public async Task<int> EliminarAsync(Producto pProducto)
        {
            return await _productosDAL.EliminarAsync(pProducto);
        }
        public async Task<Producto> ObtenerPorIdAsync(Producto pProducto)
        {
            return await _productosDAL.ObtenerPorIdAsync(pProducto);
        }
        public async Task<List<Producto>> ObtenerTodosAsync()
        {
            return await _productosDAL.ObtenerTodosAsync();
        }
        public Task AgregarTodosAsync(List<Producto> pProducto)
        {
            return _productosDAL.AgregarTodosAsync(pProducto);
        }
    }
}
