using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Interfaces
{
    public interface IUnidades
    {
        PaginatedList<unidades> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);

        unidades GetItem(int id);
        unidades Create(unidades unidad);
        unidades Edit(unidades unidad);
        unidades Delete(int id);
        //unidades Delete(unidades unidad);
        public bool IsUnitNameExists(string name);
        public bool IsUnitNameExists(string name, int id);
    }
}
