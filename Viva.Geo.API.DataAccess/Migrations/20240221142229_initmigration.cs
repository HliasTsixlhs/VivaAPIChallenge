using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Viva.Geo.API.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class initmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Capital = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Borders",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    BorderCountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Borders", x => new { x.CountryId, x.BorderCountryId });
                    table.ForeignKey(
                        name: "FK_Borders_Countries_BorderCountryId",
                        column: x => x.BorderCountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Borders_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Borders_BorderCountryId",
                table: "Borders",
                column: "BorderCountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Borders");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
