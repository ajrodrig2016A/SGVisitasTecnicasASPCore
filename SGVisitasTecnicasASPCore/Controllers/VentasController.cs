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
    public class VentasController : Controller
    {
        private readonly IClientes _clientesRepo;
        private readonly IVentas _ventasRepo;
        private readonly SgvtDB _context;
        public VentasController(IVentas ventasRepo, IClientes clientesRepo, SgvtDB context) // here the repository will be passed by the dependency injection.
        {
            _ventasRepo = ventasRepo;
            _clientesRepo = clientesRepo;
            _context = context;
        }

        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Código de Venta");
            sortModel.AddColumn("Número de Factura");
            sortModel.AddColumn("Fecha de Creación");
            sortModel.AddColumn("Fecha de Cierre");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<ventas> items = _ventasRepo.GetQuotes(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);


            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(items);
        }

        private void PopulateViewbags()
        {
            ViewBag.clientes = GetClientes();

            ViewBag.clientes = GetClientes();

            ViewBag.productos = GetProductos();

            ViewBag.PrUnitariosAllProducts = GetPrUnitariosAllProducts();
        }

        // GET: VentasController/Details/5
        public IActionResult Details(int id)
        {
            PopulateViewbags();
            ventas venta = _context.ventas.Include(d => d.DetallesVenta).Where(c => c.id_venta == id).FirstOrDefault();
            for (int i = 0; i < venta.DetallesVenta.Count; i++)
            {
                venta.DetallesVenta[i].codigoProductoVta = venta.DetallesVenta[i].id_producto.ToString();
                venta.DetallesVenta[i].descripcion = venta.DetallesVenta[i].Producto.nombre;
                /*venta.DetallesVenta[i].marca = venta.DetallesVenta[i].Producto.Marca.nombre;
                venta.DetallesVenta[i].unidad = venta.DetallesVenta[i].Producto.Unidad.nombre;*/
            }
            return View(venta);
        }

        public IActionResult Create()
        {
            PopulateViewbags();
            ventas venta = new ventas();
            venta.DetallesVenta.Add(new detalles_venta() { id_detalle_venta = 1 });
            //foreach (var item in venta.DetallesVenta.ToList())
            //{
            //    item.codigoProducto = item.id_producto.ToString();
            //}
            venta.codigo_venta = _ventasRepo.GetNewSaleNumber();
            venta.numero_factura = _ventasRepo.GetNewInvoiceNumber();
            return View(venta);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 4194304)]
        public IActionResult Create(ventas venta)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                venta.DetallesVenta.RemoveAll(n => n.cantidad == (decimal)0.00 || n.valorUnitario == (decimal)0.00);
                venta.DetallesVenta.RemoveAll(t => t.valorTotal == (decimal)0.00);
                //venta.DetallesVenta.RemoveAll(n => n.IsDeleted == true);
                if (venta.numero_factura.Length < 6 || venta.numero_factura == null)
                    errMessage = "La longitud del número de la factura debe ser al menos de 6 caracteres";

                if (_ventasRepo.IsQuoteCodeExists(venta.id_venta) == true)
                    errMessage = errMessage + " " + " El código de la venta " + venta.id_venta + " ya existe";

                for (int i = 0; i < venta.DetallesVenta.Count; i++)
                {
                    venta.DetallesVenta[i].id_producto = int.Parse(venta.DetallesVenta[i].codigoProductoVta.Trim());
                }

                //if (_ventasRepo.IsQuoteExists(venta.nombre_cliente) == true)
                //    errMessage = errMessage + " " + " El nombre de cliente " + venta.nombre_cliente + " ya existe en la venta";

                foreach (var item in venta.DetallesVenta.ToList())
                {
                    if (String.IsNullOrEmpty(item.descripcion))
                        errMessage = "Campos del detalle están incompletos, favor llenarlos.";
                }


                if (venta.subtotal < Utils.ZERO_DEC || venta.Iva < Utils.ZERO_DEC || venta.total < Utils.ZERO_DEC)
                    errMessage = "Montos de la venta deben ser mayores que cero.";

                if (errMessage == "")
                {
                    venta = _ventasRepo.Create(venta);
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
                return View(venta);
            }
            else
            {
                TempData["SuccessMessage"] = "Venta " + venta.id_venta + " creada exitosamente";
                return RedirectToAction(nameof(Index));
            }
        }


        public IActionResult Edit(int id)
        {
            //ventas venta = _ventasRepo.GetQuote(id);
            PopulateViewbags();
            ventas venta = _context.ventas.Include(d => d.DetallesVenta).Where(c => c.id_venta == id).FirstOrDefault();
            for (int i = 0; i < venta.DetallesVenta.Count; i++)
            {
                venta.DetallesVenta[i].codigoProductoVta = venta.DetallesVenta[i].id_producto.ToString();
                venta.DetallesVenta[i].descripcion = venta.DetallesVenta[i].Producto.nombre;
                /*venta.DetallesVenta[i].marca = venta.DetallesVenta[i].Producto.Marca.nombre;
                venta.DetallesVenta[i].unidad = venta.DetallesVenta[i].Producto.Unidad.nombre;*/
            }

            TempData.Keep();
            return View(venta);
        }

        [HttpPost]
        public IActionResult Edit(ventas venta)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                venta.DetallesVenta.RemoveAll(n => n.cantidad == (decimal)0.00 || n.valorUnitario == (decimal)0.00);
                venta.DetallesVenta.RemoveAll(t => t.valorTotal == (decimal)0.00);

                if (venta.numero_factura.Length < 6 || venta.numero_factura == null)
                    errMessage = "La longitud del número de la factura debe ser al menos de 6 caracteres";

                if (_ventasRepo.IsQuoteCodeExists(venta.id_venta) == true)
                    errMessage = errMessage + " " + " El código de la venta " + venta.id_venta + " ya existe";

                for (int i = 0; i < venta.DetallesVenta.Count; i++)
                {
                    venta.DetallesVenta[i].id_producto = int.Parse(venta.DetallesVenta[i].codigoProductoVta.Trim());
                }

                //if (_ventasRepo.IsQuoteExists(venta.nombre_cliente) == true)
                //    errMessage = errMessage + " " + " El nombre de cliente " + venta.nombre_cliente + " ya existe en la venta";

                foreach (var item in venta.DetallesVenta.ToList())
                {
                    if (String.IsNullOrEmpty(item.descripcion))
                        errMessage = "Campos del detalle están incompletos, favor llenarlos.";
                }


                if (venta.subtotal < Utils.ZERO_DEC || venta.Iva < Utils.ZERO_DEC || venta.total < Utils.ZERO_DEC)
                    errMessage = "Montos de la venta deben ser mayores que cero.";


                if (errMessage == "")
                {
                    venta = _ventasRepo.Edit(venta);
                    TempData["SuccessMessage"] = "Venta " + venta.id_venta.ToString() + ", guardada exitosamente";
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
                return View(venta);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        public IActionResult Delete(int id)
        {
            PopulateViewbags();
            ventas venta = _context.ventas.Include(d => d.DetallesVenta).Where(c => c.id_venta == id).FirstOrDefault();            
            for (int i = 0; i < venta.DetallesVenta.Count; i++)
            {
                venta.DetallesVenta[i].codigoProductoVta = venta.DetallesVenta[i].id_producto.ToString();
                venta.DetallesVenta[i].descripcion = venta.DetallesVenta[i].Producto.nombre;
                //venta.DetallesVenta[i].marca = venta.DetallesVenta[i].Producto.Marca.nombre;
                //venta.DetallesVenta[i].unidad = venta.DetallesVenta[i].Producto.Unidad.nombre;
            }
            TempData.Keep();
            return View(venta);
        }


        [HttpPost]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            bool bolret = false;
            string errMessage = "";
            //ventas venta = new ventas();
            ventas venta = _context.ventas.Include(d => d.DetallesVenta).Where(c => c.id_venta == id).FirstOrDefault();
            try
            {
                bolret = _ventasRepo.Delete(id);
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
                return View(venta);
            }
            else
            {
                int currentPage = 1;
                if (TempData["CurrentPage"] != null)
                    currentPage = (int)TempData["CurrentPage"];

                TempData["SuccessMessage"] = "Venta " + venta.codigo_venta + " borrada exitosamente";
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

    }
}
