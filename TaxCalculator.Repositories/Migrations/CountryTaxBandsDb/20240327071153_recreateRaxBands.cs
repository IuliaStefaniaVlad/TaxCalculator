using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxCalculator.Repositories.Migrations.CountryTaxBandsDb
{
    /// <inheritdoc />
    public partial class recreateRaxBands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountryTaxBands",
                columns: table => new
                {
                    CountryTaxBandsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryTaxBands", x => x.CountryTaxBandsId);
                });

            migrationBuilder.CreateTable(
                name: "TaxBands",
                columns: table => new
                {
                    TaxBandId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BandOrder = table.Column<int>(type: "int", nullable: false),
                    MinRange = table.Column<int>(type: "int", nullable: false),
                    MaxRange = table.Column<int>(type: "int", nullable: true),
                    TaxRate = table.Column<int>(type: "int", nullable: false),
                    CountryTaxBandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxBands", x => x.TaxBandId);
                    table.ForeignKey(
                        name: "FK_TaxBands_CountryTaxBands_CountryTaxBandId",
                        column: x => x.CountryTaxBandId,
                        principalTable: "CountryTaxBands",
                        principalColumn: "CountryTaxBandsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TaxBands_CountryTaxBandId",
                table: "TaxBands",
                column: "CountryTaxBandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaxBands");

            migrationBuilder.DropTable(
                name: "CountryTaxBands");
        }
    }
}
