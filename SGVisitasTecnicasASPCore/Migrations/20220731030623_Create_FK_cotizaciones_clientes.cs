using Microsoft.EntityFrameworkCore.Migrations;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class Create_FK_cotizaciones_clientes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_cliente",
                table: "cotizaciones",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_cotizaciones_id_cliente",
                table: "cotizaciones",
                column: "id_cliente");

            migrationBuilder.AddForeignKey(
                name: "FK_cotizaciones_clientes_id_cliente",
                table: "cotizaciones",
                column: "id_cliente",
                principalTable: "clientes",
                principalColumn: "id_cliente",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_cotizaciones_clientes_id_cliente",
                table: "cotizaciones");

            migrationBuilder.DropIndex(
                name: "IX_cotizaciones_id_cliente",
                table: "cotizaciones");

            migrationBuilder.DropColumn(
                name: "id_cliente",
                table: "cotizaciones");
        }
    }
}
