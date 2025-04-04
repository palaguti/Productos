using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;
using PG.Productos.BL;
using PG.Productos.EN.Filtros;
using PG.Productos.EN;
using Rotativa.AspNetCore;
using static PG.Productos.EN.Filtros.CompraFiltros;
using static PG.Productos.EN.Venta;

namespace PG.Productos.WEB.Controllers
{
    public class VentaController : Controller
    {
        readonly ClienteBL clienteBL;
        readonly VentaBL ventaBL;
        readonly ProductosBL productosBL;

        public VentaController(ClienteBL pClienteBL, VentaBL pVentaBL, ProductosBL pProductosBL)
        {
            clienteBL = pClienteBL;
            ventaBL = pVentaBL;
            productosBL = pProductosBL;
        }
        // GET: CompraController
        public async Task<IActionResult> Index(byte? estado)
        {
            var ventas = await ventaBL.ObtenerPorEstadosAsync(estado ?? 0);

            var estados = new List<SelectListItem>
            {
                new SelectListItem {Value="", Text="Todos"},
                new SelectListItem {Value="1", Text="Activa"},
                new SelectListItem {Value="2", Text="Anulada"}
            };
            ViewBag.estado = new SelectList(estados, "Value", "Text", estado?.ToString());
            return View(ventas);
        }

        // GET: CompraController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompraController/Create
        public async Task<IActionResult> Create()
        {
            // Obtener todos los clientes
            var clientes = await clienteBL.ObtenerTodosAsync();
            // Asegúrate de que no sea null
            if (clientes == null)
            {
                clientes = new List<Cliente>(); // O cualquier lista vacía adecuada
            }

            // Obtener todos los productos
            var productos = await productosBL.ObtenerTodosAsync();
            // Asegúrate de que no sea null
            if (productos == null)
            {
                productos = new List<Producto>(); // O cualquier lista vacía adecuada
            }

            // Pasar los datos al ViewBag
            ViewBag.cliente = new SelectList(clientes, "Id", "Nombre");
            ViewBag.Productos = productos;

            return View();
        }


        // POST: CompraController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venta venta)
        {
            try
            {
                venta.Estado = (byte)EnumEstadoVentas.Activa;
                venta.FechaVenta = DateTime.Now;
                await ventaBL.CrearAsync(venta);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompraController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompraController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompraController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompraController/Delete/5
        public async Task<IActionResult> Anular(int id = 1)
        {
            var compra = await ventaBL.ObtenerPorIdAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            await ventaBL.AnularAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> AnularFACT(int id)
        {
            var compra = await ventaBL.ObtenerPorIdAsync(id);
            if (compra == null)
            {
                return NotFound();
            }
            await ventaBL.AnularAsync(id);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> ReporteVentasExcel(List<Venta> ventas)
        {
            using (var package = new ExcelPackage())
            {
                var hojaExcel = package.Workbook.Worksheets.Add("Reporte Ventas");

                hojaExcel.Cells["A1"].Value = "Fecha de Venta";
                hojaExcel.Cells["B1"].Value = "Cliente";
                hojaExcel.Cells["C1"].Value = "Producto";
                hojaExcel.Cells["D1"].Value = "Cantidad";
                hojaExcel.Cells["E1"].Value = "SubTotal";
                hojaExcel.Cells["F1"].Value = "Total de la Venta";

                int row = 2;
                int totalCantidad = 0;
                decimal totalSubTtal = 0;
                decimal totalGeneral = 0;

                foreach (var venta in ventas)
                {
                    foreach (var detalle in venta.DetalleVentas)
                    {
                        hojaExcel.Cells[row, 1].Value = venta.FechaVenta.ToString("yyyy-MM-dd");
                        hojaExcel.Cells[row, 2].Value = venta.Cliente?.Nombre ?? "N/A";
                        hojaExcel.Cells[row, 3].Value = detalle.Producto?.Nombre ?? "N/A";
                        hojaExcel.Cells[row, 4].Value = detalle.Cantidad;
                        hojaExcel.Cells[row, 5].Value = detalle.SubTtal;
                        hojaExcel.Cells[row, 6].Value = venta.Total;

                        totalCantidad += detalle.Cantidad;
                        totalSubTtal += detalle.SubTtal;
                        totalGeneral += venta.Total;

                        row++;
                    }

                }
                hojaExcel.Cells[row, 3].Value = "Totales";
                hojaExcel.Cells[row, 4].Value = totalCantidad;
                hojaExcel.Cells[row, 5].Value = totalSubTtal;
                hojaExcel.Cells[row, 6].Value = totalGeneral;

                hojaExcel.Cells[row, 3, row, 6].Style.Font.Bold = true;

                hojaExcel.Cells["A:F"].AutoFitColumns();

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ReporteVentasExcel.xlsx");
            }
        }
        [HttpGet]

        public async Task<IActionResult> DescargarReporte(CompraFiltros filtro)
        {
            var ventas = await ventaBL.ObtenerReporteComprasAsync(filtro);

            if (filtro.TipoReporte == (byte)EnumTipoReporte.PDF)
            {
                return new ViewAsPdf("rpVentas", ventas);
            }
            else if (filtro.TipoReporte == (byte)EnumTipoReporte.Excel)
            {
                return await ReporteVentasExcel(ventas);
            }
            return BadRequest("Formato no valido");
        }

        [HttpGet]
        public IActionResult ReporteVentas()
        {
            return View();
        }
    }
}
