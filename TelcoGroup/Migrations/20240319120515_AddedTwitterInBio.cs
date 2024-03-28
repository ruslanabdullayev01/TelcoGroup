using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelcoGroup.Migrations
{
    /// <inheritdoc />
    public partial class AddedTwitterInBio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageGroup",
                table: "Solutions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "Solutions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Twitter",
                table: "Bios",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Solutions_LanguageId",
                table: "Solutions",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Solutions_Languages_LanguageId",
                table: "Solutions",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Solutions_Languages_LanguageId",
                table: "Solutions");

            migrationBuilder.DropIndex(
                name: "IX_Solutions_LanguageId",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "LanguageGroup",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Solutions");

            migrationBuilder.DropColumn(
                name: "Twitter",
                table: "Bios");
        }
    }
}
