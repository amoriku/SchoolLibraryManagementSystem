using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_LibraryItemCopies_LibraryItemCopyId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "LibraryItemCopyId",
                table: "Reservations",
                newName: "LibraryItemId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_LibraryItemCopyId",
                table: "Reservations",
                newName: "IX_Reservations_LibraryItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_LibraryItemCopies_LibraryItemId",
                table: "Reservations",
                column: "LibraryItemId",
                principalTable: "LibraryItemCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_LibraryItemCopies_LibraryItemId",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "LibraryItemId",
                table: "Reservations",
                newName: "LibraryItemCopyId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_LibraryItemId",
                table: "Reservations",
                newName: "IX_Reservations_LibraryItemCopyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_LibraryItemCopies_LibraryItemCopyId",
                table: "Reservations",
                column: "LibraryItemCopyId",
                principalTable: "LibraryItemCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
