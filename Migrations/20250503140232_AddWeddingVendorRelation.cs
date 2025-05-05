using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DreamDay.Migrations
{
    /// <inheritdoc />
    public partial class AddWeddingVendorRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DeleteData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Vendors",
                keyColumn: "Id",
                keyValue: 6);

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

            migrationBuilder.CreateTable(
                name: "WeddingVendor",
                columns: table => new
                {
                    WeddingId = table.Column<int>(type: "int", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeddingVendor", x => new { x.WeddingId, x.VendorId });
                    table.ForeignKey(
                        name: "FK_WeddingVendor_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeddingVendor_Weddings_WeddingId",
                        column: x => x.WeddingId,
                        principalTable: "Weddings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeddingVendor_VendorId",
                table: "WeddingVendor",
                column: "VendorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeddingVendor");

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "Id", "Category", "Description", "Name", "PriceRange" },
                values: new object[,]
                {
                    { 1, "Venue", "Outdoor garden weddings with floral decor and lighting.", "Sunset Garden Venue", 4500m },
                    { 2, "Photographer", "Professional wedding photography and cinematic video packages.", "Moments Photography", 3200m },
                    { 3, "Caterer", "Fine-dining wedding catering services with custom menus.", "Gourmet Bites Catering", 5500m },
                    { 4, "Florist", "Luxury floral arrangements and wedding bouquet design.", "DreamFlorals", 1800m },
                    { 5, "Decorator", "Venue decor, draping, centerpieces, and lighting specialists.", "Elegant Decor", 2700m },
                    { 6, "DJ/Music", "Top-rated wedding DJs and live band coordination.", "Vow Tunes", 2000m }
                });

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
    }
}
