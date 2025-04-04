using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using PG.Productos.BL;
using PG.Productos.EN;
using Rotativa.AspNetCore;
namespace PG.Productos.WEB.Controllers
{
    public class ClienteController : Controller
    {
        readonly ClienteBL _clienteBL;

        public ClienteController(ClienteBL pClienteBL)
        {
            _clienteBL = pClienteBL;
        }
        public async Task<ActionResult> Index()
        {
            var clientes = await _clienteBL.ObtenerTodosAsync();
            return View(clientes);
        }

        // GET: ClienteController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ClienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Cliente pCliente)
        {
            try
            {
                await _clienteBL.CrearAsync(pCliente);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController/Edit/5
        public async Task <ActionResult> Edit(int id)
        {
            var cliente = await _clienteBL.ObtenerPorIdAsync(new Cliente { Id = id });
            return View(cliente);
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Cliente pCliente)
        {
            try
            {
                var result = await _clienteBL.ModificarAsync(pCliente);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ClienteController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var cliente = await _clienteBL.ObtenerPorIdAsync(new Cliente { Id = id });
            return View(cliente);
        }

        // POST: ClienteController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteCliente(int id)
        {
            try
            {
                var result = await _clienteBL.EliminarAsync(new Cliente { Id = id });
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
        public async Task<ActionResult> ReporteCliente()
        {
            var clientes = await _clienteBL.ObtenerTodosAsync();
            return new ViewAsPdf("rpCliente", clientes);
        }

        public async Task<JsonResult> ClientesJson()
        {
            var clientes = await _clienteBL.ObtenerTodosAsync();
            var clientesData = clientes
                .Select(p => new
                {
                    id = p.Id,
                    nombre = p.Nombre,
                    telefono = p.Telefono,
                    correo = p.Correo,
                })
                .ToList();
            return Json(clientesData);
        }
        public async Task<IActionResult> ReporteClientesExcel()
        {
            var clientes = await _clienteBL.ObtenerTodosAsync();
            using (var package = new ExcelPackage())
            {
                var hojaExcel = package.Workbook.Worksheets.Add("Cliente");

                hojaExcel.Cells["A1"].Value = "Nombre";
                hojaExcel.Cells["B1"].Value = "Telefono";
                hojaExcel.Cells["C1"].Value = "Correo";

                int row = 2;

                foreach (var producto in clientes)
                {
                    hojaExcel.Cells[row, 1].Value = producto.Nombre;
                    hojaExcel.Cells[row, 2].Value = producto.Telefono;
                    hojaExcel.Cells[row, 3].Value = producto.Correo;
                    row++;
                }
                hojaExcel.Cells["A:C"].AutoFitColumns();
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheets.sheets", "ReporteClientesExcel.xlsx");
            }
        }
        public async Task<IActionResult> SubirExcelCliente(IFormFile archivoExcel)
        {
            if (archivoExcel == null || archivoExcel.Length == 0)
            {
                return RedirectToAction("Index");
            }
            var cliente = new List<Cliente>();

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
                        var telefono = hojaExcel.Cells[row, 2].Text;
                        var correo = hojaExcel.Cells[row, 3].Text;

                        cliente.Add(new Cliente
                        {
                            Nombre = nombre,
                            Telefono = telefono,
                            Correo = correo,
                        });
                    }
                    if (cliente.Count > 0)
                    {
                        await _clienteBL.AgregarTodosAsync(cliente);
                    }
                    return RedirectToAction("Index");
                }
            }
        }
    }
}
