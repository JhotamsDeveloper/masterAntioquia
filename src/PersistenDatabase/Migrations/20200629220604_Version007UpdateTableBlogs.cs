using Microsoft.EntityFrameworkCore.Migrations;

namespace Persisten.Database.Migrations
{
    public partial class Version007UpdateTableBlogs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Subcategory",
                table: "Categorys");

            migrationBuilder.AddColumn<string>(
                name: "EventUrl",
                table: "Events",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventUrl",
                table: "Events");

            migrationBuilder.AddColumn<string>(
                name: "Subcategory",
                table: "Categorys",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
