using SGVisitasTecnicasASPCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGVisitasTecnicasASPCore.Interfaces
{
    public interface IEmpleados
    {
        PaginatedList<empleados> GetItems(string SortProperty, SortOrder sortOrder, string SearchText = "", int pageIndex = 1, int pageSize = 5);

        empleados GetItem(int id);
        empleados Create(empleados empleado);
        empleados Edit(empleados empleado);
        empleados Delete(int id);
        //empleados Delete(empleados empleado);
        public bool IsEmployeeExists(string name);
        public bool IsEmployeeExists(string name, int id_empleado);
    }
}
