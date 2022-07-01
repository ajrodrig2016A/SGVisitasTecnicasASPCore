using System.Collections.Generic;
using System.Linq;
using SGVisitasTecnicasASPCore.Models;

namespace SGVisitasTecnicasASPCore.Data
{
    public class DA_Logica
    {
        private readonly SgvtDB _context;
        public DA_Logica(SgvtDB context)
        {
            _context = context;
        }

        public List<Usuario> ListaUsuario()
        {
            List<Usuario> usuariosLst = new List<Usuario>();
            List<empleados> empLst = new List<empleados>();
            List<clientes> cliLst = new List<clientes>();

            empLst = _context.empleados.ToList();
            cliLst = _context.clientes.ToList();

            foreach (var item in empLst)
            {
                usuariosLst.Add(new Usuario { Nombre = item.nombres, Correo = item.email, Clave = item.password, Roles = new string[] { item.perfil } });
            }

            foreach (var item in cliLst)
            {
                usuariosLst.Add(new Usuario { Nombre = item.nombres, Correo = item.email, Clave = item.password, Roles = new string[] { "CLI" } });
            }

            return usuariosLst;

        }

        public Usuario ValidarUsuario(string _correo, string _clave)
        {
            return ListaUsuario().Where(item => item.Correo == _correo && item.Clave == _clave).FirstOrDefault();
        }
    }
}
