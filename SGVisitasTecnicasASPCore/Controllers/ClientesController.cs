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
                if (!Utils.IsAnyNullOrEmpty(cliente) /*&& Utils.VerificaIdentificacion(cliente.numero_documento)*/)
                {
                    _context.clientes.Add(cliente);
                    _context.usuarios.Add(new Usuario { Nombre = cliente.nombres, Correo = cliente.email, Clave = cliente.password, token_recovery = null, Rol = "CLI" });
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
            if (!Utils.IsAnyNullOrEmpty(cliente))
            {
                _context.Entry(cliente).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                Usuario user = new Usuario();
                user = _context.usuarios.Where(u => u.Correo.Trim() == cliente.email.Trim()).FirstOrDefault();
                if (user != null && (!cliente.nombres.Trim().Equals(user.Nombre.Trim()) || !cliente.email.Trim().Equals(user.Correo.Trim()) || !cliente.password.Trim().Equals(user.Clave.Trim()) || !user.Rol.Trim().Equals("CLI")))
                {
                    user.Nombre = cliente.nombres;
                    user.Correo = cliente.email;
                    user.Clave = cliente.password;
                    user.Rol = "CLI";
                    _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                /*else
                {
                    _context.usuarios.Add(new Usuario { Nombre = cliente.nombres, Correo = cliente.email, Clave = cliente.password, token_recovery = null, Rol = "CLI" });
                }*/
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
            Usuario user = _context.usuarios.Where(u => u.Correo.Trim() == cliente.email.Trim()).FirstOrDefault();
            _context.clientes.Remove(cliente);
            _context.usuarios.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
