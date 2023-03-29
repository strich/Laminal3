using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Laminal.Server.Migrations
{
    /// <inheritdoc />
    public partial class dboperation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "_Operations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    AgentId = table.Column<string>(type: "TEXT", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CommitTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CommandJson = table.Column<string>(type: "TEXT", nullable: false),
                    ItemsJson = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Operations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommitTime",
                table: "_Operations",
                column: "CommitTime");

            migrationBuilder.CreateIndex(
                name: "IX_StartTime",
                table: "_Operations",
                column: "StartTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "_Operations");
        }
    }
}
