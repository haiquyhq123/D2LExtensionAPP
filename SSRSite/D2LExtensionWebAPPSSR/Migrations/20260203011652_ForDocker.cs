using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace D2LExtensionWebAPPSSR.Migrations
{
    /// <inheritdoc />
    public partial class ForDocker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PomodoroTasks",
                keyColumn: "PromodoroTaskId",
                keyValue: 1,
                columns: new[] { "CompletedTime", "CreatedTime" },
                values: new object[] { new DateTime(2026, 2, 3, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 2, 2, 0, 0, 0, 0, DateTimeKind.Local) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PomodoroTasks",
                keyColumn: "PromodoroTaskId",
                keyValue: 1,
                columns: new[] { "CompletedTime", "CreatedTime" },
                values: new object[] { new DateTime(2026, 1, 13, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2026, 1, 12, 0, 0, 0, 0, DateTimeKind.Local) });
        }
    }
}
