using Microsoft.EntityFrameworkCore.Migrations;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class Change_fieldsTbl_visitas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "descripcion_problema",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "descripcion_req",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "requerimiento",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "requiere_instalacion",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "requiere_mantenimiento",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "tiempo_entrega",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "ubicacion_disp_seguridad",
                table: "visitas");

            migrationBuilder.AlterColumn<string>(
                name: "estado",
                table: "visitas",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "descripcion",
                table: "visitas",
                maxLength: 2147483647,
                nullable: false);

            migrationBuilder.AddColumn<bool>(
                name: "esProblema",
                table: "visitas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "requiereInstalacion",
                table: "visitas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "requiereMantenimiento",
                table: "visitas",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "tiempoEntrega",
                table: "visitas",
                maxLength: 90,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ubicacionDispSeguridad",
                table: "visitas",
                maxLength: 120,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "descripcion",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "esProblema",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "requiereInstalacion",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "requiereMantenimiento",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "tiempoEntrega",
                table: "visitas");

            migrationBuilder.DropColumn(
                name: "ubicacionDispSeguridad",
                table: "visitas");

            migrationBuilder.AlterColumn<string>(
                name: "estado",
                table: "visitas",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "descripcion_problema",
                table: "visitas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "descripcion_req",
                table: "visitas",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "requerimiento",
                table: "visitas",
                type: "varchar(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "requiere_instalacion",
                table: "visitas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "requiere_mantenimiento",
                table: "visitas",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "tiempo_entrega",
                table: "visitas",
                type: "varchar(90)",
                maxLength: 90,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ubicacion_disp_seguridad",
                table: "visitas",
                type: "varchar(120)",
                maxLength: 120,
                nullable: true);
        }
    }
}
