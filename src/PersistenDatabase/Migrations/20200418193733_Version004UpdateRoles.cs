using Microsoft.EntityFrameworkCore.Migrations;

namespace Persisten.Database.Migrations
{
    public partial class Version004UpdateRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5637f937-225e-4e11-b96a-8f3b21e4de3b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b0d92e1c-7099-448b-8cd6-8fed125b8b58");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a1ba6ec5-12c1-4830-afd5-4b6996933910", "788dea2f-5c8b-4092-8b86-95c69b4811bd", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6036db73-bbe0-465b-b30e-4d38a3a67434", "17282073-617a-45f3-b0d9-5d40d85c264c", "UserApp", "UserApp" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6036db73-bbe0-465b-b30e-4d38a3a67434");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a1ba6ec5-12c1-4830-afd5-4b6996933910");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b0d92e1c-7099-448b-8cd6-8fed125b8b58", "d3560ef2-b8c3-4084-bb20-c1f5399ed2d6", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "5637f937-225e-4e11-b96a-8f3b21e4de3b", "66a4160d-c432-4843-a60e-97a11ef95819", "UserApp", "UserApp" });
        }
    }
}
