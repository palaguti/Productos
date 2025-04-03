using PG.Productos.DAL;
using PG.Productos.EN.Filtros;
using PG.Productos.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Productos.BL
{
    public class VentaBL
    {
        readonly VentaDAL ventaDAL;

        public VentaBL(VentaDAL pVentaDAL)
        {
            ventaDAL = pVentaDAL;
        }
        public async Task<int> CrearAsync(Venta pVenta)
        {
            return await ventaDAL.CrearAsync(pVenta);
        }
        public async Task<int> AnularAsync(int idVenta)
        {
            return await ventaDAL.AnularAsync(idVenta);
        }
        public async Task<Venta> ObtenerPorIdAsync(int idVenta)
        {
            return await ventaDAL.ObtenerPorIdAsync(idVenta);
        }
        public async Task<List<Venta>> ObtenerTodosAsync()
        {
            return await ventaDAL.ObtenerTodosAsync();
        }
        public async Task<List<Venta>> ObtenerPorEstadosAsync(byte estado)
        {
            return await ventaDAL.ObtenerPorEstadoAsync(estado);
        }
        public async Task<List<Venta>> ObtenerReporteComprasAsync(CompraFiltros filtro)
        {
            return await ventaDAL.ObtenerReporteComprasAsync(filtro);
        }

    }
}
