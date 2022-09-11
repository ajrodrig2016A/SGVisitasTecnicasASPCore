using FastReport;
using FastReport.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SGVisitasTecnicasASPCore.Data;
using SGVisitasTecnicasASPCore.Interfaces;
using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Controllers
{
    [Authorize]
    public class VentasReporteController : Controller
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly SgvtDB _context; // for connecting to efcore.
        private readonly IClientes _clientesRepo;
        private readonly IConfiguration _configuration;
        private List<ventas> lstVentas = null;
        string path = "";
        string errMessage = "";

        public VentasReporteController(IWebHostEnvironment webHost, SgvtDB context, IClientes clientesRepo, IConfiguration configuration)
        {
            _webHost = webHost;
            _context = context;
            _clientesRepo = clientesRepo;
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult VentasReporte(ventas model)
        {
            PopulateViewbags();
            return View(model);
        }

        [HttpPost]
        public IActionResult GenerateVentasReporte(ventas model)
        {
            MemoryStream ms = new MemoryStream();
            StringBuilder nameReport = new StringBuilder();
            FastReport.Utils.Config.WebMode = true;
            Report repGen = new Report();
            Report repVtasPorCliente = new Report();
            string webRootPath = _webHost.WebRootPath;

            try
            {
                if (model.id_cliente == 0)
                {
                    try
                    {
                        path = Path.Combine(webRootPath, "ReporteVentasConcretadas.frx");
                        repGen.Load(path);
                        repGen.SetParameterValue("prmUsuario", @User.Identity.Name);
                        nameReport.Append("Reporte General de Ventas Concretadas" + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"));
                        nameReport.Append(".pdf");

                        if (repGen.Report.Prepare())
                        {
                            FastReport.Export.PdfSimple.PDFSimpleExport pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                            pdfExport.ShowProgress = false;
                            pdfExport.Subject = "Reporte General de Ventas";
                            pdfExport.Title = "Reporte General de Ventas";

                            repGen.Report.Export(pdfExport, ms);
                            repGen.Dispose();
                            pdfExport.Dispose();
                            ms.Position = 0;
                        }
                        else
                        {
                            errMessage = "No se pudo generar el reporte.";
                            TempData["ErrorMessage"] = errMessage;
                        }
                    }
                    catch (Exception ex)
                    {
                        errMessage = errMessage + " " + ex.Message;
                        TempData["ErrorMessage"] = errMessage;
                    }

                }
                else
                {
                    lstVentas = _context.ventas.Where(ct => ct.id_cliente == model.id_cliente).OrderBy(v => v.id_venta).ToList();

                    if (lstVentas.Count == 0)
                    {
                        model = LimpiarCampos();
                        return EmptyGetDataFromDB(model);
                    }

                    Report rep = new Report();
                    MySqlDataConnection conn = new MySqlDataConnection();
                    conn.ConnectionString = _configuration.GetConnectionString("SgvtDB");
                    TableDataSource table = new TableDataSource();
                    table.Alias = "VentasTable";
                    table.Name = "Table";
                    conn.Tables.Add(table);
                    TableDataSource table1 = new TableDataSource();
                    table1.Alias = "DetallesVentaTable";
                    table1.Name = "Table1";
                    conn.Tables.Add(table1);
                    path = Path.Combine(webRootPath, "ReporteVentasPorCliente.frx");
                    rep.Load(path);
                    rep.SetParameterValue("prmUsuario", @User.Identity.Name);

                    StringBuilder queryCtz = new StringBuilder();
                    queryCtz.Append("SELECT C.id_cliente AS 'ID CLIENTE', V.id_venta AS 'ID VTA', V.codigo_venta AS 'NRO VENTA', V.numero_factura AS 'NRO FACTURA', C.nombres AS 'CLI NOMBRES', C.apellidos AS 'CLI APELLIDOS', V.fecha_creacion AS 'FECHA DE CREACION', V.fecha_cierre AS 'FECHA DE CIERRE', V.estado AS 'ESTADO VTA', V.subtotal AS 'SUBTOTAL', V.Iva AS 'IVA', V.total AS 'TOTAL', V.observaciones AS 'OBSERVACIONES'");
                    queryCtz.Append("FROM `ventas` V INNER JOIN `clientes` C ON V.id_cliente = C.id_cliente WHERE V.id_cliente =");
                    queryCtz.Append(model.id_cliente.ToString());
                    TableDataSource dsCtz = rep.GetDataSource("VentasTable") as TableDataSource;
                    dsCtz.SelectCommand = queryCtz.ToString();

                    StringBuilder queryDetCtz = new StringBuilder();
                    queryDetCtz.Append("SELECT V.codigo_venta AS 'NRO VENTA', V.id_venta AS 'ID VENTA', D.id_detalle_venta AS 'ID DT VTA', P.nombre AS 'ITEM', P.descripcion AS 'DESCRIPCION', M.nombre AS 'MARCA', D.cantidad AS 'CANTIDAD', D.valorUnitario AS 'VALOR UNITARIO', D.descuento AS 'DESCUENTO', D.valorTotal AS 'VALOR TOTAL'");
                    queryDetCtz.Append("FROM ((`detallesventa` D INNER JOIN `ventas` V ON D.id_venta = V.id_venta ");
                    queryDetCtz.Append(") INNER JOIN `productos` P ON D.id_producto = P.id_producto ");
                    queryDetCtz.Append(") INNER JOIN `marcas` M ON P.id_marca = M.id_marca WHERE V.id_cliente = ");
                    queryDetCtz.Append(model.id_cliente.ToString());
                    TableDataSource dsDetCtz = rep.GetDataSource("DetallesVentaTable") as TableDataSource;
                    dsDetCtz.SelectCommand = queryDetCtz.ToString();

                    string clienteVta = _context.clientes.Where(ctz => ctz.id_cliente == model.id_cliente).Select(c => c.numero_documento).FirstOrDefault();
                    nameReport.Append("Reporte de Ventas por Cliente_");
                    nameReport.Append(clienteVta + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH:mm:ss"));
                    nameReport.Append(".pdf");


                    if (rep.Report.Prepare())
                    {
                        FastReport.Export.PdfSimple.PDFSimpleExport pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                        pdfExport.ShowProgress = false;
                        pdfExport.Subject = "Reporte de Ventas por cliente";
                        pdfExport.Title = "Reporte de Ventas por cliente";

                        rep.Report.Export(pdfExport, ms);
                        rep.Dispose();
                        pdfExport.Dispose();
                        ms.Position = 0;
                    }
                    else
                    {
                        errMessage = "No se pudo generar el reporte.";
                        TempData["ErrorMessage"] = errMessage;
                    }
                }

            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
                TempData["ErrorMessage"] = errMessage;
            }

            return File(ms, "application/pdf", nameReport.ToString());
        }



        private ViewResult EmptyGetDataFromDB(ventas model)
        {
            errMessage = "No hay datos para generar el reporte.";
            TempData["ErrorMessage"] = errMessage;
            PopulateViewbags();
            ModelState.Clear();
            return View("VentasReporte", model);
        }

        public IActionResult RedirectToIndex()
        {
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        private ventas LimpiarCampos()
        {
            PopulateViewbags();
            ModelState.Clear();
            ventas objVentasesReporte = new ventas() { id_cliente = 0, id_venta = 0 };
            return objVentasesReporte;
        }

        private void PopulateViewbags()
        {
            ViewBag.Clientes = GetClientes();
        }

        private List<SelectListItem> GetClientes()
        {
            var lstItems = new List<SelectListItem>();

            PaginatedList<clientes> items = _clientesRepo.GetItems("nombres", Models.SortOrder.Ascending, "", 1, 1000);
            lstItems = items.Select(ut => new SelectListItem()
            {
                Value = ut.id_cliente.ToString(),
                Text = ut.nombres
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "0",
                Text = "----Todos los clientes----"
            };

            lstItems.Insert(0, defItem);

            return lstItems;
        }

        private List<SelectListItem> GetVentas()
        {
            var lstQuotes = new List<SelectListItem>();
            List<ventas> items = _context.ventas.Include(cl => cl.Cliente).ToList();

            lstQuotes = items.Select(vta => new SelectListItem()
            {
                Value = vta.id_venta.ToString(),
                Text = vta.codigo_venta
            }).ToList();

            var defQuote = new SelectListItem()
            {
                Value = null,
                Text = "----Seleccione una venta----"
            };

            lstQuotes.Insert(0, defQuote);

            return lstQuotes;
        }

        //Otener las ventas asociadas a un cliente
        public IActionResult GetVta(int cid)
        {
            var lstVentas = _context.ventas.Include(cl => cl.Cliente).Where(s => s.id_cliente == cid).Select(c => new { Id = c.id_venta, Name = c.codigo_venta }).ToList();

            return Json(lstVentas);
        }
    }
}
