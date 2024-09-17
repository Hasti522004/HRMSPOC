using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HRMSPOC.API.Migrations
{
    /// <inheritdoc />
    public partial class changesinRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82c32285-a990-48ed-a010-22d15572b006");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c1199dc6-1f0b-4def-90b0-12ce489959b4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31959ad5-7624-4932-8238-f6a081330363", "1", "Admin", "Admin" },
                    { "84a7880b-b08d-499c-afca-c0c94017240d", "2", "HR", "HR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31959ad5-7624-4932-8238-f6a081330363");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "84a7880b-b08d-499c-afca-c0c94017240d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "82c32285-a990-48ed-a010-22d15572b006", "1", "Admin", "Admin" },
                    { "c1199dc6-1f0b-4def-90b0-12ce489959b4", "2", "HR", "HR" }
                });
        }
    }
}
