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
        public DbSet<visitas> visitas { get; set; }
        public DbSet<categorias> categorias { get; set; }
        public DbSet<productos> productos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // configures one-to-many relationship
            modelBuilder.Entity<visitas>()
                .HasOne(s => s.Empleado)
                .WithMany(g => g.Visitas)
                .HasForeignKey(s => s.empleado_id);

            modelBuilder.Entity<visitas>()
                .HasOne(s => s.Cliente)
                .WithMany(g => g.Visitas)
                .HasForeignKey(s => s.cliente_id);

            // configures one-to-many relationship
            modelBuilder.Entity<productos>()
                .HasOne(s => s.Categoria)
                .WithMany(g => g.Productos)
                .HasForeignKey(s => s.categoria_id);
        }
    }

}
