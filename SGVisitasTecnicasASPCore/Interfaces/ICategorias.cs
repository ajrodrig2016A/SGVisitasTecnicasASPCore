using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Interfaces
{
    public interface ICategorias
    {
        PaginatedList<categorias> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);

        categorias GetItem(int id);
        categorias Create(categorias categoria);
        categorias Edit(categorias categoria);
        categorias Delete(int id);
        //categorias Delete(categorias categoria);
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int id_categoria);
    }
}
