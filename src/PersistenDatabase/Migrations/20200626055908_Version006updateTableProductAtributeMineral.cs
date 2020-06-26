using Microsoft.EntityFrameworkCore.Migrations;

namespace Persisten.Database.Migrations
{
    public partial class Version006updateTableProductAtributeMineral : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Mineral",
                table: "Products",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Mineral",
                table: "Products");
        }
    }
}
