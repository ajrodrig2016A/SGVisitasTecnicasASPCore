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

namespace SGVisitasTecnicasASPCore.Controllers
{
    public class EmpleadosController : Controller
    {
        private readonly SgvtDB _context;

        public EmpleadosController(SgvtDB context)
        {
            _context = context;
        }
        bool IsAnyNullOrEmpty(object myObject)
        {
            foreach (PropertyInfo pi in myObject.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = (string)pi.GetValue(myObject);
                    if (string.IsNullOrEmpty(value))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // GET: Empleados
        [Authorize]
        public ActionResult Index()
        {
            List<empleados> empleados = new List<empleados>();
            empleados = _context.empleados.ToList();
            return View(empleados);
        }

        // GET: Empleados/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            empleados empleado = new empleados();
            empleado = _context.empleados.Where(x => x.id_empleado == id).FirstOrDefault();
            return View(empleado);
        }

        // GET: Empleados/Create
        [Authorize]
        public ActionResult Create()
        {
            return View(new empleados());
        }

        // POST: Empleados/Create
        [HttpPost]
        public ActionResult Create(empleados empleado)
        {
            try
            {
                if (!IsAnyNullOrEmpty(empleado) && Utils.VerificaIdentificacion(empleado.numero_documento))
                {
                    _context.empleados.Add(empleado);
                    _context.SaveChanges();
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            return RedirectToAction("Index");
        }

        // GET: Empleados/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            empleados empleado = new empleados();
            empleado = _context.empleados.Where(x => x.id_empleado == id).FirstOrDefault();
            return View(empleado);
        }

        // POST: Empleados/Edit/5
        [HttpPost]
        public ActionResult Edit(empleados empleado)
        {
            if (!IsAnyNullOrEmpty(empleado))
            {
                _context.Entry(empleado).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                return View();
            }

            return RedirectToAction("Index");

        }

        // GET: Empleados/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            empleados empleado = new empleados();
            empleado = _context.empleados.Where(x => x.id_empleado == id).FirstOrDefault();
            return View(empleado);
        }

        // POST: Empleados/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            empleados empleado = _context.empleados.Where(x => x.id_empleado == id).FirstOrDefault();
            _context.empleados.Remove(empleado);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
