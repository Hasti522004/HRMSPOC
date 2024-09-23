using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HRMSPOC.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleToApplicationUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "971eaba3-0c9c-43c4-8c46-a7919dba2878");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e58564fc-d655-4559-aea7-216e659157d8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "68d35f6d-1392-4215-9adc-8ccb5064fae4", "2", "HR", "HR" },
                    { "d0d04c9d-987f-4c55-988b-24669f758331", "1", "Admin", "Admin" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "68d35f6d-1392-4215-9adc-8ccb5064fae4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d0d04c9d-987f-4c55-988b-24669f758331");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "971eaba3-0c9c-43c4-8c46-a7919dba2878", "1", "Admin", "Admin" },
                    { "e58564fc-d655-4559-aea7-216e659157d8", "2", "HR", "HR" }
                });
        }
    }
}
