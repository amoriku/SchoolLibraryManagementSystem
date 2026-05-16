using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SchoolLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ApplyConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryItemCopies_LibraryItems_LibraryItemId",
                table: "LibraryItemCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHistories_AspNetUsers_UserId",
                table: "UserHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHistories_LibraryItemCopies_LibraryItemCopyId",
                table: "UserHistories");

            migrationBuilder.AlterColumn<string>(
                name: "OperationType",
                table: "UserHistories",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "LibraryItemCopies",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateTable(
                name: "Funds",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funds", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryItemCopies_LibraryItems_LibraryItemId",
                table: "LibraryItemCopies",
                column: "LibraryItemId",
                principalTable: "LibraryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistories_AspNetUsers_UserId",
                table: "UserHistories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistories_LibraryItemCopies_LibraryItemCopyId",
                table: "UserHistories",
                column: "LibraryItemCopyId",
                principalTable: "LibraryItemCopies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryItemCopies_LibraryItems_LibraryItemId",
                table: "LibraryItemCopies");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHistories_AspNetUsers_UserId",
                table: "UserHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserHistories_LibraryItemCopies_LibraryItemCopyId",
                table: "UserHistories");

            migrationBuilder.DropTable(
                name: "Funds");

            migrationBuilder.AlterColumn<int>(
                name: "OperationType",
                table: "UserHistories",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "LibraryItemCopies",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryItemCopies_LibraryItems_LibraryItemId",
                table: "LibraryItemCopies",
                column: "LibraryItemId",
                principalTable: "LibraryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
    }
}
