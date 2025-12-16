using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace D2LExtensionWebAPPSSR.Migrations
{
    /// <inheritdoc />
    public partial class InsertedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "d22a65a3-e92e-4254-a9b1-0c8594eebf2c", null, "NormalUser", "NORMAL_USER" },
                    { "d574ff33-8ab4-43d3-9556-99ca0439af7c", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d22a65a3-e92e-4254-a9b1-0c8594eebf2c");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d574ff33-8ab4-43d3-9556-99ca0439af7c");
        }
    }
}
