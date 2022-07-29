using Microsoft.EntityFrameworkCore;
using SGVisitasTecnicasASPCore.Interfaces;
using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Repositories
{
    public class CategoriasRepo:ICategorias
    {
        private readonly SgvtDB _context; // for connecting to efcore.
        public CategoriasRepo(SgvtDB context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public categorias Create(categorias categoria)
        {
            _context.categorias.Add(categoria);
            _context.SaveChanges();
            return categoria;
        }

        public categorias Delete(int id)
        {
            categorias categoria = _context.categorias.Where(x => x.id_categoria == id).FirstOrDefault();
            _context.categorias.Remove(categoria);
            _context.SaveChanges();
            //_context.categorias.Attach(categoria);
            //_context.Entry(categoria).State = EntityState.Deleted;
            //_context.SaveChanges();
            return categoria;
        }

        public categorias Edit(categorias categoria)
        {
            _context.categorias.Attach(categoria);
            _context.Entry(categoria).State = EntityState.Modified;
            _context.SaveChanges();
            return categoria;
        }


        private List<categorias> DoSort(List<categorias> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "nombre")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.nombre).ToList();
                else
                    items = items.OrderByDescending(n => n.nombre).ToList();
            }
            else if (SortProperty.ToLower() == "descripcion")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.descripcion).ToList();
                else
                    items = items.OrderByDescending(d => d.descripcion).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(s => s.estado).ToList();
                else
                    items = items.OrderByDescending(s => s.estado).ToList();
            }

            return items;
        }

        public PaginatedList<categorias> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<categorias> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.categorias.Where(n => n.nombre.Contains(SearchText) || n.descripcion.Contains(SearchText))
                    .ToList();
            }
            else
                items = _context.categorias.ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<categorias> retItems = new PaginatedList<categorias>(items, pageIndex, pageSize);

            return retItems;
        }

        public categorias GetItem(int id)
        {
            categorias item = _context.categorias.Where(u => u.id_categoria == id).FirstOrDefault();
            return item;
        }
        public bool IsItemExists(string name)
        {
            int ct = _context.categorias.Where(n => n.nombre.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int id_categoria)
        {
            int ct = _context.categorias.Where(n => n.nombre.ToLower() == name.ToLower() && n.id_categoria != id_categoria).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
