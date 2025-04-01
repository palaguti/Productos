using PG.Productos.DAL;
using PG.Productos.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Productos.BL
{
    public class ProveedorBL
    {
        private readonly ProveedorDAL _proveedorDAL;

        public ProveedorBL(ProveedorDAL pProveedorDAL)
        {
            _proveedorDAL = pProveedorDAL;
        }
        public async Task<int> CrearAsync(Proveedor pProveedor)
        {
            return await _proveedorDAL.CrearAsync(pProveedor);
        }
        public async Task<int> ModificarAsync(Proveedor pProveedor)
        {
            return await _proveedorDAL.ModificarAsync(pProveedor);
        }
        public async Task<int> EliminarAsync(Proveedor pProveedor)
        {
            return await _proveedorDAL.EliminarAsync(pProveedor);
        }
        public async Task<Proveedor> ObtenerPorIdAsync(Proveedor pProveedor)
        {
            return await _proveedorDAL.ObtenerPorIdAsync(pProveedor);
        }
        public async Task<List<Proveedor>> ObtenerTodosAsync()
        {
            return await _proveedorDAL.ObtenerTodosAsync();
        }
        public  Task AgregarTodosAsync(List<Proveedor> pProveedor)
        {
            return  _proveedorDAL.AgregarTodosAsync(pProveedor);
        }
    }
}
