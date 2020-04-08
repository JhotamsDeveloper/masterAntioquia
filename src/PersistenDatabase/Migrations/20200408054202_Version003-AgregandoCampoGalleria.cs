using Microsoft.EntityFrameworkCore.Migrations;

namespace Persisten.Database.Migrations
{
    public partial class Version003AgregandoCampoGalleria : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GalleryId",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_GalleryId",
                table: "Products",
                column: "GalleryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Galleries_GalleryId",
                table: "Products",
                column: "GalleryId",
                principalTable: "Galleries",
                principalColumn: "GalleryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Galleries_GalleryId",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_GalleryId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "GalleryId",
                table: "Products");
        }
    }
}
