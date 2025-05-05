using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DreamDay.Migrations
{
    /// <inheritdoc />
    public partial class SeedWeddings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_AspNetUsers_CoupleId",
                table: "Weddings");

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

            migrationBuilder.AlterColumn<string>(
                name: "CoupleId",
                table: "Weddings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.InsertData(
                table: "Weddings",
                columns: new[] { "Id", "CoupleId", "Date", "Title" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Beachside Bliss" },
                    { 2, null, new DateTime(2024, 8, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Garden Elegance" },
                    { 3, null, new DateTime(2023, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Classic Ballroom Romance" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_AspNetUsers_CoupleId",
                table: "Weddings",
                column: "CoupleId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_AspNetUsers_CoupleId",
                table: "Weddings");

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

            migrationBuilder.AlterColumn<string>(
                name: "CoupleId",
                table: "Weddings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Vendors",
                columns: new[] { "Id", "Category", "Description", "Name", "PriceRange" },
                values: new object[,]
                {
                    { 1, "Florist", "Premium floral arrangements", "Elegant Events Florist", 1500m },
                    { 2, "Photographer", "Wedding photography and videography", "Moments Photography", 3000m },
                    { 3, "Caterer", "Luxury wedding catering services", "Gourmet Bites Catering", 5000m }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_AspNetUsers_CoupleId",
                table: "Weddings",
                column: "CoupleId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
