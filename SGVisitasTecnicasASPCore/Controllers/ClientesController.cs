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
    public class ClientesController : Controller
    {
        private readonly SgvtDB _context;

        public ClientesController(SgvtDB context)
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
        // GET: Clientes
        [Authorize]
        public ActionResult Index()
        {
            List<clientes> clientes = new List<clientes>();
            clientes = _context.clientes.ToList();
            return View(clientes);
        }

        // GET: Clientes/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            clientes cliente = new clientes();
            cliente = _context.clientes.Where(x => x.id_cliente == id).FirstOrDefault();
            return View(cliente);
        }

        // GET: Clientes/Create
        [Authorize]
        public ActionResult Create()
        {
            return View(new clientes());
        }

        // POST: Clientes/Create
        [HttpPost]
        public ActionResult Create(clientes cliente)
        {
            try
            {
                if (!IsAnyNullOrEmpty(cliente) && Utils.VerificaIdentificacion(cliente.numero_documento))
                {
                    _context.clientes.Add(cliente);
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

        // GET: Clientes/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            clientes cliente = new clientes();
            cliente = _context.clientes.Where(x => x.id_cliente == id).FirstOrDefault();
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        public ActionResult Edit(clientes cliente)
        {
            if (!IsAnyNullOrEmpty(cliente))
            {
                _context.Entry(cliente).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();
            }
            else
            {
                return View();
            }

            return RedirectToAction("Index");
        }

        // GET: Clientes/Delete/5
        [Authorize]
        public ActionResult Delete(int id)
        {
            clientes cliente = new clientes();
            cliente = _context.clientes.Where(x => x.id_cliente == id).FirstOrDefault();
            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            clientes cliente = _context.clientes.Where(x => x.id_cliente == id).FirstOrDefault();
            _context.clientes.Remove(cliente);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
