using PG.Productos.DAL;
using PG.Productos.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Productos.BL
{
    public class ClienteBL
    {
        private readonly ClienteDAL _clienteDAL;

        public ClienteBL(ClienteDAL pClienteDAL)
        {
            _clienteDAL = pClienteDAL;
        }
        public async Task<int> CrearAsync(Cliente pCliente)
        {
            return await _clienteDAL.CrearAsync(pCliente);
        }
        public async Task<int> ModificarAsync(Cliente pCliente)
        {
            return await _clienteDAL.ModificarAsync(pCliente);
        }
        public async Task<int> EliminarAsync(Cliente pCliente)
        {
            return await _clienteDAL.EliminarAsync(pCliente);
        }
        public async Task<Cliente> ObtenerPorIdAsync(Cliente pCliente)
        {
            return await _clienteDAL.ObtenerPorIdAsync(pCliente);
        }
        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            return await _clienteDAL.ObtenerTodosAsync();
        }
        public Task AgregarTodosAsync(List<Cliente> pCliente)
        {
            return _clienteDAL.AgregarTodosAsync(pCliente);
        }
    }
}
