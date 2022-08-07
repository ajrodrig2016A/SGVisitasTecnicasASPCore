using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Interfaces
{
    public interface IVisitas
    {
        PaginatedList<visitas> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);

        visitas GetItem(int id);
        visitas Create(visitas visita);
        visitas Edit(visitas visita);
        visitas Delete(int id);
        //visitas Delete(visitas visita);
        public bool IsItemExists(string name);
        public bool IsItemExists(string name, int id);

        public bool IsItemCodeExists(int itemCode);
        public bool IsItemCodeExists(int itemCode, string name);
    }
}
