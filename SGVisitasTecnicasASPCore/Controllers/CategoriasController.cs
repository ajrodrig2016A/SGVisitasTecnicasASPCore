using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SGVisitasTecnicasASPCore.Models;
using Microsoft.AspNetCore.Authorization;
using SGVisitasTecnicasASPCore.Data;
using SGVisitasTecnicasASPCore.Interfaces;

namespace SGVisitasTecnicasASPCore.Controllers
{
    [Authorize]
    public class CategoriasController : Controller
    {
        private readonly ICategorias _Repo;
        public CategoriasController(ICategorias repo) // here the repository will be passed by the dependency injection.
        {
            _Repo = repo;
        }


        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Nombre");
            sortModel.AddColumn("Descripción");
            sortModel.AddColumn("Estado");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<categorias> items = _Repo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);


            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(items);
        }


        public IActionResult Create()
        {
            categorias item = new categorias();
            return View(item);
        }

        [HttpPost]
        public IActionResult Create(categorias item)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (item.descripcion.Length < 4 || item.descripcion == null)
                    errMessage = "Descripción debe ser al menos de 4 caracteres";

                if (_Repo.IsItemExists(item.nombre) == true)
                    errMessage = errMessage + " " + " nombre " + item.nombre + " ya existe";

                if (errMessage == "")
                {
                    item = _Repo.Create(item);
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
                return View(item);
            }
            else
            {
                TempData["SuccessMessage"] = "Categoría " + item.nombre + " creada exitosamente";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id) //Read
        {
            categorias item = _Repo.GetItem(id);
            return View(item);
        }


        public IActionResult Edit(int id)
        {
            categorias item = _Repo.GetItem(id);
            TempData.Keep();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(categorias item)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (item.descripcion.Length < 4 || item.descripcion == null)
                    errMessage = "Descripción debe ser al menos de 4 caracteres";

                if (_Repo.IsItemExists(item.nombre, item.id_categoria) == true)
                    errMessage = errMessage + item.nombre + " ya existe";

                if (errMessage == "")
                {
                    item = _Repo.Edit(item);
                    TempData["SuccessMessage"] = "Categoría " + item.nombre + ", guardada exitosamente";
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
                return View(item);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        public IActionResult Delete(int id)
        {
            categorias item = _Repo.GetItem(id);
            TempData.Keep();
            return View(item);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            categorias item = new categorias();
            try
            {
                item = _Repo.Delete(id);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(item);

            }

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = "Categoría " + item.nombre + " borrada exitosamente";
            return RedirectToAction(nameof(Index), new { pg = currentPage });

        }

    }
}
