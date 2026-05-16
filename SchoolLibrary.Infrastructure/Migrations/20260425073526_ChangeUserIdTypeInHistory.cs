using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUserIdTypeInHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHistory_AspNetUsers_UserId1",
                table: "UserHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserHistory_UserId1",
                table: "UserHistory");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "UserHistory");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserHistory",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_UserHistory_UserId",
                table: "UserHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistory_AspNetUsers_UserId",
                table: "UserHistory",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHistory_AspNetUsers_UserId",
                table: "UserHistory");

            migrationBuilder.DropIndex(
                name: "IX_UserHistory_UserId",
                table: "UserHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "UserHistory",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "UserHistory",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_UserHistory_UserId1",
                table: "UserHistory",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistory_AspNetUsers_UserId1",
                table: "UserHistory",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
