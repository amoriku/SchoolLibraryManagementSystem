using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservationAndLibraryItemEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_LibraryItemCopies_LibraryItemId",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "LibraryItemCopyId",
                table: "Reservations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_LibraryItemCopyId",
                table: "Reservations",
                column: "LibraryItemCopyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_LibraryItemCopies_LibraryItemCopyId",
                table: "Reservations",
                column: "LibraryItemCopyId",
                principalTable: "LibraryItemCopies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_LibraryItems_LibraryItemId",
                table: "Reservations",
                column: "LibraryItemId",
                principalTable: "LibraryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_LibraryItemCopies_LibraryItemCopyId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_LibraryItems_LibraryItemId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_LibraryItemCopyId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "LibraryItemCopyId",
                table: "Reservations");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_LibraryItemCopies_LibraryItemId",
                table: "Reservations",
                column: "LibraryItemId",
                principalTable: "LibraryItemCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
