using Microsoft.EntityFrameworkCore.Migrations;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class IntialDb_update_FK_detallescotizacion_cotizaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detallesCotizacion_cotizaciones_id_cotizacion",
                table: "detallesCotizacion");

            migrationBuilder.DropIndex(
                name: "IX_detallesCotizacion_id_cotizacion",
                table: "detallesCotizacion");

            migrationBuilder.AlterColumn<int>(
                name: "id_cotizacion",
                table: "detallesCotizacion",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Cotizacionid_cotizacion",
                table: "detallesCotizacion",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_detallesCotizacion_Cotizacionid_cotizacion",
                table: "detallesCotizacion",
                column: "Cotizacionid_cotizacion");

            migrationBuilder.AddForeignKey(
                name: "FK_detallesCotizacion_cotizaciones_Cotizacionid_cotizacion",
                table: "detallesCotizacion",
                column: "Cotizacionid_cotizacion",
                principalTable: "cotizaciones",
                principalColumn: "id_cotizacion",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_detallesCotizacion_cotizaciones_Cotizacionid_cotizacion",
                table: "detallesCotizacion");

            migrationBuilder.DropIndex(
                name: "IX_detallesCotizacion_Cotizacionid_cotizacion",
                table: "detallesCotizacion");

            migrationBuilder.DropColumn(
                name: "Cotizacionid_cotizacion",
                table: "detallesCotizacion");

            migrationBuilder.AlterColumn<int>(
                name: "id_cotizacion",
                table: "detallesCotizacion",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_detallesCotizacion_id_cotizacion",
                table: "detallesCotizacion",
                column: "id_cotizacion");

            migrationBuilder.AddForeignKey(
                name: "FK_detallesCotizacion_cotizaciones_id_cotizacion",
                table: "detallesCotizacion",
                column: "id_cotizacion",
                principalTable: "cotizaciones",
                principalColumn: "id_cotizacion",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
