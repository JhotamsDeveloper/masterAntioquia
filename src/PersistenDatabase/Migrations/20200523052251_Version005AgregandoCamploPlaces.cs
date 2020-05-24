using Microsoft.EntityFrameworkCore.Migrations;

namespace Persisten.Database.Migrations
{
    public partial class Version005AgregandoCamploPlaces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Places",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Places",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Places");
        }
    }
}
