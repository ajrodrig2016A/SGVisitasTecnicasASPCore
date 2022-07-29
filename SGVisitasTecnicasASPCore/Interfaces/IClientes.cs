using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Interfaces
{
    public interface IClientes
    {
        PaginatedList<clientes> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);

        clientes GetItem(int id);
        clientes Create(clientes cliente);
        clientes Edit(clientes cliente);
        clientes Delete(int id);
        //clientes Delete(clientes cliente);
        public bool IsCustomerExists(string name);
        public bool IsCustomerExists(string name, int id_cliente);
    }
}
