using Microsoft.EntityFrameworkCore.Migrations;

namespace Persisten.Database.Migrations
{
    public partial class Version10AddReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReviewsId",
                table: "Places",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReviewsId",
                table: "Galleries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameUser = table.Column<string>(nullable: true),
                    TitleReview = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Assessment = table.Column<int>(nullable: false),
                    imgUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Places_ReviewsId",
                table: "Places",
                column: "ReviewsId");

            migrationBuilder.CreateIndex(
                name: "IX_Galleries_ReviewsId",
                table: "Galleries",
                column: "ReviewsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Reviews_ReviewsId",
                table: "Galleries",
                column: "ReviewsId",
                principalTable: "Reviews",
                principalColumn: "ReviewID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Reviews_ReviewsId",
                table: "Places",
                column: "ReviewsId",
                principalTable: "Reviews",
                principalColumn: "ReviewID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Reviews_ReviewsId",
                table: "Galleries");

            migrationBuilder.DropForeignKey(
                name: "FK_Places_Reviews_ReviewsId",
                table: "Places");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Places_ReviewsId",
                table: "Places");

            migrationBuilder.DropIndex(
                name: "IX_Galleries_ReviewsId",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "ReviewsId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "ReviewsId",
                table: "Galleries");
        }
    }
}
