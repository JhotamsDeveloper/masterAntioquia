using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Persisten.Database.Migrations
{
    public partial class version002Places : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Place_Categorys_CategoryId",
                table: "Place");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Place_PlaceId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Place",
                table: "Place");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Place");

            migrationBuilder.RenameTable(
                name: "Place",
                newName: "Places");

            migrationBuilder.RenameColumn(
                name: "admin",
                table: "Places",
                newName: "Admin");

            migrationBuilder.RenameIndex(
                name: "IX_Place_CategoryId",
                table: "Places",
                newName: "IX_Places_CategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categorys",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Admin",
                table: "Places",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Places",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte>(
                name: "CoverPage",
                table: "Places",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Places",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Contract",
                table: "Places",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Places",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Places",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "Logo",
                table: "Places",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.AddColumn<string>(
                name: "Nit",
                table: "Places",
                maxLength: 15,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Places",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "State",
                table: "Places",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdateDate",
                table: "Places",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Places",
                table: "Places",
                column: "PlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Categorys_CategoryId",
                table: "Places",
                column: "CategoryId",
                principalTable: "Categorys",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Places_PlaceId",
                table: "Products",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "PlaceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_Categorys_CategoryId",
                table: "Places");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Places_PlaceId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Places",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Contract",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Logo",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Nit",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "UpdateDate",
                table: "Places");

            migrationBuilder.RenameTable(
                name: "Places",
                newName: "Place");

            migrationBuilder.RenameColumn(
                name: "Admin",
                table: "Place",
                newName: "admin");

            migrationBuilder.RenameIndex(
                name: "IX_Places_CategoryId",
                table: "Place",
                newName: "IX_Place_CategoryId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categorys",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Place",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "CoverPage",
                table: "Place",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(byte));

            migrationBuilder.AlterColumn<string>(
                name: "admin",
                table: "Place",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Place",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Place",
                table: "Place",
                column: "PlaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Place_Categorys_CategoryId",
                table: "Place",
                column: "CategoryId",
                principalTable: "Categorys",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Place_PlaceId",
                table: "Products",
                column: "PlaceId",
                principalTable: "Place",
                principalColumn: "PlaceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
