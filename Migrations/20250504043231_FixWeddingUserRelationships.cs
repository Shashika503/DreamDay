using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamDay.Migrations
{
    /// <inheritdoc />
    public partial class FixWeddingUserRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_AspNetUsers_PlannerId",
                table: "Weddings");

            migrationBuilder.DropTable(
                name: "WeddingVendor");

            migrationBuilder.AlterColumn<string>(
                name: "CoupleId",
                table: "Weddings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Venue",
                table: "Weddings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "WeddingVendors",
                columns: table => new
                {
                    VendorsId = table.Column<int>(type: "int", nullable: false),
                    WeddingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeddingVendors", x => new { x.VendorsId, x.WeddingsId });
                    table.ForeignKey(
                        name: "FK_WeddingVendors_Vendors_VendorsId",
                        column: x => x.VendorsId,
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WeddingVendors_Weddings_WeddingsId",
                        column: x => x.WeddingsId,
                        principalTable: "Weddings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeddingVendors_WeddingsId",
                table: "WeddingVendors",
                column: "WeddingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_AspNetUsers_PlannerId",
                table: "Weddings",
                column: "PlannerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_AspNetUsers_PlannerId",
                table: "Weddings");

            migrationBuilder.DropTable(
                name: "WeddingVendors");

            migrationBuilder.DropColumn(
                name: "Venue",
                table: "Weddings");

            migrationBuilder.AlterColumn<string>(
                name: "CoupleId",
                table: "Weddings",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_AspNetUsers_PlannerId",
                table: "Weddings",
                column: "PlannerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
