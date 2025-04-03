using Microsoft.EntityFrameworkCore;
using PG.Productos.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Productos.DAL
{
    public class ClienteDAL
    {
        readonly ProductosDBContext _dbContext;

        public ClienteDAL(ProductosDBContext sysProductosDB)
        {
            _dbContext = sysProductosDB;
        }

        public async Task<int> CrearAsync(Cliente pCliente)
        {
            Cliente cliente = new Cliente()
            {
                Nombre = pCliente.Nombre,
                Telefono = pCliente.Telefono,
                Correo = pCliente.Correo,
            };
            _dbContext.Clientes.Add(cliente);
            return await _dbContext.SaveChangesAsync();
        }
        public async Task<int> EliminarAsync(Cliente pCliente)
        {
            var cliente = await _dbContext.proveedores.FirstOrDefaultAsync(s => s.Id == pCliente.Id);
            if (cliente != null && cliente.Id != 0)
            {
                _dbContext.proveedores.Remove(cliente);
                return await _dbContext.SaveChangesAsync();
            }
            else
                return 0;
        }
        public async Task<int> ModificarAsync(Cliente pCliente)
        {
            var cliente = await _dbContext.Clientes.FirstOrDefaultAsync(s => s.Id == pCliente.Id);
            if (cliente != null && cliente.Id != 0)
            {
                cliente.Nombre = pCliente.Nombre;
                cliente.Telefono = pCliente.Telefono;
                cliente.Correo = pCliente.Correo;

                _dbContext.Update(cliente);
                return await _dbContext.SaveChangesAsync();
            }
            else
                return 0;
        }
        public async Task<Cliente> ObtenerPorIdAsync(Cliente pCliente)
        {
            var cliente = await _dbContext.Clientes.FirstOrDefaultAsync(s => s.Id == pCliente.Id);
            if (cliente != null && cliente.Id != 0)
            {
                return new Cliente
                {
                    Id = cliente.Id,
                    Nombre = cliente.Nombre,
                    Telefono = cliente.Telefono,
                    Correo = cliente.Correo,

                };

            }
            else
                return new Cliente();
        }
        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            var clientes = await _dbContext.Clientes.ToListAsync();
            if (clientes != null && clientes.Count > 0)
            {
                var list = new List<Cliente>();
                clientes.ForEach(p => list.Add(new Cliente
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Telefono = p.Telefono,
                    Correo = p.Correo,

                }));
                return list;

            }
            else
                return new List<Cliente>();
        }
        public async Task AgregarTodosAsync(List<Cliente> pCliente)
        {
            await _dbContext.Clientes.AddRangeAsync(pCliente);
            await _dbContext.SaveChangesAsync();
        }

    }
}
