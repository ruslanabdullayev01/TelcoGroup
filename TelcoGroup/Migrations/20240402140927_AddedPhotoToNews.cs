﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelcoGroup.Migrations
{
    /// <inheritdoc />
    public partial class AddedPhotoToNews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "News",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "News");
        }
    }
}
