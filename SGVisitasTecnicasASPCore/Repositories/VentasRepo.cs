using Microsoft.EntityFrameworkCore;
using SGVisitasTecnicasASPCore.Interfaces;
using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Repositories
{
    public class VentasRepo:IVentas
    {
        private readonly SgvtDB _context; // for connecting to efcore.
        public VentasRepo(SgvtDB context) // will be passed by dependency injection.
        {
            _context = context;
        }
        public ventas Create(ventas venta)
        {
            try
            {
                _context.Add(venta);
                _context.SaveChanges();

                if (venta.DetallesVenta.Count == 0)
                {
                    venta.DetallesVenta.Add(new detalles_venta() { id_detalle_venta = 1 });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //_context.ventas.Add(venta);
            //_context.SaveChanges();
            return venta;
        }

        public bool Delete(int id)
        {
            bool retVal = false;
            ventas venta = new ventas();
            try
            {
                venta = _context.ventas.Where(x => x.id_venta == id).FirstOrDefault();
                _context.Attach(venta);
                _context.Entry(venta).State = EntityState.Deleted;
                _context.SaveChanges();
                retVal = true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return retVal;
        }

        public ventas Edit(ventas venta)
        {
            try
            {
                List<detalles_venta> ctzDetails = _context.detallesVenta.Where(d => d.id_venta == venta.id_venta).ToList();
                _context.detallesVenta.RemoveRange(ctzDetails);
                _context.SaveChanges();

                venta.DetallesVenta.RemoveAll(n => n.cantidad == (decimal)0.00 || n.valorUnitario == (decimal)0.00);
                venta.DetallesVenta.RemoveAll(t => t.valorTotal == (decimal)0.00);
                //venta.DetallesVenta.RemoveAll(n => n.IsDeleted == true);

                _context.ventas.Attach(venta);
                _context.Entry(venta).State = EntityState.Modified;
                _context.detallesVenta.AddRange(venta.DetallesVenta);

                _context.SaveChanges();

                //ventas ctz = new ventas();
                if (venta.DetallesVenta.Count == 0)
                {
                    venta.DetallesVenta.Add(new detalles_venta() { id_detalle_venta = 1 });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //_context.ventas.Attach(venta);
            //_context.Entry(venta).State = EntityState.Modified;
            //_context.SaveChanges();
            return venta;
        }


        private List<ventas> DoSort(List<ventas> items, string SortProperty, SortOrder sortOrder)
        {

            if (SortProperty.ToLower() == "número de factura")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.numero_factura).ToList();
                else
                    items = items.OrderByDescending(n => n.numero_factura).ToList();
            }
            else if (SortProperty.ToLower() == "código de venta")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(n => n.codigo_venta).ToList();
                else
                    items = items.OrderByDescending(n => n.codigo_venta).ToList();
            }
            else if (SortProperty.ToLower() == "fecha de creación")
            {
                if (sortOrder == SortOrder.Ascending)
                    items = items.OrderBy(m => m.fecha_creacion).ToList();
                else
                    items = items.OrderByDescending(m => m.fecha_creacion).ToList();
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

        public PaginatedList<ventas> GetQuotes(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5)
        {
            List<ventas> items;

            if (SearchText != "" && SearchText != null)
            {
                items = _context.ventas.Include(c => c.Cliente).Where(n => n.codigo_venta.Contains(SearchText) || n.numero_factura.Contains(SearchText) || n.estado.Contains(SearchText) || n.Cliente.nombres.Contains(SearchText))
                .ToList();

            }
            else
                items = _context.ventas
                    .Include(c => c.Cliente)
                    .ToList();

            items = DoSort(items, SortProperty, sortOrder);

            PaginatedList<ventas> retQuotes = new PaginatedList<ventas>(items, pageIndex, pageSize);

            return retQuotes;
        }

        public ventas GetQuote(int id)
        {
            ventas item = _context.ventas.Where(u => u.id_venta == id)
                .Include(d => d.DetallesVenta)
                .FirstOrDefault();
            return item;
        }
        public bool IsQuoteExists(string name)
        {
            int ct = _context.ventas.Where(n => n.codigo_venta.ToLower() == name.ToLower()).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsQuoteExists(string name, int id)
        {
            int ct = _context.ventas.Where(n => n.codigo_venta.ToLower() == name.ToLower() && n.id_venta != id).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }

        public bool IsQuoteCodeExists(int itemCode)
        {
            int ct = _context.ventas.Where(n => n.id_venta == itemCode).Count();
            if (ct > 0)
                return true;
            else
                return false;
        }
        public bool IsQuoteCodeExists(int itemCode, string name)
        {
            if (name == "")
                return IsQuoteCodeExists(itemCode);
            var strName = _context.ventas.Where(n => n.id_venta == itemCode).Max(nm => nm.codigo_venta);
            if (strName == null || strName == name)
                return false;
            else
                return IsQuoteExists(name);
        }
        public string GetNewSaleNumber()
        {
            string slNumber = "";
            var LastSlNumber = _context.ventas.Max(cd => cd.codigo_venta);

            if (LastSlNumber == null)
                slNumber = "VT00001";

            else
            {
                int lastDigit = 1;
                int.TryParse(LastSlNumber.Substring(2,5).ToString(), out lastDigit);

                slNumber = "VT" + (lastDigit + 1).ToString().PadLeft(5, '0');
            }

            return slNumber;
        }

        public string GetNewInvoiceNumber()
        {
            string invNumber = "";
            var LastInvNumber = _context.ventas.Max(cd => cd.numero_factura);

            if (LastInvNumber == null)
                invNumber = "FAC0000000001";

            else
            {
                int lastDigit = 1;
                int.TryParse(LastInvNumber.Substring(3, 10).ToString(), out lastDigit);

                invNumber = "FAC" + (lastDigit + 1).ToString().PadLeft(10, '0');
            }

            return invNumber;
        }


    }
}
