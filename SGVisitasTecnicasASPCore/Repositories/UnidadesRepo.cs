using Microsoft.EntityFrameworkCore;
using SGVisitasTecnicasASPCore.Interfaces;
using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Repositories
{
    public class UnidadesRepo:IUnidades
    {
        private readonly SgvtDB _context; // for connecting to efcore.
        public UnidadesRepo(SgvtDB context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public unidades Create(unidades unidad)
        {
            _context.unidades.Add(unidad);
            _context.SaveChanges();
            return unidad;
        }

        public unidades Delete(int id)
        {
            unidades unidad = _context.unidades.Where(x => x.id_unidad == id).FirstOrDefault();
            _context.unidades.Remove(unidad);
            _context.SaveChanges();
            return unidad;
        }

        public unidades Edit(unidades unidad)
        {
            _context.unidades.Attach(unidad);
            _context.Entry(unidad).State = EntityState.Modified;
            _context.SaveChanges();
            return unidad;
        }


        private List<unidades> DoSort(List<unidades> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "nombre")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.nombre).ToList();
                else
                    items = items.OrderByDescending(n => n.nombre).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.descripcion).ToList();
                else
                    items = items.OrderByDescending(d => d.descripcion).ToList();
            }

            return items;
        }

        public PaginatedList<unidades> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<unidades> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.unidades.Where(n => n.nombre.Contains(SearchText) || n.descripcion.Contains(SearchText))
                    .ToList();
            }
            else
                items = _context.unidades.ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<unidades> retItems = new PaginatedList<unidades>(items, pageIndex, pageSize);

            return retItems;
        }

        public unidades GetItem(int id)
        {
            unidades item = _context.unidades.Where(u => u.id_unidad == id).FirstOrDefault();
            return item;
        }
        public bool IsUnitNameExists(string name)
        {
            int ct = _context.unidades.Where(n => n.nombre.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsUnitNameExists(string name, int id)
        {
            int ct = _context.unidades.Where(n => n.nombre.ToLower() == name.ToLower() && n.id_unidad != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
