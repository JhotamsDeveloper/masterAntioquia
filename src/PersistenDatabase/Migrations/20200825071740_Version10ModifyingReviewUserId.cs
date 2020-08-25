using Microsoft.EntityFrameworkCore.Migrations;

namespace Persisten.Database.Migrations
{
    public partial class Version10ModifyingReviewUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameUser",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "TitleReview",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "imgUser",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "TittleReview",
                table: "Reviews",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Reviews",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TittleReview",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reviews");

            migrationBuilder.AddColumn<string>(
                name: "NameUser",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TitleReview",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "imgUser",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
