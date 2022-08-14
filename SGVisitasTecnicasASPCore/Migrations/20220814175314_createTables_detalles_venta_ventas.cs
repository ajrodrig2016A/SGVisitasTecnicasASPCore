using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class createTables_detalles_venta_ventas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "descuento",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "porcentaje",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "stock",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "ubicación",
                table: "detallesCotizacion");

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_actualizacion",
                table: "productos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_registro",
                table: "productos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_actualizacion",
                table: "empleados",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "descuento",
                table: "detallesCotizacion",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ubicacion",
                table: "detallesCotizacion",
                maxLength: 90,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_actualizacion",
                table: "cotizaciones",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "servicio",
                table: "cotizaciones",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "fecha_actualizacion",
                table: "clientes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ventas",
                columns: table => new
                {
                    id_venta = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    codigo_venta = table.Column<string>(maxLength: 10, nullable: false),
                    numero_factura = table.Column<string>(maxLength: 192, nullable: false),
                    fecha_creacion = table.Column<DateTime>(nullable: false),
                    fecha_cierre = table.Column<DateTime>(nullable: false),
                    estado = table.Column<string>(maxLength: 20, nullable: false),
                    subtotal = table.Column<decimal>(nullable: false),
                    Iva = table.Column<decimal>(nullable: false),
                    total = table.Column<decimal>(nullable: false),
                    id_cliente = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ventas", x => x.id_venta);
                    table.ForeignKey(
                        name: "FK_ventas_clientes_id_cliente",
                        column: x => x.id_cliente,
                        principalTable: "clientes",
                        principalColumn: "id_cliente",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "detallesVenta",
                columns: table => new
                {
                    id_detalle_venta = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    id_venta = table.Column<int>(nullable: false),
                    id_producto = table.Column<int>(nullable: false),
                    cantidad = table.Column<decimal>(nullable: false),
                    valorUnitario = table.Column<decimal>(nullable: false),
                    descuento = table.Column<decimal>(nullable: false),
                    valorTotal = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_detallesVenta", x => x.id_detalle_venta);
                    table.ForeignKey(
                        name: "FK_detallesVenta_productos_id_producto",
                        column: x => x.id_producto,
                        principalTable: "productos",
                        principalColumn: "id_producto",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_detallesVenta_ventas_id_venta",
                        column: x => x.id_venta,
                        principalTable: "ventas",
                        principalColumn: "id_venta",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_detallesVenta_id_producto",
                table: "detallesVenta",
                column: "id_producto");

            migrationBuilder.CreateIndex(
                name: "IX_detallesVenta_id_venta",
                table: "detallesVenta",
                column: "id_venta");

            migrationBuilder.CreateIndex(
                name: "IX_ventas_id_cliente",
                table: "ventas",
                column: "id_cliente");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "detallesVenta");

            migrationBuilder.DropTable(
                name: "ventas");

            migrationBuilder.DropColumn(
                name: "fecha_actualizacion",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "fecha_registro",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "fecha_actualizacion",
                table: "empleados");

            migrationBuilder.DropColumn(
                name: "descuento",
                table: "detallesCotizacion");

            migrationBuilder.DropColumn(
                name: "ubicacion",
                table: "detallesCotizacion");

            migrationBuilder.DropColumn(
                name: "fecha_actualizacion",
                table: "cotizaciones");

            migrationBuilder.DropColumn(
                name: "servicio",
                table: "cotizaciones");

            migrationBuilder.DropColumn(
                name: "fecha_actualizacion",
                table: "clientes");

            migrationBuilder.AddColumn<decimal>(
                name: "descuento",
                table: "productos",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "porcentaje",
                table: "productos",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "stock",
                table: "productos",
                type: "decimal(18, 2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ubicación",
                table: "detallesCotizacion",
                type: "varchar(90)",
                maxLength: 90,
                nullable: false,
                defaultValue: "");
        }
    }
}
