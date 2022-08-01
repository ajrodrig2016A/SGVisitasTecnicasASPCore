using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SGVisitasTecnicasASPCore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGVisitasTecnicasASPCore.Interfaces;

namespace SGVisitasTecnicasASPCore.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly IWebHostEnvironment _webHost;
        private readonly IMarcas _marcasRepo;
        private readonly IUnidades _unidadesRepo;
        private readonly ICategorias _categoriasRepo;
        private readonly IProductos _productosRepo;
        public ProductosController(IProductos productosRepo, IMarcas marcasRepo, IUnidades unidadesRepo, ICategorias categoriasRepo, IWebHostEnvironment webHost) // here the repository will be passed by the dependency injection.
        {
            _webHost = webHost;
            _productosRepo = productosRepo;
            _marcasRepo = marcasRepo;
            _unidadesRepo = unidadesRepo;
            _categoriasRepo = categoriasRepo;
        }

        private string GetUploadedFileName(productos producto)
        {
            string uniqueFileName = null;

            if (producto.ImageFile != null)
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "image");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + producto.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    producto.ImageFile.CopyTo(fileStream);
                }
            }
            //ViewData["message"] = $"{producto.ImageFile.Length} bytes uploaded successfully!";
            return uniqueFileName;
        }


        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Nombre");
            sortModel.AddColumn("Marca");
            sortModel.AddColumn("Descripción");
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

            PaginatedList<productos> items = _productosRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);


            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(items);
        }

        private void PopulateViewbags()
        {
            ViewBag.Marcas = GetMarcas();

            ViewBag.Unidades = GetUnidades();

            ViewBag.Categorias = GetCategorias();

        }

        public IActionResult Create()
        {
            productos item = new productos();
            PopulateViewbags();
            return View(item);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 4194304)]
        public IActionResult Create(productos product)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (product.descripcion.Length < 4 || product.descripcion == null)
                    errMessage = "Descripción del producto Debe tener al menos 4 caracteres";



                if (_productosRepo.IsItemCodeExists(product.id_producto) == true)
                    errMessage = errMessage + " " + " El código de producto " + product.id_producto + " ya existe";



                if (_productosRepo.IsItemExists(product.nombre) == true)
                    errMessage = errMessage + " " + " El nombre del producto " + product.nombre + " ya existe";

                if (errMessage == "")
                {

                    string uniqueFileName = GetUploadedFileName(product);
                    product.ImageName = uniqueFileName;


                    product = _productosRepo.Create(product);
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
                return View(product);

                //return RedirectToAction(nameof(Create));
            }
            else
            {
                TempData["SuccessMessage"] = "Producto " + product.nombre + " creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
        }

        //public IActionResult Details(int id) //Read
        //{
        //    productos item = _productosRepo.GetItem(id);
        //    return View(item);
        //}


        public IActionResult Edit(int id)
        {
            productos producto = _productosRepo.GetItem(id);
            ViewBag.Marcas = GetMarcas();
            ViewBag.Unidades = GetUnidades();
            ViewBag.Categorias = GetCategorias();
            TempData.Keep();
            return View(producto);
        }

        [HttpPost]
        public IActionResult Edit(productos product)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (product.descripcion.Length < 4 || product.descripcion == null)
                    errMessage = "Descripción del producto debe tener al menos 4 caracteres";


                if (_productosRepo.IsItemCodeExists(product.id_producto, product.nombre) == true)
                    errMessage = errMessage + " " + " El código de producto " + product.id_producto.ToString() + " ya existe";

                if (_productosRepo.IsItemExists(product.nombre, product.id_producto) == true)
                    errMessage = errMessage + " El nombre del producto " + product.nombre + " ya existe";

                if (product.ImageFile != null)
                {
                    string filePath = Path.Combine(_webHost.WebRootPath, "image", product.ImageName);
                    System.IO.File.Delete(filePath);

                    string uniqueFileName = GetUploadedFileName(product);
                    product.ImageName = uniqueFileName;
                }

                if (errMessage == "")
                {
                    product = _productosRepo.Edit(product);
                    TempData["SuccessMessage"] = product.nombre + ", producto guardado exitosamente";
                    bolret = true;
                }
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
                ViewBag.Marcas = GetMarcas();
                ViewBag.Unidades = GetUnidades();
                ViewBag.Categorias = GetCategorias();
                return View(product);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        public IActionResult Delete(int id)
        {
            PopulateViewbags();
            productos producto = _productosRepo.GetItem(id);
            TempData.Keep();
            return View(producto);
        }


        [HttpPost]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            productos producto = new productos();
            try
            {
                //delete from wwwroot/image
                var imagePath = Path.Combine(_webHost.WebRootPath, "image", producto.ImageName);
                if (System.IO.File.Exists(imagePath))
                    System.IO.File.Delete(imagePath);
                producto = _productosRepo.Delete(id);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                if (ex.InnerException != null)
                    errMessage = ex.InnerException.Message;

                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(producto);
            }

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = "Producto " + producto.nombre + " borrado exitosamente";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }

        private List<SelectListItem> GetMarcas()
        {
            var lstItems = new List<SelectListItem>();

            PaginatedList<marcas> items = _marcasRepo.GetItems("nombre", SortOrder.Ascending, "", 1, 1000);
            lstItems = items.Select(ut => new SelectListItem()
            {
                Value = ut.id_marca.ToString(),
                Text = ut.nombre
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Seleccione la Marca----"
            };

            lstItems.Insert(0, defItem);

            return lstItems;
        }
        private List<SelectListItem> GetUnidades()
        {
            var lstItems = new List<SelectListItem>();

            PaginatedList<unidades> items = _unidadesRepo.GetItems("nombre", SortOrder.Ascending, "", 1, 1000);
            lstItems = items.Select(ut => new SelectListItem()
            {
                Value = ut.id_unidad.ToString(),
                Text = ut.nombre
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Seleccione la Unidad----"
            };

            lstItems.Insert(0, defItem);

            return lstItems;
        }

        private List<SelectListItem> GetCategorias()
        {
            var lstItems = new List<SelectListItem>();

            PaginatedList<categorias> items = _categoriasRepo.GetItems("nombre", SortOrder.Ascending, "", 1, 1000);
            lstItems = items.Select(ut => new SelectListItem()
            {
                Value = ut.id_categoria.ToString(),
                Text = ut.nombre
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Seleccione la Categoría----"
            };

            lstItems.Insert(0, defItem);

            return lstItems;
        }

    }
}
