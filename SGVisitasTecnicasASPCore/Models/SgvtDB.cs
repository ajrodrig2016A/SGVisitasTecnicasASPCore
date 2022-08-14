using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MySql.EntityFrameworkCore.Extensions;

namespace SGVisitasTecnicasASPCore.Models
{

    public class SgvtDB: DbContext
    {
        public SgvtDB(DbContextOptions<SgvtDB> options) : base(options)
        { }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseMySQL("server=localhost;database=sgvt_db;User ID=root;password=UIsrael2022A;");
        //}
        
        public DbSet<empleados> empleados { get; set; }
        public DbSet<clientes> clientes { get; set; }
        public DbSet<Usuario> usuarios { get; set; }
        public DbSet<visitas> visitas { get; set; }
        public DbSet<categorias> categorias { get; set; }
        public DbSet<productos> productos { get; set; }

        public DbSet<cotizaciones> cotizaciones { get; set; }
        public DbSet<detalles_cotizacion> detallesCotizacion { get; set; }

        public DbSet<ventas> ventas { get; set; }
        public DbSet<detalles_venta> detallesVenta { get; set; }

        public DbSet<unidades> unidades { get; set; }
        public DbSet<marcas> marcas { get; set; }

    }

}
