using Microsoft.EntityFrameworkCore.Migrations;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class Create_FK_productos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "marca",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "unidad",
                table: "productos");

            migrationBuilder.AddColumn<int>(
                name: "id_marca",
                table: "productos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "id_unidad",
                table: "productos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_productos_id_marca",
                table: "productos",
                column: "id_marca");

            migrationBuilder.CreateIndex(
                name: "IX_productos_id_unidad",
                table: "productos",
                column: "id_unidad");

            migrationBuilder.AddForeignKey(
                name: "FK_productos_marcas_id_marca",
                table: "productos",
                column: "id_marca",
                principalTable: "marcas",
                principalColumn: "id_marca",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_productos_unidades_id_unidad",
                table: "productos",
                column: "id_unidad",
                principalTable: "unidades",
                principalColumn: "id_unidad",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productos_marcas_id_marca",
                table: "productos");

            migrationBuilder.DropForeignKey(
                name: "FK_productos_unidades_id_unidad",
                table: "productos");

            migrationBuilder.DropIndex(
                name: "IX_productos_id_marca",
                table: "productos");

            migrationBuilder.DropIndex(
                name: "IX_productos_id_unidad",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "id_marca",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "id_unidad",
                table: "productos");

            migrationBuilder.AddColumn<string>(
                name: "marca",
                table: "productos",
                type: "varchar(60)",
                maxLength: 60,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "unidad",
                table: "productos",
                type: "varchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");
        }
    }
}
