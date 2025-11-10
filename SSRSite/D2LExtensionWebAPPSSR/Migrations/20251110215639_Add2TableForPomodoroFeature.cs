using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace D2LExtensionWebAPPSSR.Migrations
{
    /// <inheritdoc />
    public partial class Add2TableForPomodoroFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PomodoroTasks",
                columns: table => new
                {
                    PromodoroTaskId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpectedPomodoroSession = table.Column<int>(type: "int", nullable: false),
                    CompletedPomodoroSession = table.Column<int>(type: "int", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "DateTime2", nullable: false),
                    CompletedTime = table.Column<DateTime>(type: "DateTime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PomodoroTasks", x => x.PromodoroTaskId);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    SessionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Completed = table.Column<bool>(type: "bit", nullable: false),
                    PomodoroTaskId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_Sessions_PomodoroTasks_PomodoroTaskId",
                        column: x => x.PomodoroTaskId,
                        principalTable: "PomodoroTasks",
                        principalColumn: "PromodoroTaskId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "PomodoroTasks",
                columns: new[] { "PromodoroTaskId", "CompletedPomodoroSession", "CompletedTime", "CreatedTime", "Description", "ExpectedPomodoroSession", "Status", "Title" },
                values: new object[] { 1, 2, new DateTime(2025, 11, 11, 0, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 11, 10, 0, 0, 0, 0, DateTimeKind.Local), "Nah", 4, "Active", "Learning To Build" });

            migrationBuilder.InsertData(
                table: "Sessions",
                columns: new[] { "SessionId", "Completed", "Duration", "PomodoroTaskId" },
                values: new object[] { 1, true, 25, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_PomodoroTaskId",
                table: "Sessions",
                column: "PomodoroTaskId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "PomodoroTasks");
        }
    }
}
