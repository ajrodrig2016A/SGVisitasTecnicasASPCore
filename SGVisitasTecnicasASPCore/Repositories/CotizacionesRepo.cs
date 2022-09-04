using Microsoft.EntityFrameworkCore;
using SGVisitasTecnicasASPCore.Interfaces;
using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Repositories
{
    public class CotizacionesRepo:ICotizaciones
    {
        private readonly SgvtDB _context; // for connecting to efcore.
        public CotizacionesRepo(SgvtDB context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public cotizaciones Create(cotizaciones cotizacion)
        {
            try
            {
                _context.Add(cotizacion);
                _context.SaveChanges();

                if (cotizacion.DetallesCotizacion.Count == 0)
                {
                    cotizacion.DetallesCotizacion.Add(new detalles_cotizacion() { id_detalle_cotización = 1 });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //_context.cotizaciones.Add(cotizacion);
            //_context.SaveChanges();
            return cotizacion;
        }

        public bool Delete(int id)
        {
            bool retVal = false;
            cotizaciones cotizacion = new cotizaciones();
            try
            {
                cotizacion = _context.cotizaciones.Where(x => x.id_cotizacion == id).FirstOrDefault();
                _context.Attach(cotizacion);
                _context.Entry(cotizacion).State = EntityState.Deleted;
                _context.SaveChanges();
                retVal = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return retVal;
        }

        public cotizaciones Edit(cotizaciones cotizacion)
        {
            try
            {
                List<detalles_cotizacion> ctzDetails = _context.detallesCotizacion.Where(d => d.id_cotizacion == cotizacion.id_cotizacion).ToList();
                _context.detallesCotizacion.RemoveRange(ctzDetails);
                _context.SaveChanges();

                cotizacion.DetallesCotizacion.RemoveAll(n => n.cantidad == (decimal)0.00 || n.valorUnitario == (decimal)0.00);
                cotizacion.DetallesCotizacion.RemoveAll(t => t.valorTotal == (decimal)0.00);
                //cotizacion.DetallesCotizacion.RemoveAll(n => n.IsDeleted == true);

                _context.cotizaciones.Attach(cotizacion);
                _context.Entry(cotizacion).State = EntityState.Modified;
                _context.detallesCotizacion.AddRange(cotizacion.DetallesCotizacion);

                _context.SaveChanges();

                if (cotizacion.DetallesCotizacion.Count == 0)
                {
                    cotizacion.DetallesCotizacion.Add(new detalles_cotizacion() { id_detalle_cotización = 1 });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //_context.cotizaciones.Attach(cotizacion);
            //_context.Entry(cotizacion).State = EntityState.Modified;
            //_context.SaveChanges();
            return cotizacion;
        }


        private List<cotizaciones> DoSort(List<cotizaciones> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "código")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.codigo).ToList();
                else
                    items = items.OrderByDescending(n => n.codigo).ToList();
            }
            else if (SortProperty.ToLower() == "servicio")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.servicio).ToList();
                else
                    items = items.OrderByDescending(d => d.servicio).ToList();
            }
            else if (SortProperty.ToLower() == "sector del inmueble")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.sector_inmueble).ToList();
                else
                    items = items.OrderByDescending(d => d.sector_inmueble).ToList();
            }
            else if (SortProperty.ToLower() == "dirección del inmueble")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(d => d.direccion_inmueble).ToList();
                else
                    items = items.OrderByDescending(d => d.direccion_inmueble).ToList();
            }
            else
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(m => m.fecha_registro).ToList();
                else
                    items = items.OrderByDescending(m => m.fecha_registro).ToList();
            }

            return items;
        }

        public PaginatedList<cotizaciones> GetQuotes(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<cotizaciones> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.cotizaciones.Include(c => c.Cliente).Include(e => e.Empleado).Where(n => n.codigo.Contains(SearchText) || n.servicio.Contains(SearchText) || n.sector_inmueble.Contains(SearchText) || n.direccion_inmueble.Contains(SearchText) || n.Cliente.nombres.Contains(SearchText) || n.Empleado.nombres.Contains(SearchText) || n.telefono.Contains(SearchText) || n.estado.Contains(SearchText) || n.tiempo_entrega.Contains(SearchText)).ToList();
            }
            else
                items = _context.cotizaciones
                    .Include(c => c.Cliente)
                    .Include(e => e.Empleado)
                    .ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<cotizaciones> retQuotes = new PaginatedList<cotizaciones>(items, pageIndex, pageSize);

            return retQuotes;
        }

        public cotizaciones GetQuote(int id)
        {
            cotizaciones item = _context.cotizaciones.Where(u => u.id_cotizacion == id)
                .Include(d => d.DetallesCotizacion)
                .FirstOrDefault();
            return item;
        }
        public bool IsQuoteExists(string name)
        {
            int ct = _context.cotizaciones.Where(n => n.codigo.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsQuoteExists(string name, int id)
        {
            int ct = _context.cotizaciones.Where(n => n.codigo.ToLower() == name.ToLower() && n.id_cotizacion != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsQuoteCodeExists(int itemCode)
        {
            int ct = _context.cotizaciones.Where(n => n.id_cotizacion == itemCode).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsQuoteCodeExists(int itemCode, string name)
        {
            if (name == "")
                return IsQuoteCodeExists(itemCode);
            var strName = _context.cotizaciones.Where(n => n.id_cotizacion == itemCode).Max(nm => nm.codigo);
            if (strName == null || strName == name)
                return false;
            else
                return IsQuoteExists(name);
        }
        public string GetNewCTNumber()
        {
            string ctNumber = "";
            var LastCtNumber = _context.cotizaciones.Max(cd => cd.codigo);

            if (LastCtNumber == null)
                ctNumber = "CT00001";

            else
            {
                int lastDigit = 1;
                int.TryParse(LastCtNumber.Substring(2,5).ToString(), out lastDigit);

                ctNumber = "CT" + (lastDigit + 1).ToString().PadLeft(5, '0');
            }

            return ctNumber;
        }


    }
}
