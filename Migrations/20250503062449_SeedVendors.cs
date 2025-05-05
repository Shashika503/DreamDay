using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DreamDay.Migrations
{
    /// <inheritdoc />
    public partial class SeedVendors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "Id", "Category", "Description", "Name", "PriceRange" },
                values: new object[,]
                {
                    { 1, "Florist", "Premium floral arrangements", "Elegant Events Florist", 1500m },
                    { 2, "Photographer", "Wedding photography and videography", "Moments Photography", 3000m },
                    { 3, "Caterer", "Luxury wedding catering services", "Gourmet Bites Catering", 5000m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
