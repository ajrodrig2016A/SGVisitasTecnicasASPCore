using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SGVisitasTecnicasASPCore.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using SGVisitasTecnicasASPCore.Data;
using SGVisitasTecnicasASPCore.Interfaces;

namespace SGVisitasTecnicasASPCore.Controllers
{
    [Authorize]
    public class ClientesController : Controller
    {
        private readonly IClientes _Repo;
        public ClientesController(IClientes repo) // here the repository will be passed by the dependency injection.
        {
            _Repo = repo;
        }


        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Número Documento");
            sortModel.AddColumn("Nombres");
            sortModel.AddColumn("Apellidos");
            //sortModel.AddColumn("Fecha Registro");
            //sortModel.AddColumn("Es Activo");
            //sortModel.AddColumn("Email");
            //sortModel.AddColumn("Teléfono");
            //sortModel.AddColumn("Password");
            //sortModel.AddColumn("Perfil");
            //sortModel.AddColumn("Cargo");
            sortModel.ApplySort(sortExpression);
            ViewData["sortModel"] = sortModel;

            ViewBag.SearchText = SearchText;

            PaginatedList<clientes> items = _Repo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);


            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(items);
        }


        public IActionResult Create()
        {
            clientes item = new clientes();
            return View(item);
        }

        [HttpPost]
        public IActionResult Create(clientes item)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                if (item.numero_documento.Length < 10 || item.numero_documento == null)
                    errMessage = "Número de identificación debe ser al menos de 10 caracteres";

                if (_Repo.IsCustomerExists(item.nombres) == true)
                    errMessage = errMessage + " " + " nombres " + item.nombres + " ya existe";

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
                TempData["SuccessMessage"] = "Cliente " + item.nombres + " creado exitosamente";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Details(int id) //Read
        {
            clientes item = _Repo.GetItem(id);
            return View(item);
        }


        public IActionResult Edit(int id)
        {
            clientes item = _Repo.GetItem(id);
            TempData.Keep();
            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(clientes item)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                if (item.numero_documento.Length < 10 || item.numero_documento == null)
                    errMessage = "Número de identificación debe ser al menos de 10 caracteres";

                if (_Repo.IsCustomerExists(item.nombres, item.id_cliente) == true)
                    errMessage = errMessage + item.nombres + " ya existe";

                if (errMessage == "")
                {
                    item = _Repo.Edit(item);
                    TempData["SuccessMessage"] = "Cliente " + item.nombres + ", guardado exitosamente";
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
            clientes item = _Repo.GetItem(id);
            TempData.Keep();
            return View(item);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            clientes item = new clientes();
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

            TempData["SuccessMessage"] = "Cliente " + item.nombres + " borrado exitosamente";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }

    }
}
