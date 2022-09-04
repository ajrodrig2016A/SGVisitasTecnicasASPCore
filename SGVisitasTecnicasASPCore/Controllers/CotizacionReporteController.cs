using FastReport;
using FastReport.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using SGVisitasTecnicasASPCore.Data;
using SGVisitasTecnicasASPCore.Interfaces;
using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace SGVisitasTecnicasASPCore.Controllers
{
    [Authorize]
    public class CotizacionReporteController : Controller
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly SgvtDB _context; // for connecting to efcore.
        private readonly IClientes _clientesRepo;
        private readonly IConfiguration _configuration;
        private List<detalles_cotizacion> lstDetCotizacion = null;
        string path = "";
        string errMessage = "";

        public CotizacionReporteController(IWebHostEnvironment webHost, SgvtDB context, IClientes clientesRepo, IConfiguration configuration)
        {
            _webHost = webHost;
            _context = context;
            _clientesRepo = clientesRepo;
            _configuration = configuration;
        }  

        [HttpGet]
        public IActionResult CotizacionReporte(cotizaciones model)
        {
            if(User.IsInRole("CLI"))
            {
                int idCliente = _context.clientes.Where(c => c.nombres == User.Identity.Name).FirstOrDefault().id_cliente;
                ViewBag.Clientes = GetClientes(idCliente);
                ViewBag.Cotizaciones = GetCotizaciones(idCliente);
                return View(model);
            }
            PopulateViewbags();
            return View(model);
        }
        [HttpPost]
        public IActionResult GenerateCotizacionReporte(cotizaciones model)
        {
            MemoryStream ms = new MemoryStream();
            StringBuilder nameReport = new StringBuilder();

            try
            {
                if (model.id_cliente == 0)
                {
                    var cliente = _context.clientes.Where(ctz => ctz.id_cliente == model.id_cliente).Select(c => new { c.nombres, c.apellidos }).FirstOrDefault();

                    errMessage = "El cliente " + cliente.nombres + " " + cliente.apellidos + " no registra cotizaciones.";
                    //ModelState.AddModelError("", errMessage);
                    return EmptyGetDataFromDB(model);
                }

                lstDetCotizacion = _context.detallesCotizacion.Where(ct => ct.id_cotizacion == model.id_cotizacion).OrderBy(v => v.id_producto).ToList();

                if (lstDetCotizacion.Count == 0)
                {
                    model = LimpiarCampos();
                    return EmptyGetDataFromDB(model);
                }

                FastReport.Utils.Config.WebMode = true;
                Report rep = new Report();
                MySqlDataConnection conn = new MySqlDataConnection();
                conn.ConnectionString = _configuration.GetConnectionString("SgvtDB");
                TableDataSource table = new TableDataSource();
                table.Alias = "CotizacionTable";
                table.Name = "Table";
                conn.Tables.Add(table);
                TableDataSource table1 = new TableDataSource();
                table1.Alias = "DetallesCotizacionTable";
                table1.Name = "Table1";
                conn.Tables.Add(table1);

                string webRootPath = _webHost.WebRootPath;
                path = Path.Combine(webRootPath, "ReporteCotizacion.frx");
                rep.Load(path);

                StringBuilder queryCtz = new StringBuilder();
                queryCtz.Append("SELECT C.id_cotizacion AS 'ID COTIZACION', C.codigo AS 'NRO COTIZACION', c1.nombres AS 'CLI NOMBRES', c1.apellidos AS 'CLI APELLIDOS', c1.direccion AS 'DIRECCION DEL CLIENTE', c1.numero_contacto AS 'TELEFONO DEL CLIENTE', replace(C.servicio, '_', ' ')  AS 'NOMBRE DEL SERVICIO', C.subtotal AS 'SUBTOTAL', C.observaciones AS 'OBSERVACIONES', C.sector_inmueble AS 'LUGAR DE ENTREGA', C.tiempo_entrega AS 'TIEMPO DE ENTREGA', C.forma_pago AS 'FORMA DE PAGO', C.garantia_equipos AS 'GARANTIA DE EQUIPOS', C.validez AS 'VALIDEZ DE LA OFERTA', E.nombres AS 'EMP NOMBRES', E.apellidos AS 'EMP APELLIDOS'");
                queryCtz.Append("FROM (`cotizaciones` C INNER JOIN `clientes` c1 ON C.id_cliente = c1.id_cliente");
                queryCtz.Append(") INNER JOIN `empleados` E ON C.id_empleado = E.id_empleado WHERE C.id_cotizacion = ");
                queryCtz.Append(model.id_cotizacion.ToString());               
                TableDataSource dsCtz = rep.GetDataSource("CotizacionTable") as TableDataSource;
                dsCtz.SelectCommand = queryCtz.ToString();

                StringBuilder queryDetCtz = new StringBuilder();
                queryDetCtz.Append("SELECT C.codigo AS 'NRO COTIZACION', C.id_cotizacion AS 'ID COTIZACION', D.id_detalle_cotización AS 'ID DT CTZ', P.nombre AS 'ITEM', P.descripcion AS 'DESCRIPCION', M.nombre AS 'MARCA', D.cantidad AS 'CANTIDAD', D.valorUnitario AS 'VALOR UNITARIO', D.descuento AS 'DESCUENTO', D.valorTotal AS 'VALOR TOTAL'");
                queryDetCtz.Append("FROM((`detallescotizacion` D INNER JOIN `cotizaciones` C ON D.id_cotizacion = C.id_cotizacion");
                queryDetCtz.Append(") INNER JOIN `productos` P ON D.id_producto = P.id_producto");
                queryDetCtz.Append(") INNER JOIN `marcas` M ON P.id_marca = M.id_marca WHERE C.id_cotizacion = ");
                queryDetCtz.Append(model.id_cotizacion.ToString());
                TableDataSource dsDetCtz = rep.GetDataSource("DetallesCotizacionTable") as TableDataSource;
                dsDetCtz.SelectCommand = queryDetCtz.ToString();

                string codigoCtz = _context.cotizaciones.Where(ctz => ctz.id_cotizacion == model.id_cotizacion).Select(c => c.codigo).FirstOrDefault();
                nameReport.Append("CotizacionFinal_");
                nameReport.Append(codigoCtz);
                nameReport.Append(".pdf");


                if (rep.Report.Prepare())
                {
                    FastReport.Export.PdfSimple.PDFSimpleExport pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                    pdfExport.ShowProgress = false;
                    pdfExport.Subject = "Cotizacion por Servicios";
                    pdfExport.Title = "Cotizacion por Servicios";

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
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
                TempData["ErrorMessage"] = errMessage;
            }

            return File(ms, "application/pdf", nameReport.ToString());
        }

        private ViewResult EmptyGetDataFromDB(cotizaciones model)
        {
            errMessage = "No hay datos para generar el reporte.";
            TempData["ErrorMessage"] = errMessage;
            PopulateViewbags();
            ModelState.Clear();
            return View("CotizacionesReporte", model);
        }

        public IActionResult RedirectToIndex()
        {
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        private cotizaciones LimpiarCampos()
        {
            PopulateViewbags();
            ModelState.Clear();
            cotizaciones objCotizacionesReporte = new cotizaciones() { id_cliente = 0, id_cotizacion = 0 };
            return objCotizacionesReporte;
        }

        private void PopulateViewbags()
        {
            int idCliente = 0;
            ViewBag.Clientes = GetClientes(idCliente);
        }

        private List<SelectListItem> GetClientes(int idCliente)
        {
            if (idCliente > 0)
            {
                List<SelectListItem> lstItem = new List<SelectListItem>();
                clientes item = _clientesRepo.GetItem(idCliente);
                SelectListItem oneItem = new SelectListItem()
                {
                    Value = item.id_cliente.ToString(),
                    Text = item.nombres
                };

                lstItem.Insert(0, oneItem);
                return lstItem;
            }

            var lstItems = new List<SelectListItem>();

            PaginatedList<clientes> items = _clientesRepo.GetItems("nombres", Models.SortOrder.Ascending, "", 1, 1000);
            lstItems = items.Select(ut => new SelectListItem()
            {
                Value = ut.id_cliente.ToString(),
                Text = ut.nombres
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Seleccione el Cliente----"
            };

            lstItems.Insert(0, defItem);

            return lstItems;
        }

        private List<SelectListItem> GetCotizaciones(int idCliente)
        {
            var lstQuotes = new List<SelectListItem>();
            List<cotizaciones> items = _context.cotizaciones.Include(cl => cl.Cliente).Where(ct => ct.id_cliente == idCliente).ToList();

            lstQuotes = items.Select(ctz => new SelectListItem()
            {
                Value = ctz.id_cotizacion.ToString(),
                Text = ctz.codigo
            }).ToList();

            var defQuote = new SelectListItem()
            {
                Value = null,
                Text = "----Seleccione una cotización----"
            };

            lstQuotes.Insert(0, defQuote);

            return lstQuotes;
        }

        //Otener las cotizaciones asociadas a un cliente
        public IActionResult GetCtz(int cid)
        {
            var lstCotizaciones = _context.cotizaciones.Include(cl => cl.Cliente).Where(s => s.id_cliente == cid).Select(c => new { Id = c.id_cotizacion, Name = c.codigo }).ToList();

            return Json(lstCotizaciones);
        }

    }
}
