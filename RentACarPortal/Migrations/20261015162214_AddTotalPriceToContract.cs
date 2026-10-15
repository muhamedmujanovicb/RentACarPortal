using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddTotalPriceToContract : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TotalPrice",
                table: "Contracts",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "Contracts");
        }
    }
}
