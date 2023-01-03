using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeWright.Metadata.API.Migrations.EventSourceDb
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Version = table.Column<long>(type: "INTEGER", nullable: false),
                    TypeId = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    SourceId = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    CreateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => new { x.Id, x.TenantId, x.Version });
                });

            migrationBuilder.CreateTable(
                name: "Snapshots",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Version = table.Column<long>(type: "INTEGER", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Snapshots", x => new { x.Id, x.TenantId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_Id_TenantId",
                table: "Events",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Events_Version",
                table: "Events",
                column: "Version");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Snapshots");
        }
    }
}
