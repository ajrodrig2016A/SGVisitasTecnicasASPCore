using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
                if (!Utils.IsAnyNullOrEmpty(empleado) /*&& Utils.VerificaIdentificacion(empleado.numero_documento)*/)
                {
                    _context.empleados.Add(empleado);
                    _context.usuarios.Add(new Usuario { Nombre = empleado.nombres, Correo = empleado.email, Clave = empleado.password, token_recovery = null, Rol = empleado.perfil });
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
            if (!Utils.IsAnyNullOrEmpty(empleado))
            {
                _context.Entry(empleado).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                Usuario user = new Usuario();
                user = _context.usuarios.Where(u => u.Correo.Trim() == empleado.email.Trim()).FirstOrDefault();
                if (user != null && (!empleado.nombres.Trim().Equals(user.Nombre.Trim()) || !empleado.email.Trim().Equals(user.Correo.Trim()) || !empleado.password.Trim().Equals(user.Clave.Trim()) || !empleado.perfil.Trim().Equals(user.Rol.Trim())))
                {
                    user.Nombre = empleado.nombres;
                    user.Correo = empleado.email;
                    user.Clave = empleado.password;
                    user.Rol = empleado.perfil;
                    _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                /*else
                {
                    _context.usuarios.Add(new Usuario { Nombre = empleado.nombres, Correo = empleado.email, Clave = empleado.password, token_recovery = null, Rol = empleado.perfil });
                }*/
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
            Usuario user = _context.usuarios.Where(u => u.Correo.Trim() == empleado.email.Trim()).FirstOrDefault();
            _context.empleados.Remove(empleado);
            _context.usuarios.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
