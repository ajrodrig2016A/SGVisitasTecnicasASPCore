using Microsoft.EntityFrameworkCore;
using SGVisitasTecnicasASPCore.Interfaces;
using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Repositories
{
    public class VisitasRepo:IVisitas
    {
        private readonly SgvtDB _context; // for connecting to efcore.
        public VisitasRepo(SgvtDB context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public visitas Create(visitas visita)
        {
            _context.visitas.Add(visita);
            _context.SaveChanges();
            return visita;
        }

        public visitas Delete(int id)
        {
            visitas visita = _context.visitas.Where(x => x.id_visita == id).FirstOrDefault();
            _context.visitas.Remove(visita);
            _context.SaveChanges();
            //_context.visitas.Attach(visita);
            //_context.Entry(visita).State = EntityState.Deleted;
            //_context.SaveChanges();
            return visita;
        }

        public visitas Edit(visitas visita)
        {
            _context.visitas.Attach(visita);
            _context.Entry(visita).State = EntityState.Modified;
            _context.SaveChanges();
            return visita;
        }


        private List<visitas> DoSort(List<visitas> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "fecha agendada")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.fecha_agendada).ToList();
                else
                    items = items.OrderByDescending(n => n.fecha_agendada).ToList();
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
                    items = items.OrderBy(m => m.fecha_cierre).ToList();
                else
                    items = items.OrderByDescending(m => m.fecha_cierre).ToList();
            }

            return items;
        }

        public PaginatedList<visitas> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<visitas> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.visitas.Include(c => c.Cliente).Include(e => e.Empleado).Where(n => n.descripcion.Contains(SearchText) || n.ubicacionDispSeguridad.Contains(SearchText) || n.estado.Contains(SearchText) || n.Cliente.nombres.Contains(SearchText) || n.Empleado.nombres.Contains(SearchText))
                    .ToList();
            }
            else
                items = _context.visitas.Include(c => c.Cliente).Include(e => e.Empleado).ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<visitas> retItems = new PaginatedList<visitas>(items, pageIndex, pageSize);

            return retItems;
        }

        public visitas GetItem(int id)
        {
            visitas item = _context.visitas.Where(u => u.id_visita == id).Include(c => c.Cliente).Include(e => e.Empleado).FirstOrDefault();
            return item;
        }
        public bool IsItemExists(string name)
        {
            int ct = _context.visitas.Where(n => n.Cliente.nombres.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemExists(string name, int id)
        {
            int ct = _context.visitas.Where(n => n.Cliente.nombres.ToLower() == name.ToLower() && n.id_visita != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsItemCodeExists(int itemCode)
        {
            int ct = _context.visitas.Where(n => n.id_visita == itemCode).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsItemCodeExists(int itemCode, string name)
        {
            if (name == "")
                return IsItemCodeExists(itemCode);
            var strName = _context.visitas.Where(n => n.id_visita == itemCode).Max(nm => nm.Cliente.nombres);
            if (strName == null || strName == name)
                return false;
            else
                return IsItemExists(name);
        }

    }
}
