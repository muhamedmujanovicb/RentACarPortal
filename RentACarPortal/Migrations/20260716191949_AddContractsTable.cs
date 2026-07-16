using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddContractsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    RentalStation = table.Column<string>(type: "TEXT", nullable: false),
                    TypeOfVehicle = table.Column<string>(type: "TEXT", nullable: false),
                    RegisterNumberOfVehicle = table.Column<string>(type: "TEXT", nullable: false),
                    RentDriver = table.Column<string>(type: "TEXT", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    Telephone = table.Column<string>(type: "TEXT", nullable: false),
                    PassportNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PlaceOfIssue = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfIssue = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    PersonalNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    DrivingLicenseNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    RentalStartDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    RentalStartTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    RentalStartPlace = table.Column<string>(type: "TEXT", nullable: false),
                    RentalEndDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    RentalEndTime = table.Column<TimeOnly>(type: "TEXT", nullable: false),
                    RentalEndPlace = table.Column<string>(type: "TEXT", nullable: false),
                    Insurance = table.Column<bool>(type: "INTEGER", nullable: false),
                    FuelRecieved = table.Column<string>(type: "TEXT", nullable: false),
                    FuelReturned = table.Column<string>(type: "TEXT", nullable: false),
                    FullTankSizeLiquid = table.Column<double>(type: "REAL", nullable: false),
                    Deposit = table.Column<double>(type: "REAL", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", nullable: false),
                    Remarks = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contracts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_UserId",
                table: "Contracts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Contracts");
        }
    }
}
