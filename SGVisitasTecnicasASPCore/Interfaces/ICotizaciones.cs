using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Interfaces
{
    public interface ICotizaciones
    {
        PaginatedList<cotizaciones> GetQuotes(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);

        cotizaciones GetQuote(int id);
        cotizaciones Create(cotizaciones cotizacion);
        cotizaciones Edit(cotizaciones cotizacion);
        cotizaciones Delete(int id);
        //cotizaciones Delete(cotizaciones cotizacion);
        public bool IsQuoteExists(string name);
        public bool IsQuoteExists(string name, int id);

        public bool IsQuoteCodeExists(int itemCode);
        public bool IsQuoteCodeExists(int itemCode, string name);
    }
}
