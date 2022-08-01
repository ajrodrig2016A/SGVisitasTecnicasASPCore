using Microsoft.EntityFrameworkCore.Migrations;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class alter_column_nombreCliente_cotizaciones : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nombre_cliente",
                table: "cotizaciones");

            migrationBuilder.AddColumn<string>(
                name: "codigo",
                table: "cotizaciones",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "codigo",
                table: "cotizaciones");

            migrationBuilder.AddColumn<string>(
                name: "nombre_cliente",
                table: "cotizaciones",
                type: "varchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");
        }
    }
}
