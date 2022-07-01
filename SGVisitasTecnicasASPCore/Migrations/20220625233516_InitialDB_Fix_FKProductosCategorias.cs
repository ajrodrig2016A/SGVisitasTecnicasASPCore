using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class InitialDB_Fix_FKProductosCategorias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productos_categorias_id_producto",
                table: "productos");

            migrationBuilder.AlterColumn<int>(
                name: "id_producto",
                table: "productos",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_productos_categorias_categoria_id",
                table: "productos");

            migrationBuilder.DropIndex(
                name: "IX_productos_categoria_id",
                table: "productos");

            migrationBuilder.AlterColumn<int>(
                name: "id_producto",
                table: "productos",
                type: "int",
                nullable: false,
                oldClrType: typeof(int))
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddForeignKey(
                name: "FK_productos_categorias_id_producto",
                table: "productos",
                column: "id_producto",
                principalTable: "categorias",
                principalColumn: "id_categoria",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
