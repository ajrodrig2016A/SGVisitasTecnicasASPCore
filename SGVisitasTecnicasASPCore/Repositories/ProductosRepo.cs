using Microsoft.EntityFrameworkCore;
using SGVisitasTecnicasASPCore.Interfaces;
using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Repositories
{
    public class ProductosRepo:IProductos
    {
        private readonly SgvtDB _context; // for connecting to efcore.
        public ProductosRepo(SgvtDB context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public productos Create(productos producto)
        {
            _context.productos.Add(producto);
            _context.SaveChanges();
            return producto;
        }

        public productos Delete(int id)
        {
            productos producto = _context.productos.Where(x => x.id_producto == id).FirstOrDefault();
            _context.productos.Remove(producto);
            _context.SaveChanges();
            //_context.productos.Attach(producto);
            //_context.Entry(producto).State = EntityState.Deleted;
            //_context.SaveChanges();
            return producto;
        }

        public productos Edit(productos producto)
        {
            _context.productos.Attach(producto);
            _context.Entry(producto).State = EntityState.Modified;
            _context.SaveChanges();
            return producto;
        }


        private List<productos> DoSort(List<productos> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "nombre")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.nombre).ToList();
                else
                    items = items.OrderByDescending(n => n.nombre).ToList();
            }
            else //(SortProperty.ToLower() == "descripcion")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.descripcion).ToList();
                else
                    items = items.OrderByDescending(d => d.descripcion).ToList();
            }
            //else
            //{
            //    if (sortOrder == SortOrder.Ascending)
            //        items = items.OrderBy(m => m.marca).ToList();
            //    else
            //        items = items.OrderByDescending(m => m.marca).ToList();
            //}

            return items;
        }

        public PaginatedList<productos> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<productos> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.productos.Where(n => n.nombre.Contains(SearchText) || n.descripcion.Contains(SearchText))
                    .ToList();
            }
            else
                items = _context.productos.Include(m => m.Marca).Include(u => u.Unidad).ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<productos> retItems = new PaginatedList<productos>(items, pageIndex, pageSize);

            return retItems;
        }

        public productos GetItem(int id)
        {
            productos item = _context.productos.Where(u => u.id_producto == id).FirstOrDefault();
            return item;
        }
        public bool IsItemExists(string name)
        {
            int ct = _context.productos.Where(n => n.nombre.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int id)
        {
            int ct = _context.productos.Where(n => n.nombre.ToLower() == name.ToLower() && n.id_producto != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemCodeExists(int itemCode)
        {
            int ct = _context.productos.Where(n => n.id_producto == itemCode).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsItemCodeExists(int itemCode, string name)
        {
            if (name == "")
                return IsItemCodeExists(itemCode);
            var strName = _context.productos.Where(n => n.id_producto == itemCode).Max(nm => nm.nombre);
            if (strName == null || strName == name)
                return false;
            else
                return IsItemExists(name);
        }

    }
}
