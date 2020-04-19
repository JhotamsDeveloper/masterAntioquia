using Microsoft.EntityFrameworkCore.Migrations;

namespace Persisten.Database.Migrations
{
    public partial class Version004CreateUserRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { "b4263412-a981-4dcd-9ebc-57fa56769552", "d953ba6d-e73b-42d5-a246-a59a6ff5dd9d", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9e58de91-a5dd-45cd-9238-6c37d4f681be", "8e5d59e5-9efb-412d-9b48-7bedcbac403b", "UserApp", "UserApp" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9e58de91-a5dd-45cd-9238-6c37d4f681be");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b4263412-a981-4dcd-9ebc-57fa56769552");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a1ba6ec5-12c1-4830-afd5-4b6996933910", "788dea2f-5c8b-4092-8b86-95c69b4811bd", "Admin", "Admin" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6036db73-bbe0-465b-b30e-4d38a3a67434", "17282073-617a-45f3-b0d9-5d40d85c264c", "UserApp", "UserApp" });
        }
    }
}
