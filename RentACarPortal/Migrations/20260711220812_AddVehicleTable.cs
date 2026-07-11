using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Make = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    NumberOfSeats = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDiesel = table.Column<bool>(type: "INTEGER", nullable: false),
                    FuelEfficiency = table.Column<double>(type: "REAL", nullable: false),
                    FuelTankSize = table.Column<double>(type: "REAL", nullable: false),
                    TypeOfVehicle = table.Column<string>(type: "TEXT", nullable: false),
                    HasChildrenSafety = table.Column<bool>(type: "INTEGER", nullable: false),
                    DriveTerrain = table.Column<string>(type: "TEXT", nullable: false),
                    BootSpace = table.Column<string>(type: "TEXT", nullable: false),
                    ACtype = table.Column<string>(type: "TEXT", nullable: false),
                    HasNavigation = table.Column<bool>(type: "INTEGER", nullable: false),
                    DailyRate = table.Column<double>(type: "REAL", nullable: false),
                    HasInsurance = table.Column<bool>(type: "INTEGER", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    RegisterNumberOfVehicle = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_UserId",
                table: "Vehicles",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
