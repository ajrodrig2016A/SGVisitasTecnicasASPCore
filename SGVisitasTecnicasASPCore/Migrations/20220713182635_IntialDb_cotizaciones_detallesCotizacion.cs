using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class IntialDb_cotizaciones_detallesCotizacion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cotizaciones",
                columns: table => new
                {
                    id_cotizacion = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nombre_cliente = table.Column<string>(maxLength: 60, nullable: false),
                    sector_inmueble = table.Column<string>(maxLength: 60, nullable: false),
                    direccion_inmueble = table.Column<string>(maxLength: 90, nullable: false),
                    fecha_registro = table.Column<DateTime>(nullable: false),
                    telefono = table.Column<string>(maxLength: 20, nullable: false),
                    estado = table.Column<string>(maxLength: 20, nullable: false),
                    tiempo_entrega = table.Column<string>(maxLength: 160, nullable: false),
                    instalacion = table.Column<string>(maxLength: 160, nullable: false),
                    garantia_equipos = table.Column<string>(maxLength: 255, nullable: false),
                    forma_pago = table.Column<string>(maxLength: 60, nullable: false),
                    subtotal = table.Column<decimal>(nullable: false),
                    validez = table.Column<string>(maxLength: 160, nullable: false),
                    observaciones = table.Column<string>(maxLength: 2147483647, nullable: false),
                    id_empleado = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cotizaciones", x => x.id_cotizacion);
                    table.ForeignKey(
                        name: "FK_cotizaciones_empleados_id_empleado",
                        column: x => x.id_empleado,
                        principalTable: "empleados",
                        principalColumn: "id_empleado",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detallesCotizacion",
                columns: table => new
                {
                    id_detalle_cotización = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    id_cotizacion = table.Column<int>(nullable: true),
                    id_producto = table.Column<int>(nullable: true),
                    descripcion = table.Column<string>(maxLength: 2147483647, nullable: false),
                    ubicación = table.Column<string>(maxLength: 90, nullable: false),
                    marca = table.Column<string>(maxLength: 60, nullable: false),
                    unidad = table.Column<string>(maxLength: 8, nullable: false),
                    cantidad = table.Column<decimal>(nullable: false),
                    valorUnitario = table.Column<decimal>(nullable: false),
                    valorTotal = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detallesCotizacion", x => x.id_detalle_cotización);
                    table.ForeignKey(
                        name: "FK_detallesCotizacion_cotizaciones_id_cotizacion",
                        column: x => x.id_cotizacion,
                        principalTable: "cotizaciones",
                        principalColumn: "id_cotizacion",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_detallesCotizacion_productos_id_producto",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cotizaciones_id_empleado",
                table: "cotizaciones",
                column: "id_empleado");

            migrationBuilder.CreateIndex(
                name: "IX_detallesCotizacion_id_cotizacion",
                table: "detallesCotizacion",
                column: "id_cotizacion");

            migrationBuilder.CreateIndex(
                name: "IX_detallesCotizacion_id_producto",
                table: "detallesCotizacion",
                column: "id_producto");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "detallesCotizacion");

            migrationBuilder.DropTable(
                name: "cotizaciones");
        }
    }
}
