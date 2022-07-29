using Microsoft.EntityFrameworkCore;
using SGVisitasTecnicasASPCore.Interfaces;
using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Repositories
{
    public class EmpleadosRepo:IEmpleados
    {
        private readonly SgvtDB _context; // for connecting to efcore.
        public EmpleadosRepo(SgvtDB context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public empleados Create(empleados empleado)
        {
            _context.empleados.Add(empleado);
            _context.usuarios.Add(new Usuario { Nombre = empleado.nombres, Correo = empleado.email, Clave = empleado.password, token_recovery = null, Rol = empleado.perfil });
            _context.SaveChanges();
            return empleado;
        }

        public empleados Delete(int id)
        {
            empleados empleado = _context.empleados.Where(x => x.id_empleado == id).FirstOrDefault();
            _context.empleados.Remove(empleado);
            _context.SaveChanges();
            //_context.empleados.Attach(empleado);
            //_context.Entry(empleado).State = EntityState.Deleted;
            //_context.SaveChanges();
            return empleado;
        }

        public empleados Edit(empleados empleado)
        {
            _context.empleados.Attach(empleado);
            _context.Entry(empleado).State = EntityState.Modified;
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
            return empleado;
        }


        private List<empleados> DoSort(List<empleados> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "nombres")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.nombres).ToList();
                else
                    items = items.OrderByDescending(n => n.nombres).ToList();
            }
            else if (SortProperty.ToLower() == "apellidos")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.apellidos).ToList();
                else
                    items = items.OrderByDescending(d => d.apellidos).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(s => s.numero_documento).ToList();
                else
                    items = items.OrderByDescending(s => s.numero_documento).ToList();
            }

            return items;
        }

        public PaginatedList<empleados> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<empleados> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.empleados.Where(n => n.nombres.Contains(SearchText) || n.apellidos.Contains(SearchText))
                    .ToList();
            }
            else
                items = _context.empleados.ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<empleados> retItems = new PaginatedList<empleados>(items, pageIndex, pageSize);

            return retItems;
        }

        public empleados GetItem(int id)
        {
            empleados item = _context.empleados.Where(u => u.id_empleado == id).FirstOrDefault();
            return item;
        }
        public bool IsEmployeeExists(string name)
        {
            int ct = _context.empleados.Where(n => n.nombres.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsEmployeeExists(string name, int id_empleado)
        {
            int ct = _context.empleados.Where(n => n.nombres.ToLower() == name.ToLower() && n.id_empleado != id_empleado).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
