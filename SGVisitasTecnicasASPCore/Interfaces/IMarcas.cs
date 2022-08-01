using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Interfaces
{
    public interface IMarcas
    {
        PaginatedList<marcas> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);

        marcas GetItem(int id);
        marcas Create(marcas marca);
        marcas Edit(marcas marca);
        marcas Delete(int id);
        //marcas Delete(marcas marca);
        public bool IsBrandNameExists(string name);
        public bool IsBrandNameExists(string name, int id);
    }
}
