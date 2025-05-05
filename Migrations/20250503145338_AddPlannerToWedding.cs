using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DreamDay.Migrations
{
    /// <inheritdoc />
    public partial class AddPlannerToWedding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_AspNetUsers_CoupleId",
                table: "Weddings");

            migrationBuilder.AddColumn<string>(
                name: "PlannerId",
                table: "Weddings",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Weddings_PlannerId",
                table: "Weddings",
                column: "PlannerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_AspNetUsers_CoupleId",
                table: "Weddings",
                column: "CoupleId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_AspNetUsers_PlannerId",
                table: "Weddings",
                column: "PlannerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_AspNetUsers_CoupleId",
                table: "Weddings");

            migrationBuilder.DropForeignKey(
                name: "FK_Weddings_AspNetUsers_PlannerId",
                table: "Weddings");

            migrationBuilder.DropIndex(
                name: "IX_Weddings_PlannerId",
                table: "Weddings");

            migrationBuilder.DropColumn(
                name: "PlannerId",
                table: "Weddings");

            migrationBuilder.AddForeignKey(
                name: "FK_Weddings_AspNetUsers_CoupleId",
                table: "Weddings",
                column: "CoupleId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
