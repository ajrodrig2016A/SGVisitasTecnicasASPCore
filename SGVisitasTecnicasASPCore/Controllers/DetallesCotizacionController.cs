using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SGVisitasTecnicasASPCore.Models;

namespace SGVisitasTecnicasASPCore.Controllers
{
    public class DetallesCotizacionController : Controller
    {
        private readonly SgvtDB _context;

        public DetallesCotizacionController(SgvtDB context)
        {
            _context = context;
        }
        // GET: DetallesCotizacionController
        public ActionResult Index()
        {
            List<detalles_cotizacion> lstDetallesCotizacion = new List<detalles_cotizacion>();
            lstDetallesCotizacion = _context.detallesCotizacion.ToList();
            return View(lstDetallesCotizacion);
        }

        // GET: DetallesCotizacionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DetallesCotizacionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DetallesCotizacionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DetallesCotizacionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: DetallesCotizacionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: DetallesCotizacionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: DetallesCotizacionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
