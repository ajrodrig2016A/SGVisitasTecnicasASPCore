using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class IntialDb_productos_categorias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categorias",
                columns: table => new
                {
                    id_categoria = table.Column<int>(nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nombre = table.Column<string>(maxLength: 64, nullable: false),
                    descripcion = table.Column<string>(maxLength: 2147483647, nullable: false),
                    estado = table.Column<string>(maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categorias", x => x.id_categoria);
                });

            migrationBuilder.CreateTable(
                name: "productos",
                columns: table => new
                {
                    id_producto = table.Column<int>(nullable: false),
                    nombre = table.Column<string>(maxLength: 90, nullable: false),
                    marca = table.Column<string>(maxLength: 60, nullable: false),
                    descripcion = table.Column<string>(maxLength: 2147483647, nullable: false),
                    unidad = table.Column<string>(maxLength: 8, nullable: false),
                    cantidad = table.Column<decimal>(nullable: false),
                    precioUnitario = table.Column<decimal>(nullable: false),
                    stock = table.Column<decimal>(nullable: false),
                    descuento = table.Column<decimal>(nullable: false),
                    porcentaje = table.Column<decimal>(nullable: false),
                    categoria_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productos", x => x.id_producto);
                    table.ForeignKey(
                        name: "FK_productos_categorias_id_producto",
                        column: x => x.id_producto,
                        principalTable: "categorias",
                        principalColumn: "id_categoria",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "productos");

            migrationBuilder.DropTable(
                name: "categorias");
        }
    }
}
