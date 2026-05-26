using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpgradeGrade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Grades_Name",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Grades");

            migrationBuilder.AddColumn<string>(
                name: "Letter",
                table: "Grades",
                type: "character varying(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte>(
                name: "Number",
                table: "Grades",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Letter",
                table: "Grades");

            migrationBuilder.DropColumn(
                name: "Number",
                table: "Grades");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Grades",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Grades_Name",
                table: "Grades",
                column: "Name",
                unique: true);
        }
    }
}
