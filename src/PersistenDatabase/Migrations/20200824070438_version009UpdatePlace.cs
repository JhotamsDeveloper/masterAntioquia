using Microsoft.EntityFrameworkCore.Migrations;

namespace Persisten.Database.Migrations
{
    public partial class version009UpdatePlace : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "urban",
                table: "Places",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "urban",
                table: "Places");
        }
    }
}
