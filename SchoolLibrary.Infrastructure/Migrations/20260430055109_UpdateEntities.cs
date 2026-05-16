using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHistory_AspNetUsers_UserId",
                table: "UserHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHistory_LibraryItemCopies_LibraryItemCopyId",
                table: "UserHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHistory",
                table: "UserHistory");

            migrationBuilder.RenameTable(
                name: "UserHistory",
                newName: "UserHistories");

            migrationBuilder.RenameIndex(
                name: "IX_UserHistory_UserId",
                table: "UserHistories",
                newName: "IX_UserHistories_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHistory_LibraryItemCopyId",
                table: "UserHistories",
                newName: "IX_UserHistories_LibraryItemCopyId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "LibraryItems",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Edition",
                table: "LibraryItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ISBN_10",
                table: "LibraryItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ISBN_13",
                table: "LibraryItems",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "LibraryItems",
                type: "numeric",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReceiptDate",
                table: "LibraryItems",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "UserHistories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OperationType",
                table: "UserHistories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHistories",
                table: "UserHistories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistories_AspNetUsers_UserId",
                table: "UserHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistories_LibraryItemCopies_LibraryItemCopyId",
                table: "UserHistories",
                column: "LibraryItemCopyId",
                principalTable: "LibraryItemCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHistories_AspNetUsers_UserId",
                table: "UserHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHistories_LibraryItemCopies_LibraryItemCopyId",
                table: "UserHistories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHistories",
                table: "UserHistories");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "LibraryItems");

            migrationBuilder.DropColumn(
                name: "Edition",
                table: "LibraryItems");

            migrationBuilder.DropColumn(
                name: "ISBN_10",
                table: "LibraryItems");

            migrationBuilder.DropColumn(
                name: "ISBN_13",
                table: "LibraryItems");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "LibraryItems");

            migrationBuilder.DropColumn(
                name: "ReceiptDate",
                table: "LibraryItems");

            migrationBuilder.DropColumn(
                name: "OperationType",
                table: "UserHistories");

            migrationBuilder.RenameTable(
                name: "UserHistories",
                newName: "UserHistory");

            migrationBuilder.RenameIndex(
                name: "IX_UserHistories_UserId",
                table: "UserHistory",
                newName: "IX_UserHistory_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserHistories_LibraryItemCopyId",
                table: "UserHistory",
                newName: "IX_UserHistory_LibraryItemCopyId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "UserHistory",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHistory",
                table: "UserHistory",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistory_AspNetUsers_UserId",
                table: "UserHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistory_LibraryItemCopies_LibraryItemCopyId",
                table: "UserHistory",
                column: "LibraryItemCopyId",
                principalTable: "LibraryItemCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
