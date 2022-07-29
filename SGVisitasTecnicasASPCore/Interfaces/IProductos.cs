using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Interfaces
{
    public interface IProductos
    {
        PaginatedList<productos> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);

        productos GetItem(int id);
        productos Create(productos producto);
        productos Edit(productos producto);
        productos Delete(int id);
        //productos Delete(productos producto);
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int id);

        public bool IsItemCodeExists(int itemCode);
        public bool IsItemCodeExists(int itemCode, string name);
    }
}
