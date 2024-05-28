using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TelcoGroup.Migrations
{
    /// <inheritdoc />
    public partial class AddedHeaders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Headers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlueText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GreenText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: true),
                    LanguageGroup = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Headers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Headers_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Headers_LanguageId",
                table: "Headers",
                column: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Headers");
        }
    }
}
