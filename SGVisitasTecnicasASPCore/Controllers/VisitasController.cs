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
    public class VisitasController : Controller
    {
        private readonly IClientes _clientesRepo;
        private readonly IEmpleados _empleadosRepo;
        private readonly IVisitas _visitasRepo;
        public VisitasController(IVisitas visitasRepo, IClientes clientesRepo, IEmpleados empleadosRepo) // here the repository will be passed by the dependency injection.
        {
            _visitasRepo = visitasRepo;
            _clientesRepo = clientesRepo;
            _empleadosRepo = empleadosRepo;
        }

        public IActionResult Index(string sortExpression = "", string SearchText = "", int pg = 1, int pageSize = 5)
        {
            SortModel sortModel = new SortModel();
            sortModel.AddColumn("Fecha Agendada");
            sortModel.AddColumn("Fecha Cierre");
            sortModel.AddColumn("Descripcion");
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

            PaginatedList<visitas> items = _visitasRepo.GetItems(sortModel.SortedProperty, sortModel.SortedOrder, SearchText, pg, pageSize);


            var pager = new PagerModel(items.TotalRecords, pg, pageSize);
            pager.SortExpression = sortExpression;
            this.ViewBag.Pager = pager;


            TempData["CurrentPage"] = pg;


            return View(items);
        }

        private void PopulateViewbags()
        {
            ViewBag.Clientes = GetClientes();

            ViewBag.Empleados = GetEmpleados();
        }

        public IActionResult Create()
        {
            visitas item = new visitas();
            PopulateViewbags();
            return View(item);
        }

        [HttpPost]
        [RequestFormLimits(MultipartBodyLengthLimit = 4194304)]
        public IActionResult Create(visitas visita)
        {
            bool bolret = false;
            string errMessage = "";
            try
            {
                clientes clsCliente = _clientesRepo.GetItem(visita.id_cliente);
                empleados clsEmpleado = _empleadosRepo.GetItem(visita.id_empleado);

                if (visita.descripcion.Length < 4 || visita.descripcion == null)
                    errMessage = "Descripción del requerimiento Debe tener al menos 4 caracteres";

                if (visita.ubicacionDispSeguridad.Length < 4 || visita.ubicacionDispSeguridad == null)
                    errMessage = "Ubicación del dispositivo de seguridad  Debe tener al menos 4 caracteres";

                if (_visitasRepo.IsItemCodeExists(visita.id_visita) == true)
                    errMessage = errMessage + " " + " El código de visita " + visita.id_visita + " ya existe";

                if (_visitasRepo.IsItemExists(clsCliente.nombres) == true)
                    errMessage = errMessage + " " + " La visita del cliente " + clsCliente.nombres + " ya existe";

                if (errMessage == "")
                {
                    visita = _visitasRepo.Create(visita);
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
                return View(visita);

                //return RedirectToAction(nameof(Create));
            }
            else
            {
                TempData["SuccessMessage"] = "Visita " + visita.id_visita + " creada exitosamente";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: VisitasController/Details/5
        public IActionResult Details(int id)
        {
            PopulateViewbags();
            visitas item = _visitasRepo.GetItem(id);
            return View(item);
        }

        public IActionResult Edit(int id)
        {
            PopulateViewbags();
            visitas visita = _visitasRepo.GetItem(id);
            TempData.Keep();
            return View(visita);
        }

        [HttpPost]
        public IActionResult Edit(visitas visita)
        {
            bool bolret = false;
            string errMessage = "";

            try
            {
                clientes clsCliente = _clientesRepo.GetItem(visita.id_cliente);
                empleados clsEmpleado = _empleadosRepo.GetItem(visita.id_empleado);

                if (visita.descripcion.Length < 4 || visita.descripcion == null)
                    errMessage = "Descripción del requerimiento Debe tener al menos 4 caracteres";

                if (visita.ubicacionDispSeguridad.Length < 4 || visita.ubicacionDispSeguridad == null)
                    errMessage = "Ubicación del dispositivo de seguridad  Debe tener al menos 4 caracteres";

                //if (_visitasRepo.IsItemCodeExists(visita.id_visita) == true)
                //    errMessage = errMessage + " " + " El código de visita " + visita.id_visita + " ya existe";

                //if (_visitasRepo.IsItemExists(clsCliente.nombres) == true)
                //    errMessage = errMessage + " " + " La visita del cliente " + clsCliente.nombres + " ya existe";


                if (errMessage == "")
                {
                    visita = _visitasRepo.Edit(visita);
                    TempData["SuccessMessage"] = "Visita " + visita.id_visita + ", guardada exitosamente";
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
                ViewBag.Clientes = GetClientes();
                ViewBag.Empleados = GetEmpleados();
                return View(visita);
            }
            else
                return RedirectToAction(nameof(Index), new { pg = currentPage });
        }

        public IActionResult Delete(int id)
        {
            PopulateViewbags();
            visitas visita = _visitasRepo.GetItem(id);
            TempData.Keep();
            return View(visita);
        }


        [HttpPost]
        public IActionResult Delete(int id, IFormCollection collection)
        {
            visitas visita = new visitas();
            try
            {
                visita = _visitasRepo.Delete(id);
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message;
                if (ex.InnerException != null)
                    errMessage = ex.InnerException.Message;

                TempData["ErrorMessage"] = errMessage;
                ModelState.AddModelError("", errMessage);
                return View(visita);
            }

            int currentPage = 1;
            if (TempData["CurrentPage"] != null)
                currentPage = (int)TempData["CurrentPage"];

            TempData["SuccessMessage"] = "Visita " + visita.id_visita + " borrada exitosamente";
            return RedirectToAction(nameof(Index), new { pg = currentPage });


        }

        private List<SelectListItem> GetClientes()
        {
            var lstItems = new List<SelectListItem>();

            PaginatedList<clientes> items = _clientesRepo.GetItems("nombres", SortOrder.Ascending, "", 1, 1000);
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
        private List<SelectListItem> GetEmpleados()
        {
            var lstItems = new List<SelectListItem>();

            PaginatedList<empleados> items = _empleadosRepo.GetItems("nombres", SortOrder.Ascending, "", 1, 1000);
            lstItems = items.Select(ut => new SelectListItem()
            {
                Value = ut.id_empleado.ToString(),
                Text = ut.nombres
            }).ToList();

            var defItem = new SelectListItem()
            {
                Value = "",
                Text = "----Seleccione el Empleado----"
            };

            lstItems.Insert(0, defItem);

            return lstItems;
        }

    }
}
