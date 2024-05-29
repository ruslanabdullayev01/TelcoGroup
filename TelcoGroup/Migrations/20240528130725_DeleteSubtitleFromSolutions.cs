using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelcoGroup.Migrations
{
    /// <inheritdoc />
    public partial class DeleteSubtitleFromSolutions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubTitle",
                table: "Solutions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubTitle",
                table: "Solutions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
