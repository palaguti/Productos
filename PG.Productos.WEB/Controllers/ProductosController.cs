using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PG.Productos.BL;
using PG.Productos.EN;
using Rotativa.AspNetCore;

namespace PG.Productos.WEB.Controllers
{
    public class ProductosController : Controller
    {
        private readonly ProductosBL _productosBL;
        public ProductosController(ProductosBL productosBL)
        {
            _productosBL = productosBL;
        }
        // GET: ProductosController
        public async Task<ActionResult> Index()
        {
            var productos = await _productosBL.ObtenerTodosAsync();
            return View(productos);
        }

        // GET: ProductosController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Create(Producto pProducto)
        {
            try
            {
                await _productosBL.CrearAsync(pProducto);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductosController/Edit/5
        public async Task <ActionResult> Edit(int id)
        {
            var producto = await _productosBL.ObtenerPorIdAsync(new Producto { Id = id });
            return View(producto);
        }

        // POST: ProductosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Edit(Producto pProducto)
        {
            try
            {
                await _productosBL.ModificarAsync(pProducto);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductosController/Delete/5
        public async Task <ActionResult> Delete(int id)
        {
            var producto = await _productosBL.ObtenerPorIdAsync(new Producto { Id=id });
            return View(producto);
        }

        // POST: ProductosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <ActionResult> Delete(Producto pProducto)
        {
            try
            {
                await _productosBL.EliminarAsync(new Producto { Id = pProducto.Id });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<ActionResult> ReporteProductos()
        {
            var productos = await _productosBL.ObtenerTodosAsync();
            return new ViewAsPdf("rpProductos",productos);
        }
        public async Task<JsonResult> ProductosJson()
        {
            var productos = await _productosBL.ObtenerTodosAsync();
            var productosData = productos
                .Select(p => new
                {
                    nombre = p.Nombre,
                    stok = p.CantidadDisponible,
                })
                .ToList();
            return Json(productosData);
        }
        public async Task<JsonResult> ProductosJsonPrecio()
        {
            var productos = await _productosBL.ObtenerTodosAsync();

            var productosData = productos
                .Select(p => new
                {
                    fechacreacion = p.FechaCreacion.ToString("yyyy-MM-dd"),
                    precio = p.Precio
                })
                .ToList();

            var groupedData = productosData
                .GroupBy(p => p.fechacreacion)
                .Select(g => new
                {
                    fecha = g.Key,
                    precioPromedio = g.Average(p => p.precio)
                })
                .OrderBy(g => g.fecha)
                .ToList();
            return Json(groupedData);
        }
        public async Task<IActionResult> ReporteProductosExcel() 
        {
            var productos = await _productosBL.ObtenerTodosAsync();
            using (var package = new ExcelPackage())
            {
                var hojaExcel = package.Workbook.Worksheets.Add("Productos");

                hojaExcel.Cells["A1"].Value = "Nombre";
                hojaExcel.Cells["B1"].Value = "Precio";
                hojaExcel.Cells["C1"].Value = "Cantidad";
                hojaExcel.Cells["D1"].Value = "Fecha";

                int row = 2;

                foreach (var producto in productos) 
                {
                    hojaExcel.Cells[row, 1].Value = producto.Nombre;
                    hojaExcel.Cells[row, 2].Value = producto.Precio;
                    hojaExcel.Cells[row, 3].Value = producto.CantidadDisponible;
                    hojaExcel.Cells[row, 4].Value = producto.FechaCreacion.ToString("yyyy-MM-dd");
                    row++;
                }
                hojaExcel.Cells["A:D"].AutoFitColumns();
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheets.sheets", "ReporteProductosExcel.xlsx");
            }
        }
        public async Task<IActionResult> SubirExcelProductos(IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
            {
                return RedirectToAction("Index");
            }
            var productos = new List<Producto>();

            using (var stream = new MemoryStream())
            {
                await archivoExcel.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var hojaExcel = package.Workbook.Worksheets[0];

                    int rowCount = hojaExcel.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var nombre = hojaExcel.Cells[row, 1].Text;
                        var precio = hojaExcel.Cells[row, 2].GetValue<decimal>();
                        var cantidad = hojaExcel.Cells[row, 3].GetValue<int>();
                        var fecha = hojaExcel.Cells[row, 4].GetValue<DateTime>();

                        if (string.IsNullOrEmpty(nombre) || precio <= 0 || cantidad < 0)
                            continue;
                        productos.Add(new Producto
                        {
                            Nombre = nombre,
                            Precio = precio,
                            CantidadDisponible = cantidad,
                            FechaCreacion = fecha,
                        });
                    }
                    if (productos.Count > 0)
                    {
                        await _productosBL.AgregarTodosAsync(productos);
                    }
                    return RedirectToAction("Index");
                }
            }
        }
    }
}
