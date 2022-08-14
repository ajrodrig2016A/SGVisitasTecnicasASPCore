using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Interfaces
{
    public interface IVentas
    {
        PaginatedList<ventas> GetQuotes(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);

        ventas GetQuote(int id);
        ventas Create(ventas cotizacion);
        ventas Edit(ventas cotizacion);
        bool Delete(int id);
        //ventas Delete(ventas cotizacion);
        public bool IsQuoteExists(string name);
        public bool IsQuoteExists(string name, int id);

        public bool IsQuoteCodeExists(int itemCode);
        public bool IsQuoteCodeExists(int itemCode, string name);

        public string GetNewSaleNumber();
        public string GetNewInvoiceNumber();
    }
}
