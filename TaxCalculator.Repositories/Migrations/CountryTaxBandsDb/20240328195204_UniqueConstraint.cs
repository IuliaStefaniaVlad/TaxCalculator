using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaxCalculator.Repositories.Migrations.CountryTaxBandsDb
{
    /// <inheritdoc />
    public partial class UniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "CountryTaxBands",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_CountryTaxBands_Country",
                table: "CountryTaxBands",
                column: "Country",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CountryTaxBands_Country",
                table: "CountryTaxBands");

            migrationBuilder.AlterColumn<string>(
                name: "Country",
                table: "CountryTaxBands",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
