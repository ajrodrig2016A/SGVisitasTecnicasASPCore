using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGVisitasTecnicasASPCore.Models;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Mvc.Rendering;
using SGVisitasTecnicasASPCore.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace SGVisitasTecnicasASPCore.Controllers
{
    [Authorize]
    public class CotizacionesController : Controller
    {
        private readonly IEmpleados _empleadosRepo;
        private readonly ICotizaciones _cotizacionesRepo;
        private readonly SgvtDB _context;
        public CotizacionesController(ICotizaciones cotizacionesRepo, IEmpleados empleadosRepo, SgvtDB context) // here the repository will be passed by the dependency injection.
        {
            _cotizacionesRepo = cotizacionesRepo;
            _empleadosRepo = empleadosRepo;
            _context = context;
        }

        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Nombre Cliente");
            sortModel.AddColumn("Sector Inmueble");
            sortModel.AddColumn("Dirección Inmueble");
            //sortModel.AddColumn("Nombre Imagen");
            //sortModel.AddColumn("Unidad");
            //sortModel.AddColumn("Cantidad");
            //sortModel.AddColumn("Precio Unitario");
            //sortModel.AddColumn("Stock");
            //sortModel.AddColumn("Descuento");
            //sortModel.AddColumn("Porcentaje"); 
            //sortModel.AddColumn("Categoría");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<cotizaciones> items = _cotizacionesRepo.GetQuotes(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);


            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(items);
        }

        private void PopulateViewbags()
        {

            //ViewBag.Units = GetUnits();

            //ViewBag.Brands = GetBrands();

            ViewBag.empleados = GetEmpleados();

        }

        // GET: CotizacionesController/Details/5
        public IActionResult Details(int id)
        {

            ViewBag.empleados = GetEmpleados();

            cotizaciones cotizacion = _context.cotizaciones.Include(d => d.DetallesCotizacion).Where(c => c.id_cotizacion == id).FirstOrDefault();
            return View(cotizacion);
        }

        public IActionResult Create()
        {
            PopulateViewbags();
            cotizaciones cotizacion = new cotizaciones();
            cotizacion.DetallesCotizacion.Add(new detalles_cotizacion() { id_detalle_cotización = 1 });            
            return View(cotizacion);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 4194304)]
        public IActionResult Create(cotizaciones cotizacion)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                cotizacion.DetallesCotizacion.RemoveAll(n => n.cantidad == (decimal)0.00 || n.valorUnitario == (decimal)0.00);
                cotizacion.DetallesCotizacion.RemoveAll(t => t.valorTotal == (decimal)0.00);
                //cotizacion.DetallesCotizacion.RemoveAll(n => n.IsDeleted == true);
                if (cotizacion.nombre_cliente.Length < 4 || cotizacion.nombre_cliente == null)
                    errMessage = "La longitud del nombre debe ser al menos de 4 caracteres";



                if (_cotizacionesRepo.IsQuoteCodeExists(cotizacion.id_cotizacion) == true)
                    errMessage = errMessage + " " + " El código de la cotización " + cotizacion.id_cotizacion + " ya existe";



                //if (_cotizacionesRepo.IsQuoteExists(cotizacion.nombre_cliente) == true)
                //    errMessage = errMessage + " " + " El nombre de cliente " + cotizacion.nombre_cliente + " ya existe en la cotización";

                foreach (var item in cotizacion.DetallesCotizacion.ToList())
                {
                    if (String.IsNullOrEmpty(item.descripcion) || String.IsNullOrEmpty(item.ubicación) || String.IsNullOrEmpty(item.marca) || String.IsNullOrEmpty(item.unidad) /*||
                    cotizacion.DetallesCotizacion.Where(d => d.id_cotizacion == cotizacion.id_cotizacion).Select(x => x.cantidad).FirstOrDefault() > (decimal)0.00 ||
                    cotizacion.DetallesCotizacion.Where(d => d.id_cotizacion == cotizacion.id_cotizacion).Select(x => x.valorUnitario).FirstOrDefault() > (decimal)0.00 ||
                    cotizacion.DetallesCotizacion.Where(d => d.id_cotizacion == cotizacion.id_cotizacion).Select(x => x.valorTotal).FirstOrDefault() > (decimal)0.00*/)
                        errMessage = "Campos del detalle están incompletos, favor llenarlos.";
                }


                if (cotizacion.instalacion.Length <= 0 || cotizacion.instalacion == null ||
                    cotizacion.garantia_equipos.Length <= 0 || cotizacion.garantia_equipos == null ||
                    cotizacion.forma_pago.Length <= 0 || cotizacion.forma_pago == null ||
                    cotizacion.validez.Length <= 0 || cotizacion.validez == null ||
                    cotizacion.observaciones.Length <= 0 || cotizacion.observaciones == null )
                    errMessage = "Campos de la cotización están incompletos, favor llenarlos.";

                if (errMessage == "")
                {

                    cotizacion = _cotizacionesRepo.Create(cotizacion);
                    bolret = true;
                }
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }
            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                PopulateViewbags();
                return View(cotizacion);

                //return RedirectToAction(nameof(Create));
            }
            else
            {
                TempData["SuccessMessage"] = "Cotización " + cotizacion.id_cotizacion + " creada exitosamente";
                return RedirectToAction(nameof(Index));
            }
        }

        //public IActionResult Details(int id) //Read
        //{
        //    cotizaciones item = _cotizacionesRepo.GetQuote(id);
        //    return View(item);
        //}


        public IActionResult Edit(int id)
        {
            //cotizaciones cotizacion = _cotizacionesRepo.GetQuote(id);
            ViewBag.empleados = GetEmpleados();
            cotizaciones cotizacion = _context.cotizaciones.Include(d => d.DetallesCotizacion).Where(c => c.id_cotizacion == id).FirstOrDefault();
            TempData.Keep();
            return View(cotizacion);
        }

        [HttpPost]
        public IActionResult Edit(cotizaciones cotizacion)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (cotizacion.nombre_cliente.Length < 4 || cotizacion.nombre_cliente == null)
                    errMessage = "La longitud del nombre debe ser al menos de 4 caracteres";


                if (_cotizacionesRepo.IsQuoteCodeExists(cotizacion.id_cotizacion, cotizacion.nombre_cliente) == true)
                    errMessage = errMessage + " " + " El Código de la cotización " + cotizacion.id_cotizacion.ToString() + " ya existe";

                //if (_cotizacionesRepo.IsQuoteExists(cotizacion.nombre_cliente, cotizacion.id_cotizacion) == true)
                //    errMessage = errMessage + " El nombre de cliente " + cotizacion.nombre_cliente + " ya existe en la cotización";

                foreach (var item in cotizacion.DetallesCotizacion.ToList())
                {
                    if (String.IsNullOrEmpty(item.descripcion) || String.IsNullOrEmpty(item.ubicación) || String.IsNullOrEmpty(item.marca) || String.IsNullOrEmpty(item.unidad) /*||
                    cotizacion.DetallesCotizacion.Where(d => d.id_cotizacion == cotizacion.id_cotizacion).Select(x => x.cantidad).FirstOrDefault() > (decimal)0.00 ||
                    cotizacion.DetallesCotizacion.Where(d => d.id_cotizacion == cotizacion.id_cotizacion).Select(x => x.valorUnitario).FirstOrDefault() > (decimal)0.00 ||
                    cotizacion.DetallesCotizacion.Where(d => d.id_cotizacion == cotizacion.id_cotizacion).Select(x => x.valorTotal).FirstOrDefault() > (decimal)0.00*/)
                        errMessage = "Campos del detalle están incompletos, favor llenarlos.";
                }


                if (cotizacion.instalacion.Length <= 0 || cotizacion.instalacion == null ||
                    cotizacion.garantia_equipos.Length <= 0 || cotizacion.garantia_equipos == null ||
                    cotizacion.forma_pago.Length <= 0 || cotizacion.forma_pago == null ||
                    cotizacion.validez.Length <= 0 || cotizacion.validez == null ||
                    cotizacion.observaciones.Length <= 0 || cotizacion.observaciones == null)
                    errMessage = "Campos de la cotización están incompletos, favor llenarlos.";

                if (errMessage == "")
                {
                    cotizacion = _cotizacionesRepo.Edit(cotizacion);
                    TempData["SuccessMessage"] = cotizacion.id_cotizacion.ToString() + ", cotizacion guardada exitosamente";
                    bolret = true;
                }
                ViewBag.empleados = GetEmpleados();
            }
            catch (Exception ex)
            {
                errMessage = errMessage + " " + ex.Message;
            }



            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];


            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                ViewBag.empleados = GetEmpleados();
                return View(cotizacion);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        public IActionResult Delete(int id)
        {
            //cotizaciones cotizacion = _cotizacionesRepo.GetQuote(id);
            cotizaciones cotizacion = _context.cotizaciones.Include(d => d.DetallesCotizacion).Where(c => c.id_cotizacion == id).FirstOrDefault();
            TempData.Keep();
            return View(cotizacion);
        }


        [HttpPost]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            cotizaciones cotizacion = new cotizaciones();
            try
            {
                cotizacion = _cotizacionesRepo.Delete(id);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                if (ex.InnerException != null)
                    errMessage = ex.InnerException.Message;

                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(cotizacion);
            }

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = "Cotización " + cotizacion.nombre_cliente + " borrada exitosamente";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }


        private List<SelectListItem> GetEmpleados()
        {
            var lstQuotes = new List<SelectListItem>();
            List<empleados> items = _context.empleados.ToList();

            lstQuotes = items.Select(emp => new SelectListItem()
            {
                Value = emp.id_empleado.ToString(),
                Text = emp.nombres
            }).ToList();

            var defQuote = new SelectListItem()
            {
                Value = null,
                Text = "Seleccione el empleado"
            };

            lstQuotes.Insert(0, defQuote);

            return lstQuotes;
        }

    }
}
