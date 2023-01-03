using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CodeWright.Metadata.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Metadata",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Metadata", x => new { x.Id, x.TenantId, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "References",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    TenantId = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetId = table.Column<string>(type: "TEXT", nullable: false),
                    Version = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_References", x => new { x.Id, x.TenantId, x.Type, x.TargetId });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Metadata_Id_TenantId",
                table: "Metadata",
                columns: new[] { "Id", "TenantId" });

            migrationBuilder.CreateIndex(
                name: "IX_Metadata_TenantId_Name_Value",
                table: "Metadata",
                columns: new[] { "TenantId", "Name", "Value" });

            migrationBuilder.CreateIndex(
                name: "IX_References_Id_TenantId",
                table: "References",
                columns: new[] { "Id", "TenantId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Metadata");

            migrationBuilder.DropTable(
                name: "References");
        }
    }
}
