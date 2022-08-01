using Microsoft.EntityFrameworkCore.Migrations;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class CreateFK_detalles_cotizacion_productos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "descripcion",
                table: "detallesCotizacion");

            migrationBuilder.DropColumn(
                name: "marca",
                table: "detallesCotizacion");

            migrationBuilder.DropColumn(
                name: "unidad",
                table: "detallesCotizacion");

            migrationBuilder.AddColumn<int>(
                name: "id_producto",
                table: "detallesCotizacion",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_detallesCotizacion_id_producto",
                table: "detallesCotizacion",
                column: "id_producto");

            migrationBuilder.AddForeignKey(
                name: "FK_detallesCotizacion_productos_id_producto",
                table: "detallesCotizacion",
                column: "id_producto",
                principalTable: "productos",
                principalColumn: "id_producto",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detallesCotizacion_productos_id_producto",
                table: "detallesCotizacion");

            migrationBuilder.DropIndex(
                name: "IX_detallesCotizacion_id_producto",
                table: "detallesCotizacion");

            migrationBuilder.DropColumn(
                name: "id_producto",
                table: "detallesCotizacion");

            migrationBuilder.AddColumn<string>(
                name: "descripcion",
                table: "detallesCotizacion",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "marca",
                table: "detallesCotizacion",
                type: "varchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "unidad",
                table: "detallesCotizacion",
                type: "varchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");
        }
    }
}
