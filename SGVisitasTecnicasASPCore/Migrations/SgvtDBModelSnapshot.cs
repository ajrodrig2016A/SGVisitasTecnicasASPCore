﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SGVisitasTecnicasASPCore.Models;

namespace SGVisitasTecnicasASPCore.Migrations
{
    [DbContext(typeof(SgvtDB))]
    partial class SgvtDBModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.Usuario", b =>
                {
                    b.Property<int>("id_usuario")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Clave")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Correo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Nombre")
                        .HasColumnType("text");

                    b.Property<string>("Rol")
                        .HasColumnType("text");

                    b.Property<string>("token_recovery")
                        .HasColumnType("text");

                    b.HasKey("id_usuario");

                    b.ToTable("usuarios");
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.categorias", b =>
                {
                    b.Property<int>("id_categoria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasMaxLength(2147483647);

                    b.Property<string>("estado")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("id_categoria");

                    b.ToTable("categorias");
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.clientes", b =>
                {
                    b.Property<int>("id_cliente")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("apellidos")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("direccion")
                        .IsRequired()
                        .HasColumnType("varchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<DateTime>("fecha_actualizacion")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("fecha_registro")
                        .HasColumnType("datetime");

                    b.Property<string>("genero")
                        .IsRequired()
                        .HasColumnType("varchar(16)")
                        .HasMaxLength(16);

                    b.Property<string>("nombres")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("numero_contacto")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("numero_documento")
                        .IsRequired()
                        .HasColumnType("varchar(13)")
                        .HasMaxLength(13);

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("id_cliente");

                    b.ToTable("clientes");
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.cotizaciones", b =>
                {
                    b.Property<int>("id_cotizacion")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("codigo")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("direccion_inmueble")
                        .IsRequired()
                        .HasColumnType("varchar(90)")
                        .HasMaxLength(90);

                    b.Property<string>("estado")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<DateTime>("fecha_actualizacion")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("fecha_registro")
                        .HasColumnType("datetime");

                    b.Property<string>("forma_pago")
                        .IsRequired()
                        .HasColumnType("varchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("garantia_equipos")
                        .IsRequired()
                        .HasColumnType("varchar(255)")
                        .HasMaxLength(255);

                    b.Property<int>("id_cliente")
                        .HasColumnType("int");

                    b.Property<int>("id_empleado")
                        .HasColumnType("int");

                    b.Property<string>("instalacion")
                        .IsRequired()
                        .HasColumnType("varchar(160)")
                        .HasMaxLength(160);

                    b.Property<string>("observaciones")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasMaxLength(2147483647);

                    b.Property<string>("sector_inmueble")
                        .IsRequired()
                        .HasColumnType("varchar(60)")
                        .HasMaxLength(60);

                    b.Property<string>("servicio")
                        .IsRequired()
                        .HasColumnType("varchar(100)")
                        .HasMaxLength(100);

                    b.Property<decimal>("subtotal")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("telefono")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("tiempo_entrega")
                        .IsRequired()
                        .HasColumnType("varchar(160)")
                        .HasMaxLength(160);

                    b.Property<string>("validez")
                        .IsRequired()
                        .HasColumnType("varchar(160)")
                        .HasMaxLength(160);

                    b.HasKey("id_cotizacion");

                    b.HasIndex("id_cliente");

                    b.HasIndex("id_empleado");

                    b.ToTable("cotizaciones");
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.detalles_cotizacion", b =>
                {
                    b.Property<int>("id_detalle_cotización")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("cantidad")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("descuento")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("id_cotizacion")
                        .HasColumnType("int");

                    b.Property<int>("id_producto")
                        .HasColumnType("int");

                    b.Property<string>("ubicacion")
                        .IsRequired()
                        .HasColumnType("varchar(90)")
                        .HasMaxLength(90);

                    b.Property<decimal>("valorTotal")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("valorUnitario")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("id_detalle_cotización");

                    b.HasIndex("id_cotizacion");

                    b.HasIndex("id_producto");

                    b.ToTable("detallesCotizacion");
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.detalles_venta", b =>
                {
                    b.Property<int>("id_detalle_venta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("cantidad")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("descuento")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<int>("id_producto")
                        .HasColumnType("int");

                    b.Property<int>("id_venta")
                        .HasColumnType("int");

                    b.Property<decimal>("valorTotal")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("valorUnitario")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("id_detalle_venta");

                    b.HasIndex("id_producto");

                    b.HasIndex("id_venta");

                    b.ToTable("detallesVenta");
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.empleados", b =>
                {
                    b.Property<int>("id_empleado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("apellidos")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("cargo")
                        .IsRequired()
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32);

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<bool>("es_activo")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime>("fecha_actualizacion")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("fecha_registro")
                        .HasColumnType("datetime");

                    b.Property<string>("nombres")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.Property<string>("numero_documento")
                        .IsRequired()
                        .HasColumnType("varchar(13)")
                        .HasMaxLength(13);

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<string>("perfil")
                        .IsRequired()
                        .HasColumnType("varchar(3)")
                        .HasMaxLength(3);

                    b.Property<string>("telefono")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.HasKey("id_empleado");

                    b.ToTable("empleados");
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.marcas", b =>
                {
                    b.Property<int>("id_marca")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("varchar(192)")
                        .HasMaxLength(192);

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("id_marca");

                    b.ToTable("marcas");
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.productos", b =>
                {
                    b.Property<int>("id_producto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ImageName")
                        .HasColumnType("text");

                    b.Property<decimal>("cantidad")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasMaxLength(2147483647);

                    b.Property<DateTime>("fecha_actualizacion")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("fecha_registro")
                        .HasColumnType("datetime");

                    b.Property<int>("id_categoria")
                        .HasColumnType("int");

                    b.Property<int>("id_marca")
                        .HasColumnType("int");

                    b.Property<int>("id_unidad")
                        .HasColumnType("int");

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("varchar(90)")
                        .HasMaxLength(90);

                    b.Property<decimal>("precioUnitario")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("id_producto");

                    b.HasIndex("id_categoria");

                    b.HasIndex("id_marca");

                    b.HasIndex("id_unidad");

                    b.ToTable("productos");
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.unidades", b =>
                {
                    b.Property<int>("id_unidad")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("varchar(192)")
                        .HasMaxLength(192);

                    b.Property<string>("nombre")
                        .IsRequired()
                        .HasColumnType("varchar(64)")
                        .HasMaxLength(64);

                    b.HasKey("id_unidad");

                    b.ToTable("unidades");
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.ventas", b =>
                {
                    b.Property<int>("id_venta")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<decimal>("Iva")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("codigo_venta")
                        .IsRequired()
                        .HasColumnType("varchar(10)")
                        .HasMaxLength(10);

                    b.Property<string>("estado")
                        .IsRequired()
                        .HasColumnType("varchar(20)")
                        .HasMaxLength(20);

                    b.Property<DateTime>("fecha_cierre")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("fecha_creacion")
                        .HasColumnType("datetime");

                    b.Property<int>("id_cliente")
                        .HasColumnType("int");

                    b.Property<string>("numero_factura")
                        .IsRequired()
                        .HasColumnType("varchar(192)")
                        .HasMaxLength(192);

                    b.Property<string>("observaciones")
                        .HasColumnType("text")
                        .HasMaxLength(2147483647);

                    b.Property<decimal>("subtotal")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal>("total")
                        .HasColumnType("decimal(18, 2)");

                    b.HasKey("id_venta");

                    b.HasIndex("id_cliente");

                    b.ToTable("ventas");
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.visitas", b =>
                {
                    b.Property<int>("id_visita")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("descripcion")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasMaxLength(2147483647);

                    b.Property<bool>("esProblema")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("estado")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("fecha_agendada")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("fecha_cierre")
                        .HasColumnType("datetime");

                    b.Property<int>("id_cliente")
                        .HasColumnType("int");

                    b.Property<int>("id_empleado")
                        .HasColumnType("int");

                    b.Property<string>("observaciones")
                        .HasColumnType("text")
                        .HasMaxLength(2147483647);

                    b.Property<bool>("requiereInstalacion")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("requiereMantenimiento")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("tiempoEntrega")
                        .IsRequired()
                        .HasColumnType("varchar(90)")
                        .HasMaxLength(90);

                    b.Property<string>("ubicacionDispSeguridad")
                        .IsRequired()
                        .HasColumnType("varchar(120)")
                        .HasMaxLength(120);

                    b.HasKey("id_visita");

                    b.HasIndex("id_cliente");

                    b.HasIndex("id_empleado");

                    b.ToTable("visitas");
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.cotizaciones", b =>
                {
                    b.HasOne("SGVisitasTecnicasASPCore.Models.clientes", "Cliente")
                        .WithMany()
                        .HasForeignKey("id_cliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SGVisitasTecnicasASPCore.Models.empleados", "Empleado")
                        .WithMany()
                        .HasForeignKey("id_empleado")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.detalles_cotizacion", b =>
                {
                    b.HasOne("SGVisitasTecnicasASPCore.Models.cotizaciones", "Cotizacion")
                        .WithMany("DetallesCotizacion")
                        .HasForeignKey("id_cotizacion")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SGVisitasTecnicasASPCore.Models.productos", "Producto")
                        .WithMany()
                        .HasForeignKey("id_producto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.detalles_venta", b =>
                {
                    b.HasOne("SGVisitasTecnicasASPCore.Models.productos", "Producto")
                        .WithMany()
                        .HasForeignKey("id_producto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SGVisitasTecnicasASPCore.Models.ventas", "Venta")
                        .WithMany("DetallesVenta")
                        .HasForeignKey("id_venta")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.productos", b =>
                {
                    b.HasOne("SGVisitasTecnicasASPCore.Models.categorias", "Categoria")
                        .WithMany()
                        .HasForeignKey("id_categoria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SGVisitasTecnicasASPCore.Models.marcas", "Marca")
                        .WithMany()
                        .HasForeignKey("id_marca")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SGVisitasTecnicasASPCore.Models.unidades", "Unidad")
                        .WithMany()
                        .HasForeignKey("id_unidad")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.ventas", b =>
                {
                    b.HasOne("SGVisitasTecnicasASPCore.Models.clientes", "Cliente")
                        .WithMany()
                        .HasForeignKey("id_cliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.visitas", b =>
                {
                    b.HasOne("SGVisitasTecnicasASPCore.Models.clientes", "Cliente")
                        .WithMany()
                        .HasForeignKey("id_cliente")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SGVisitasTecnicasASPCore.Models.empleados", "Empleado")
                        .WithMany()
                        .HasForeignKey("id_empleado")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
