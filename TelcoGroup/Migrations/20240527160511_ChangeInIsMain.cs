using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelcoGroup.Migrations
{
    /// <inheritdoc />
    public partial class ChangeInIsMain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "Solutions");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "News",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMain",
                table: "News");

            migrationBuilder.AddColumn<bool>(
                name: "IsMain",
                table: "Solutions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
