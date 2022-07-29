using Microsoft.EntityFrameworkCore.Migrations;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class IntialDb_FK_cotizaciones_empleados : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cotizaciones_empleados_id_empleado",
                table: "cotizaciones");

            migrationBuilder.DropIndex(
                name: "IX_cotizaciones_id_empleado",
                table: "cotizaciones");

            migrationBuilder.DropColumn(
                name: "id_empleado",
                table: "cotizaciones");

            migrationBuilder.AddColumn<int>(
                name: "empleado_id",
                table: "cotizaciones",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_cotizaciones_empleado_id",
                table: "cotizaciones",
                column: "empleado_id");

            migrationBuilder.AddForeignKey(
                name: "FK_cotizaciones_empleados_empleado_id",
                table: "cotizaciones",
                column: "empleado_id",
                principalTable: "empleados",
                principalColumn: "id_empleado",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cotizaciones_empleados_empleado_id",
                table: "cotizaciones");

            migrationBuilder.DropIndex(
                name: "IX_cotizaciones_empleado_id",
                table: "cotizaciones");

            migrationBuilder.DropColumn(
                name: "empleado_id",
                table: "cotizaciones");

            migrationBuilder.AddColumn<int>(
                name: "id_empleado",
                table: "cotizaciones",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_cotizaciones_id_empleado",
                table: "cotizaciones",
                column: "id_empleado");

            migrationBuilder.AddForeignKey(
                name: "FK_cotizaciones_empleados_id_empleado",
                table: "cotizaciones",
                column: "id_empleado",
                principalTable: "empleados",
                principalColumn: "id_empleado",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
