using Microsoft.EntityFrameworkCore.Migrations;

namespace SGVisitasTecnicasASPCore.Migrations
{
    public partial class InitialDB_ImageName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageName",
                table: "productos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageName",
                table: "productos");
        }
    }
}
