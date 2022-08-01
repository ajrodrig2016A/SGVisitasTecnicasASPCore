using Microsoft.EntityFrameworkCore.Migrations;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class Update_FK_visitas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_visitas_clientes_cliente_id",
                table: "visitas");

            migrationBuilder.DropForeignKey(
                name: "FK_visitas_empleados_empleado_id",
                table: "visitas");

            migrationBuilder.DropIndex(
                name: "IX_visitas_cliente_id",
                table: "visitas");

            migrationBuilder.DropIndex(
                name: "IX_visitas_empleado_id",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "cliente_id",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "empleado_id",
                table: "visitas");

            migrationBuilder.AddColumn<int>(
                name: "id_cliente",
                table: "visitas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "id_empleado",
                table: "visitas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_visitas_id_cliente",
                table: "visitas",
                column: "id_cliente");

            migrationBuilder.CreateIndex(
                name: "IX_visitas_id_empleado",
                table: "visitas",
                column: "id_empleado");

            migrationBuilder.AddForeignKey(
                name: "FK_visitas_clientes_id_cliente",
                table: "visitas",
                column: "id_cliente",
                principalTable: "clientes",
                principalColumn: "id_cliente",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_visitas_empleados_id_empleado",
                table: "visitas",
                column: "id_empleado",
                principalTable: "empleados",
                principalColumn: "id_empleado",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_visitas_clientes_id_cliente",
                table: "visitas");

            migrationBuilder.DropForeignKey(
                name: "FK_visitas_empleados_id_empleado",
                table: "visitas");

            migrationBuilder.DropIndex(
                name: "IX_visitas_id_cliente",
                table: "visitas");

            migrationBuilder.DropIndex(
                name: "IX_visitas_id_empleado",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "id_cliente",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "id_empleado",
                table: "visitas");

            migrationBuilder.AddColumn<int>(
                name: "cliente_id",
                table: "visitas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "empleado_id",
                table: "visitas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_visitas_cliente_id",
                table: "visitas",
                column: "cliente_id");

            migrationBuilder.CreateIndex(
                name: "IX_visitas_empleado_id",
                table: "visitas",
                column: "empleado_id");

            migrationBuilder.AddForeignKey(
                name: "FK_visitas_clientes_cliente_id",
                table: "visitas",
                column: "cliente_id",
                principalTable: "clientes",
                principalColumn: "id_cliente",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_visitas_empleados_empleado_id",
                table: "visitas",
                column: "empleado_id",
                principalTable: "empleados",
                principalColumn: "id_empleado",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
