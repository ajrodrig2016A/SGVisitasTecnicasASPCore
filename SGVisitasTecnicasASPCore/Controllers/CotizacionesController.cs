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
using SGVisitasTecnicasASPCore.Data;

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
            sortModel.AddColumn("Código");
            sortModel.AddColumn("Servicio");
            sortModel.AddColumn("Sector del Inmueble");
            sortModel.AddColumn("Dirección del Inmueble");
            sortModel.AddColumn("Fecha de Registro");
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
            ViewBag.clientes = GetClientes();

            ViewBag.empleados = GetEmpleados();

            ViewBag.productos = GetProductos();

            ViewBag.nombresProducto = GetDescripcionProducto();

            ViewBag.nombresUnidad = GetUnidades();

            ViewBag.IdUnidadesAllProducts = GetIdUnidadesAllProducts();

            ViewBag.nombresMarca = GetMarcas();

            ViewBag.IdMarcasAllProducts = GetIdMarcasAllProducts();

            ViewBag.PrUnitariosAllProducts = GetPrUnitariosAllProducts();
        }

        // GET: CotizacionesController/Details/5
        public IActionResult Details(int id)
        {
            PopulateViewbags();
            cotizaciones cotizacion = _cotizacionesRepo.GetQuote(id);

            for (int i = 0; i < cotizacion.DetallesCotizacion.Count; i++)
            {
                cotizacion.DetallesCotizacion[i].codigoProducto = cotizacion.DetallesCotizacion[i].id_producto.ToString();
                cotizacion.DetallesCotizacion[i].descripcion = cotizacion.DetallesCotizacion[i].Producto.descripcion;
                cotizacion.DetallesCotizacion[i].marca = cotizacion.DetallesCotizacion[i].Producto.Marca.nombre;
                cotizacion.DetallesCotizacion[i].unidad = cotizacion.DetallesCotizacion[i].Producto.Unidad.nombre;
            }
            return View(cotizacion);
        }

        public IActionResult Create()
        {
            PopulateViewbags();
            cotizaciones cotizacion = new cotizaciones();
            cotizacion.DetallesCotizacion.Add(new detalles_cotizacion() { id_detalle_cotización = 1 });
            //foreach (var item in cotizacion.DetallesCotizacion.ToList())
            //{
            //    item.codigoProducto = item.id_producto.ToString();
            //}
            cotizacion.codigo = _cotizacionesRepo.GetNewCTNumber();
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
                if (cotizacion.codigo.Length < 6 || cotizacion.codigo == null)
                    errMessage = "La longitud del código de la cotización debe ser al menos de 6 caracteres";

                if (_cotizacionesRepo.IsQuoteCodeExists(cotizacion.id_cotizacion) == true)
                    errMessage = errMessage + " " + " El código de la cotización " + cotizacion.id_cotizacion + " ya existe";

                for (int i = 0; i < cotizacion.DetallesCotizacion.Count; i++)
                {
                    cotizacion.DetallesCotizacion[i].id_producto = int.Parse(cotizacion.DetallesCotizacion[i].idProducto.Trim());
                    cotizacion.DetallesCotizacion[i].codigoProducto = _context.productos.Where(p => p.id_producto == cotizacion.DetallesCotizacion[i].id_producto).Select(c => c.nombre).FirstOrDefault();
                    cotizacion.DetallesCotizacion[i].descripcion = _context.productos.Where(p => p.id_producto == cotizacion.DetallesCotizacion[i].id_producto).Select(c => c.descripcion).FirstOrDefault();
                }

                //if (_cotizacionesRepo.IsQuoteExists(cotizacion.nombre_cliente) == true)
                //    errMessage = errMessage + " " + " El nombre de cliente " + cotizacion.nombre_cliente + " ya existe en la cotización";

                foreach (var item in cotizacion.DetallesCotizacion.ToList())
                {
                    if (String.IsNullOrEmpty(item.descripcion) || String.IsNullOrEmpty(item.marca) || String.IsNullOrEmpty(item.unidad))
                        errMessage = "Campos del detalle están incompletos, favor llenarlos.";
                }

                if (cotizacion.subtotal < Utils.ZERO_DEC)
                    errMessage = "El subtotal de la cotización debe ser mayor que cero.";

                if (cotizacion.instalacion.Length <= 0 || cotizacion.instalacion == null ||
                    cotizacion.garantia_equipos.Length <= 0 || cotizacion.garantia_equipos == null ||
                    cotizacion.forma_pago.Length <= 0 || cotizacion.forma_pago == null ||
                    cotizacion.validez.Length <= 0 || cotizacion.validez == null)
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
            }
            else
            {
                TempData["SuccessMessage"] = "Cotización " + cotizacion.id_cotizacion + " creada exitosamente";
                return RedirectToAction(nameof(Index));
            }
        }


        public IActionResult Edit(int id)
        {
            //cotizaciones cotizacion = _cotizacionesRepo.GetQuote(id);
            PopulateViewbags();
            cotizaciones cotizacion = _cotizacionesRepo.GetQuote(id);

            for (int i = 0; i < cotizacion.DetallesCotizacion.Count; i++)
            {
                cotizacion.DetallesCotizacion[i].codigoProducto = cotizacion.DetallesCotizacion[i].id_producto.ToString();
                cotizacion.DetallesCotizacion[i].descripcion = cotizacion.DetallesCotizacion[i].Producto.descripcion;
                cotizacion.DetallesCotizacion[i].marca = cotizacion.DetallesCotizacion[i].Producto.Marca.nombre;
                cotizacion.DetallesCotizacion[i].unidad = cotizacion.DetallesCotizacion[i].Producto.Unidad.nombre;
            }

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
                cotizacion.DetallesCotizacion.RemoveAll(d => d.IsDeleted == true);
                cotizacion.DetallesCotizacion.RemoveAll(n => n.cantidad == (decimal)0.00 || n.valorUnitario == (decimal)0.00);
                cotizacion.DetallesCotizacion.RemoveAll(t => t.valorTotal == (decimal)0.00);

                if (cotizacion.codigo.Length < 6 || cotizacion.codigo == null)
                    errMessage = "La longitud del código de la cotización debe ser al menos de 6 caracteres";

                //if (_cotizacionesRepo.IsQuoteCodeExists(cotizacion.id_cotizacion) == true)
                //    errMessage = errMessage + " " + " El código de la cotización " + cotizacion.id_cotizacion + " ya existe";

                for (int i = 0; i < cotizacion.DetallesCotizacion.Count; i++)
                {
                    cotizacion.DetallesCotizacion[i].id_producto = int.Parse(cotizacion.DetallesCotizacion[i].codigoProducto.Trim());
                    cotizacion.DetallesCotizacion[i].descripcion = _context.productos.Where(p => p.id_producto == cotizacion.DetallesCotizacion[i].id_producto).Select(c => c.nombre).FirstOrDefault();
                }

                //if (_cotizacionesRepo.IsQuoteExists(cotizacion.nombre_cliente) == true)
                //    errMessage = errMessage + " " + " El nombre de cliente " + cotizacion.nombre_cliente + " ya existe en la cotización";

                foreach (var item in cotizacion.DetallesCotizacion.ToList())
                {
                    if (String.IsNullOrEmpty(item.descripcion) || String.IsNullOrEmpty(item.marca) || String.IsNullOrEmpty(item.unidad))
                        errMessage = "Campos del detalle están incompletos, favor llenarlos.";
                }

                if (cotizacion.subtotal < Utils.ZERO_DEC)
                    errMessage = "El subtotal de la cotización debe ser mayor que cero.";

                if (cotizacion.instalacion.Length <= 0 || cotizacion.instalacion == null ||
                    cotizacion.garantia_equipos.Length <= 0 || cotizacion.garantia_equipos == null ||
                    cotizacion.forma_pago.Length <= 0 || cotizacion.forma_pago == null ||
                    cotizacion.validez.Length <= 0 || cotizacion.validez == null)
                    errMessage = "Campos de la cotización están incompletos, favor llenarlos.";


                if (errMessage == "")
                {
                    cotizacion = _cotizacionesRepo.Edit(cotizacion);
                    TempData["SuccessMessage"] = "Cotización " + cotizacion.id_cotizacion.ToString() + ", guardada exitosamente";
                    bolret = true;
                }
                PopulateViewbags();
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
                PopulateViewbags();
                return View(cotizacion);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        public IActionResult Delete(int id)
        {
            PopulateViewbags();
            cotizaciones cotizacion = _cotizacionesRepo.GetQuote(id);

            for (int i = 0; i < cotizacion.DetallesCotizacion.Count; i++)
            {
                cotizacion.DetallesCotizacion[i].codigoProducto = cotizacion.DetallesCotizacion[i].id_producto.ToString();
                cotizacion.DetallesCotizacion[i].descripcion = cotizacion.DetallesCotizacion[i].Producto.descripcion;
                cotizacion.DetallesCotizacion[i].marca = cotizacion.DetallesCotizacion[i].Producto.Marca.nombre;
                cotizacion.DetallesCotizacion[i].unidad = cotizacion.DetallesCotizacion[i].Producto.Unidad.nombre;
            }
            TempData.Keep();
            return View(cotizacion);
        }


        [HttpPost]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            bool bolret = false;
            string errMessage = "";

            cotizaciones cotizacion = _cotizacionesRepo.GetQuote(id);
            try
            {
                bolret = _cotizacionesRepo.Delete(id);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                if (ex.InnerException != null)
                    errMessage = ex.InnerException.Message;
            }

            if (bolret == false)
            {
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(cotizacion);
            }
            else
            {
                int currentPage = 1;
                if (TempData["CurrentPage"] != null)
                    currentPage = (int)TempData["CurrentPage"];

                TempData["SuccessMessage"] = "Cotización " + cotizacion.codigo + " borrada exitosamente";
                return RedirectToAction(nameof(Index), new { pg = currentPage });
            }
        }
        private List<SelectListItem> GetClientes()
        {
            var lstQuotes = new List<SelectListItem>();
            List<clientes> items = _context.clientes.ToList();

            lstQuotes = items.Select(emp => new SelectListItem()
            {
                Value = emp.id_cliente.ToString(),
                Text = emp.nombres
            }).ToList();

            var defQuote = new SelectListItem()
            {
                Value = null,
                Text = "----Seleccione el cliente----"
            };

            lstQuotes.Insert(0, defQuote);

            return lstQuotes;
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
                Text = "----Seleccione el empleado----"
            };

            lstQuotes.Insert(0, defQuote);

            return lstQuotes;
        }

        private List<SelectListItem> GetProductos()
        {
            var lstQuotes = new List<SelectListItem>();
            List<productos> items = _context.productos.ToList();

            lstQuotes = items.Select(emp => new SelectListItem()
            {
                Value = emp.id_producto.ToString(),
                Text = emp.nombre
            }).ToList();

            var defQuote = new SelectListItem()
            {
                Value = null,
                Text = "----Seleccione el producto----"
            };

            lstQuotes.Insert(0, defQuote);

            return lstQuotes;
        }

        private List<SelectListItem> GetIdUnidadesAllProducts()
        {
            var lstQuotes = new List<SelectListItem>();
            List<productos> items = _context.productos.ToList();

            lstQuotes = items.Select(emp => new SelectListItem()
            {
                Value = emp.id_producto.ToString(),
                Text = emp.id_unidad.ToString()
            }).ToList();

            return lstQuotes;
        }

        private List<SelectListItem> GetMarcas()
        {
            var lstQuotes = new List<SelectListItem>();
            List<marcas> items = _context.marcas.ToList();

            lstQuotes = items.Select(emp => new SelectListItem()
            {
                Value = emp.id_marca.ToString(),
                Text = emp.nombre
            }).ToList();

            return lstQuotes;
        }

        private List<SelectListItem> GetIdMarcasAllProducts()
        {
            var lstQuotes = new List<SelectListItem>();
            List<productos> items = _context.productos.ToList();

            lstQuotes = items.Select(emp => new SelectListItem()
            {
                Value = emp.id_producto.ToString(),
                Text = emp.id_marca.ToString()
            }).ToList();

            return lstQuotes;
        }

        private List<SelectListItem> GetPrUnitariosAllProducts()
        {
            var lstQuotes = new List<SelectListItem>();
            List<productos> items = _context.productos.ToList();

            lstQuotes = items.Select(emp => new SelectListItem()
            {
                Value = emp.id_producto.ToString(),
                Text = emp.precioUnitario.ToString()
            }).ToList();

            return lstQuotes;
        }

        private List<SelectListItem> GetDescripcionProducto()
        {
            var lstQuotes = new List<SelectListItem>();
            List<productos> items = _context.productos.ToList();

            lstQuotes = items.Select(emp => new SelectListItem()
            {
                Value = emp.id_producto.ToString(),
                Text = emp.descripcion
            }).ToList();

            var defQuote = new SelectListItem()
            {
                Value = null,
                Text = "----Seleccione el producto----"
            };

            lstQuotes.Insert(0, defQuote);

            return lstQuotes;
        }

        private List<SelectListItem> GetUnidades()
        {
            var lstQuotes = new List<SelectListItem>();
            List<unidades> items = _context.unidades.ToList();

            lstQuotes = items.Select(emp => new SelectListItem()
            {
                Value = emp.id_unidad.ToString(),
                Text = emp.nombre
            }).ToList();

            return lstQuotes;
        }

    }
}
