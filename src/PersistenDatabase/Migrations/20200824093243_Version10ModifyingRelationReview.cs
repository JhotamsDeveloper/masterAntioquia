using Microsoft.EntityFrameworkCore.Migrations;

namespace Persisten.Database.Migrations
{
    public partial class Version10ModifyingRelationReview : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_Reviews_ReviewsId",
                table: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Places_ReviewsId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "ReviewsId",
                table: "Places");

            migrationBuilder.AddColumn<int>(
                name: "PlaceId",
                table: "Reviews",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PlaceId",
                table: "Reviews",
                column: "PlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Places_PlaceId",
                table: "Reviews",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "PlaceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Places_PlaceId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_PlaceId",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "Reviews");

            migrationBuilder.AddColumn<int>(
                name: "ReviewsId",
                table: "Places",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Places_ReviewsId",
                table: "Places",
                column: "ReviewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Reviews_ReviewsId",
                table: "Places",
                column: "ReviewsId",
                principalTable: "Reviews",
                principalColumn: "ReviewID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
