using Microsoft.EntityFrameworkCore;
using SGVisitasTecnicasASPCore.Interfaces;
using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Repositories
{
    public class MarcasRepo:IMarcas
    {
        private readonly SgvtDB _context; // for connecting to efcore.
        public MarcasRepo(SgvtDB context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public marcas Create(marcas marca)
        {
            _context.marcas.Add(marca);
            _context.SaveChanges();
            return marca;
        }

        public marcas Delete(int id)
        {
            marcas marca = _context.marcas.Where(x => x.id_marca == id).FirstOrDefault();
            _context.marcas.Remove(marca);
            _context.SaveChanges();
            //_context.marcas.Attach(marca);
            //_context.Entry(marca).State = EntityState.Deleted;
            //_context.SaveChanges();
            return marca;
        }

        public marcas Edit(marcas marca)
        {
            _context.marcas.Attach(marca);
            _context.Entry(marca).State = EntityState.Modified;
            _context.SaveChanges();
            return marca;
        }


        private List<marcas> DoSort(List<marcas> items, string SortProperty, SortOrder sortOrder)
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

        public PaginatedList<marcas> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<marcas> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.marcas.Where(n => n.nombre.Contains(SearchText) || n.descripcion.Contains(SearchText))
                    .ToList();
            }
            else
                items = _context.marcas.ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<marcas> retItems = new PaginatedList<marcas>(items, pageIndex, pageSize);

            return retItems;
        }

        public marcas GetItem(int id)
        {
            marcas item = _context.marcas.Where(u => u.id_marca == id).FirstOrDefault();
            return item;
        }
        public bool IsBrandNameExists(string name)
        {
            int ct = _context.marcas.Where(n => n.nombre.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsBrandNameExists(string name, int id)
        {
            int ct = _context.marcas.Where(n => n.nombre.ToLower() == name.ToLower() && n.id_marca != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
    }
}
