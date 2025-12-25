using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace D2LExtensionWebAPPSSR.Migrations
{
    /// <inheritdoc />
    public partial class FixAllowNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PomodoroTasks",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PomodoroTasks",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000);

            migrationBuilder.UpdateData(
                table: "PomodoroTasks",
                keyColumn: "PromodoroTaskId",
                keyValue: 1,
                columns: new[] { "CompletedTime", "CreatedTime" },
                values: new object[] { new DateTime(2025, 12, 26, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 12, 25, 0, 0, 0, 0, DateTimeKind.Local) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "PomodoroTasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PomodoroTasks",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "PomodoroTasks",
                keyColumn: "PromodoroTaskId",
                keyValue: 1,
                columns: new[] { "CompletedTime", "CreatedTime" },
                values: new object[] { new DateTime(2025, 11, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
