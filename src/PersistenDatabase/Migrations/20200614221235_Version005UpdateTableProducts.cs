using Microsoft.EntityFrameworkCore.Migrations;

namespace Persisten.Database.Migrations
{
    public partial class Version005UpdateTableProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Products_ProductsProductId",
                table: "Galleries");

            migrationBuilder.DropIndex(
                name: "IX_Galleries_ProductsProductId",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "HalfPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "HighPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "LowPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ProductsProductId",
                table: "Galleries");

            migrationBuilder.AlterColumn<float>(
                name: "Price",
                table: "Products",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Increments",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SquareCover",
                table: "Products",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Galleries_ProducId",
                table: "Galleries",
                column: "ProducId");

            migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Products_ProducId",
                table: "Galleries",
                column: "ProducId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Galleries_Products_ProducId",
                table: "Galleries");

            migrationBuilder.DropIndex(
                name: "IX_Galleries_ProducId",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "Increments",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SquareCover",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Price",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<float>(
                name: "HalfPrice",
                table: "Products",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "HighPrice",
                table: "Products",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "LowPrice",
                table: "Products",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "ProductsProductId",
                table: "Galleries",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Galleries_ProductsProductId",
                table: "Galleries",
                column: "ProductsProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Galleries_Products_ProductsProductId",
                table: "Galleries",
                column: "ProductsProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
