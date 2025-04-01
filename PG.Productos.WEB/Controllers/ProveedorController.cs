using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PG.Productos.BL;
using PG.Productos.EN;
using Rotativa.AspNetCore;

namespace PG.Productos.WEB.Controllers
{
    public class ProveedorController : Controller
    {
        readonly ProveedorBL _proveedorBL;

        public ProveedorController(ProveedorBL pProveedorBL)
        {
            _proveedorBL = pProveedorBL;
        }
        public async Task<ActionResult> Index()
        {
            var proveedores = await _proveedorBL.ObtenerTodosAsync();
            return View(proveedores);
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
        public async Task<ActionResult> Create(Proveedor pProveedor)
        {
            try
            {
                await _proveedorBL.CrearAsync(pProveedor);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductosController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            var proveedor = await _proveedorBL.ObtenerPorIdAsync(new Proveedor { Id = id });
            return View(proveedor);
        }

        // POST: ProductosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Proveedor pProveedor)
        {
            try
            {
                var result = await _proveedorBL.ModificarAsync(pProveedor);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductosController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var proveedor = await _proveedorBL.ObtenerPorIdAsync(new Proveedor { Id = id });
            return View(proveedor);
        }

        // POST: ProductosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteProveedor(int id)
        {
            try
            {
                var result = await _proveedorBL.EliminarAsync(new Proveedor { Id = id });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<ActionResult> ReporteProveedores()
        {
            var proveedores = await _proveedorBL.ObtenerTodosAsync();
            return new ViewAsPdf("rpProveedor", proveedores);
        }

        public async Task<JsonResult> ProveedoresJson()
        {
            var proveedores = await _proveedorBL.ObtenerTodosAsync();
            var proveedoresData = proveedores
                .Select(p => new
                {
                    id = p.Id,
                    nombre = p.Nombre,
                    nrc = p.NRC,
                    direccion = p.Direccion,
                    telefono = p.Telefono,
                    email = p.Email
                })
                .ToList();
            return Json(proveedoresData);
        }
        public async Task<IActionResult> ReporteProductosExcel()
        {
            var productos = await _proveedorBL.ObtenerTodosAsync();
            using (var package = new ExcelPackage())
            {
                var hojaExcel = package.Workbook.Worksheets.Add("Proveedor");

                hojaExcel.Cells["A1"].Value = "Nombre";
                hojaExcel.Cells["B1"].Value = "NRC";
                hojaExcel.Cells["C1"].Value = "Direccion";
                hojaExcel.Cells["D1"].Value = "Telefono";
                hojaExcel.Cells["E1"].Value = "Email";

                int row = 2;

                foreach (var producto in productos)
                {
                    hojaExcel.Cells[row, 1].Value = producto.Nombre;
                    hojaExcel.Cells[row, 2].Value = producto.NRC;
                    hojaExcel.Cells[row, 3].Value = producto.Direccion;
                    hojaExcel.Cells[row, 4].Value = producto.Telefono;
                    hojaExcel.Cells[row, 5].Value = producto.Email;
                    row++;
                }
                hojaExcel.Cells["A:E"].AutoFitColumns();
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheets.sheets", "ReporteProveedorExcel.xlsx");
            }
        }
        public async Task<IActionResult> SubirExcelProveedor(IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
            {
                return RedirectToAction("Index");
            }
            var proveedor = new List<Proveedor>();

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
                        var NRC = hojaExcel.Cells[row, 2].Text;
                        var direccion = hojaExcel.Cells[row, 3].Text;
                        var telefono = hojaExcel.Cells[row, 4].Text;
                        var email = hojaExcel.Cells[row, 1].Text;

                        proveedor.Add(new Proveedor
                        {
                            Nombre = nombre,
                            NRC = NRC,
                            Direccion = direccion,
                            Telefono = telefono,
                            Email = email,
                        });
                    }
                    if (proveedor.Count > 0)
                    {
                        await _proveedorBL.AgregarTodosAsync(proveedor);
                    }
                    return RedirectToAction("Index");
                }
            }
        }

    }
}
