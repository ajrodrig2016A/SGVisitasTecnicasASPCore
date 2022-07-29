using Microsoft.EntityFrameworkCore.Migrations;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class IntialDb_rm_FK_detallescotizacion_productos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_producto",
                table: "detallesCotizacion",
                type: "int",
                nullable: true);

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
                onDelete: ReferentialAction.Restrict);
        }
    }
}
