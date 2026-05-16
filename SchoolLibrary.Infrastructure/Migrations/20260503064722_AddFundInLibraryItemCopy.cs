using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFundInLibraryItemCopy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "FundId",
                table: "LibraryItemCopies",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateIndex(
                name: "IX_LibraryItemCopies_FundId",
                table: "LibraryItemCopies",
                column: "FundId");

            migrationBuilder.AddForeignKey(
                name: "FK_LibraryItemCopies_Funds_FundId",
                table: "LibraryItemCopies",
                column: "FundId",
                principalTable: "Funds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LibraryItemCopies_Funds_FundId",
                table: "LibraryItemCopies");

            migrationBuilder.DropIndex(
                name: "IX_LibraryItemCopies_FundId",
                table: "LibraryItemCopies");

            migrationBuilder.DropColumn(
                name: "FundId",
                table: "LibraryItemCopies");
        }
    }
}
