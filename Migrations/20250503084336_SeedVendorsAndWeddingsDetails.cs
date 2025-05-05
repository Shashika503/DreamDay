using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DreamDay.Migrations
{
    /// <inheritdoc />
    public partial class SeedVendorsAndWeddingsDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Weddings",
                columns: new[] { "Id", "CoupleId", "Date", "Title" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beachside Bliss" },
                    { 2, null, new DateTime(2024, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Garden Elegance" },
                    { 3, null, new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Classic Ballroom Romance" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Weddings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Weddings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Weddings",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
