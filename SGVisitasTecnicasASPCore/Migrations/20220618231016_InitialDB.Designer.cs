﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SGVisitasTecnicasASPCore.Models;

namespace SGVisitasTecnicasASPCore.Migrations
{
    [DbContext(typeof(SgvtDB))]
    [Migration("20220618231016_InitialDB")]
    partial class InitialDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

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

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.visitas", b =>
                {
                    b.Property<int>("id_visita")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("cliente_id")
                        .HasColumnType("int");

                    b.Property<string>("descripcion_problema")
                        .HasColumnType("text");

                    b.Property<string>("descripcion_req")
                        .HasColumnType("text");

                    b.Property<int>("empleado_id")
                        .HasColumnType("int");

                    b.Property<string>("estado")
                        .HasColumnType("varchar(50)")
                        .HasMaxLength(50);

                    b.Property<DateTime>("fecha_agendada")
                        .HasColumnType("datetime");

                    b.Property<DateTime>("fecha_cierre")
                        .HasColumnType("datetime");

                    b.Property<string>("requerimiento")
                        .HasColumnType("varchar(120)")
                        .HasMaxLength(120);

                    b.Property<bool>("requiere_instalacion")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("requiere_mantenimiento")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("tiempo_entrega")
                        .HasColumnType("varchar(90)")
                        .HasMaxLength(90);

                    b.Property<string>("ubicacion_disp_seguridad")
                        .HasColumnType("varchar(120)")
                        .HasMaxLength(120);

                    b.HasKey("id_visita");

                    b.HasIndex("cliente_id");

                    b.HasIndex("empleado_id");

                    b.ToTable("visitas");
                });

            modelBuilder.Entity("SGVisitasTecnicasASPCore.Models.visitas", b =>
                {
                    b.HasOne("SGVisitasTecnicasASPCore.Models.clientes", "Cliente")
                        .WithMany("Visitas")
                        .HasForeignKey("cliente_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SGVisitasTecnicasASPCore.Models.empleados", "Empleado")
                        .WithMany("Visitas")
                        .HasForeignKey("empleado_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
