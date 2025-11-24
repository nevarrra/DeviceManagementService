using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DeviceManagementService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Devices",
                columns: new[] { "Id", "Brand", "CreatedAt", "Name", "State" },
                values: new object[,]
                {
                    { 1, "Apple", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "MacBook Pro 16", 0 },
                    { 2, "Lenovo", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "ThinkPad X1 Carbon", 1 },
                    { 3, "Microsoft", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Surface Laptop 5", 0 },
                    { 4, "Siemens", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "WashPRO", 2 },
                    { 5, "Samsung", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "BestWifiFridge", 0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");
        }
    }
}
