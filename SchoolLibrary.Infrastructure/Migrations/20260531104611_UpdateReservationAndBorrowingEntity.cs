using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReservationAndBorrowingEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowing_AspNetUsers_ReaderId",
                table: "Borrowing");

            migrationBuilder.DropForeignKey(
                name: "FK_Borrowing_LibraryItemCopies_LibraryItemCopyId",
                table: "Borrowing");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_AspNetUsers_ReaderId",
                table: "Reservation");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_LibraryItemCopies_LibraryItemCopyId",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Borrowing",
                table: "Borrowing");

            migrationBuilder.RenameTable(
                name: "Reservation",
                newName: "Reservations");

            migrationBuilder.RenameTable(
                name: "Borrowing",
                newName: "Borrowings");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_ReaderId",
                table: "Reservations",
                newName: "IX_Reservations_ReaderId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservation_LibraryItemCopyId",
                table: "Reservations",
                newName: "IX_Reservations_LibraryItemCopyId");

            migrationBuilder.RenameIndex(
                name: "IX_Borrowing_ReaderId",
                table: "Borrowings",
                newName: "IX_Borrowings_ReaderId");

            migrationBuilder.RenameIndex(
                name: "IX_Borrowing_LibraryItemCopyId",
                table: "Borrowings",
                newName: "IX_Borrowings_LibraryItemCopyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Borrowings",
                table: "Borrowings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_AspNetUsers_ReaderId",
                table: "Borrowings",
                column: "ReaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_LibraryItemCopies_LibraryItemCopyId",
                table: "Borrowings",
                column: "LibraryItemCopyId",
                principalTable: "LibraryItemCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_ReaderId",
                table: "Reservations",
                column: "ReaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_LibraryItemCopies_LibraryItemCopyId",
                table: "Reservations",
                column: "LibraryItemCopyId",
                principalTable: "LibraryItemCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_AspNetUsers_ReaderId",
                table: "Borrowings");

            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_LibraryItemCopies_LibraryItemCopyId",
                table: "Borrowings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_ReaderId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_LibraryItemCopies_LibraryItemCopyId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reservations",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Borrowings",
                table: "Borrowings");

            migrationBuilder.RenameTable(
                name: "Reservations",
                newName: "Reservation");

            migrationBuilder.RenameTable(
                name: "Borrowings",
                newName: "Borrowing");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_ReaderId",
                table: "Reservation",
                newName: "IX_Reservation_ReaderId");

            migrationBuilder.RenameIndex(
                name: "IX_Reservations_LibraryItemCopyId",
                table: "Reservation",
                newName: "IX_Reservation_LibraryItemCopyId");

            migrationBuilder.RenameIndex(
                name: "IX_Borrowings_ReaderId",
                table: "Borrowing",
                newName: "IX_Borrowing_ReaderId");

            migrationBuilder.RenameIndex(
                name: "IX_Borrowings_LibraryItemCopyId",
                table: "Borrowing",
                newName: "IX_Borrowing_LibraryItemCopyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reservation",
                table: "Reservation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Borrowing",
                table: "Borrowing",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowing_AspNetUsers_ReaderId",
                table: "Borrowing",
                column: "ReaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowing_LibraryItemCopies_LibraryItemCopyId",
                table: "Borrowing",
                column: "LibraryItemCopyId",
                principalTable: "LibraryItemCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_AspNetUsers_ReaderId",
                table: "Reservation",
                column: "ReaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_LibraryItemCopies_LibraryItemCopyId",
                table: "Reservation",
                column: "LibraryItemCopyId",
                principalTable: "LibraryItemCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
