using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class InitialDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "clientes",
                columns: table => new
                {
                    id_cliente = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    numero_documento = table.Column<string>(maxLength: 13, nullable: false),
                    nombres = table.Column<string>(maxLength: 64, nullable: false),
                    apellidos = table.Column<string>(maxLength: 64, nullable: false),
                    fecha_registro = table.Column<DateTime>(nullable: false),
                    genero = table.Column<string>(maxLength: 16, nullable: false),
                    direccion = table.Column<string>(maxLength: 128, nullable: false),
                    email = table.Column<string>(maxLength: 64, nullable: false),
                    password = table.Column<string>(maxLength: 20, nullable: false),
                    numero_contacto = table.Column<string>(maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clientes", x => x.id_cliente);
                });

            migrationBuilder.CreateTable(
                name: "empleados",
                columns: table => new
                {
                    id_empleado = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    numero_documento = table.Column<string>(maxLength: 13, nullable: false),
                    nombres = table.Column<string>(maxLength: 64, nullable: false),
                    apellidos = table.Column<string>(maxLength: 64, nullable: false),
                    fecha_registro = table.Column<DateTime>(nullable: false),
                    es_activo = table.Column<bool>(nullable: false),
                    email = table.Column<string>(maxLength: 64, nullable: false),
                    telefono = table.Column<string>(maxLength: 20, nullable: false),
                    password = table.Column<string>(maxLength: 20, nullable: false),
                    perfil = table.Column<string>(maxLength: 3, nullable: false),
                    cargo = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_empleados", x => x.id_empleado);
                });

            migrationBuilder.CreateTable(
                name: "visitas",
                columns: table => new
                {
                    id_visita = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    fecha_agendada = table.Column<DateTime>(nullable: false),
                    fecha_cierre = table.Column<DateTime>(nullable: false),
                    requerimiento = table.Column<string>(maxLength: 120, nullable: true),
                    descripcion_req = table.Column<string>(nullable: true),
                    tiempo_entrega = table.Column<string>(maxLength: 90, nullable: true),
                    ubicacion_disp_seguridad = table.Column<string>(maxLength: 120, nullable: true),
                    requiere_instalacion = table.Column<bool>(nullable: false),
                    requiere_mantenimiento = table.Column<bool>(nullable: false),
                    descripcion_problema = table.Column<string>(nullable: true),
                    estado = table.Column<string>(maxLength: 50, nullable: true),
                    empleado_id = table.Column<int>(nullable: false),
                    cliente_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_visitas", x => x.id_visita);
                    table.ForeignKey(
                        name: "FK_visitas_clientes_cliente_id",
                        column: x => x.cliente_id,
                        principalTable: "clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_visitas_empleados_empleado_id",
                        column: x => x.empleado_id,
                        principalTable: "empleados",
                        principalColumn: "id_empleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_visitas_cliente_id",
                table: "visitas",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_visitas_empleado_id",
                table: "visitas",
                column: "empleado_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "visitas");

            migrationBuilder.DropTable(
                name: "clientes");

            migrationBuilder.DropTable(
                name: "empleados");
        }
    }
}
