using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelcoGroup.Migrations
{
    /// <inheritdoc />
    public partial class RemovePhotoInNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "News");

            migrationBuilder.AlterColumn<string>(
                name: "SubTitle",
                table: "Solutions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SubTitle",
                table: "Solutions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
