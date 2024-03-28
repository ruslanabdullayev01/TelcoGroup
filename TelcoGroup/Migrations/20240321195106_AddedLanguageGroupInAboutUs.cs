using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelcoGroup.Migrations
{
    /// <inheritdoc />
    public partial class AddedLanguageGroupInAboutUs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LanguageGroup",
                table: "AboutUs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "LanguageId",
                table: "AboutUs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AboutUs_LanguageId",
                table: "AboutUs",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_AboutUs_Languages_LanguageId",
                table: "AboutUs",
                column: "LanguageId",
                principalTable: "Languages",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AboutUs_Languages_LanguageId",
                table: "AboutUs");

            migrationBuilder.DropIndex(
                name: "IX_AboutUs_LanguageId",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "LanguageGroup",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "AboutUs");
        }
    }
}
