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

namespace SGVisitasTecnicasASPCore.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly SgvtDB _context;

        public CategoriasController(SgvtDB context)
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

        // GET: CategoriasController
        [Authorize]
        public ActionResult Index()
        {
            List<categorias> categorias = new List<categorias>();
            categorias = _context.categorias.ToList();
            return View(categorias);
        }

        // GET: CategoriasController/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            categorias categoria = new categorias();
            categoria = _context.categorias.Where(x => x.id_categoria == id).FirstOrDefault();
            return View(categoria);
        }

        // GET: CategoriasController/Create
        [Authorize]
        public ActionResult Create()
        {
            return View(new categorias());
        }

        // POST: CategoriasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(categorias categoria)
        {
            try
            {
                if (!IsAnyNullOrEmpty(categoria))
                {
                    _context.categorias.Add(categoria);
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

        // GET: CategoriasController/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            categorias categoria = new categorias();
            categoria = _context.categorias.Where(x => x.id_categoria == id).FirstOrDefault();
            return View(categoria);
        }

        // POST: CategoriasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(categorias categoria)
        {
            if (!IsAnyNullOrEmpty(categoria))
            {
                _context.Entry(categoria).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                return View();
            }

            return RedirectToAction("Index");
        }

        // GET: CategoriasController/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            categorias categoria = new categorias();
            categoria = _context.categorias.Where(x => x.id_categoria == id).FirstOrDefault();
            return View(categoria);
        }

        // POST: CategoriasController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            categorias categoria = _context.categorias.Where(x => x.id_categoria == id).FirstOrDefault();
            _context.categorias.Remove(categoria);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
