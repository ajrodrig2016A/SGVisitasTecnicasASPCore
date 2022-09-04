using Microsoft.EntityFrameworkCore;
using SGVisitasTecnicasASPCore.Interfaces;
using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Repositories
{
    public class ClientesRepo:IClientes
    {
        private readonly SgvtDB _context; // for connecting to efcore.
        public ClientesRepo(SgvtDB context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public clientes Create(clientes cliente)
        {
            _context.clientes.Add(cliente);
            _context.usuarios.Add(new Usuario { Nombre = cliente.nombres, Correo = cliente.email, Clave = cliente.password, token_recovery = null, Rol = "CLI" });
            _context.SaveChanges();
            return cliente;
        }

        public clientes Delete(int id)
        {
            clientes cliente = _context.clientes.Where(x => x.id_cliente == id).FirstOrDefault();
            Usuario user = _context.usuarios.Where(u => u.Correo.Trim() == cliente.email.Trim()).FirstOrDefault();
            _context.clientes.Remove(cliente);
            _context.usuarios.Remove(user);
            _context.SaveChanges();
            //_context.clientes.Attach(cliente);
            //_context.Entry(cliente).State = EntityState.Deleted;
            //_context.SaveChanges();
            return cliente;
        }

        public clientes Edit(clientes cliente)
        {
            _context.clientes.Attach(cliente);
            _context.Entry(cliente).State = EntityState.Modified;
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

            _context.SaveChanges();
            return cliente;
        }


        private List<clientes> DoSort(List<clientes> items, string SortProperty, SortOrder sortOrder)
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
            else if (SortProperty.ToLower() == "fecha de registro")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.fecha_registro).ToList();
                else
                    items = items.OrderByDescending(d => d.fecha_registro).ToList();
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

        public PaginatedList<clientes> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<clientes> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.clientes.Where(n => n.numero_documento.Contains(SearchText) || n.nombres.Contains(SearchText) || n.apellidos.Contains(SearchText) || n.genero.Contains(SearchText) || n.direccion.Contains(SearchText) || n.email.Contains(SearchText) || n.numero_contacto.Contains(SearchText))
                    .ToList();
            }
            else
                items = _context.clientes.ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<clientes> retItems = new PaginatedList<clientes>(items, pageIndex, pageSize);

            return retItems;
        }

        public clientes GetItem(int id)
        {
            clientes item = _context.clientes.Where(u => u.id_cliente == id).FirstOrDefault();
            return item;
        }
        public bool IsCustomerExists(string name)
        {
            int ct = _context.clientes.Where(n => n.nombres.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsCustomerExists(string name, int id_cliente)
        {
            int ct = _context.clientes.Where(n => n.nombres.ToLower() == name.ToLower() && n.id_cliente != id_cliente).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
