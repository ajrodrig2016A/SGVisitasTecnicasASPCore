using Microsoft.EntityFrameworkCore.Migrations;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class Update_FK_cotizaciones_categorias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productos_categorias_categoria_id",
                table: "productos");

            migrationBuilder.DropIndex(
                name: "IX_productos_categoria_id",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "categoria_id",
                table: "productos");

            migrationBuilder.AddColumn<int>(
                name: "id_categoria",
                table: "productos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_productos_id_categoria",
                table: "productos",
                column: "id_categoria");

            migrationBuilder.AddForeignKey(
                name: "FK_productos_categorias_id_categoria",
                table: "productos",
                column: "id_categoria",
                principalTable: "categorias",
                principalColumn: "id_categoria",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productos_categorias_id_categoria",
                table: "productos");

            migrationBuilder.DropIndex(
                name: "IX_productos_id_categoria",
                table: "productos");

            migrationBuilder.DropColumn(
                name: "id_categoria",
                table: "productos");

            migrationBuilder.AddColumn<int>(
                name: "categoria_id",
                table: "productos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_productos_categoria_id",
                table: "productos",
                column: "categoria_id");

            migrationBuilder.AddForeignKey(
                name: "FK_productos_categorias_categoria_id",
                table: "productos",
                column: "categoria_id",
                principalTable: "categorias",
                principalColumn: "id_categoria",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
