using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarPortal.Migrations
{
    /// <inheritdoc />
    public partial class YourMigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Users_UserId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Users_UserId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "BookingContractRequests");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Vehicles");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Vehicles",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_UserId",
                table: "Vehicles",
                newName: "IX_Vehicles_CompanyId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Contracts",
                newName: "VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_UserId",
                table: "Contracts",
                newName: "IX_Contracts_VehicleId");

            migrationBuilder.AlterColumn<string>(
                name: "PersonalNumber",
                table: "Contracts",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "DrivingLicenseNumber",
                table: "Contracts",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "ClientId",
                table: "Contracts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Contracts",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ClientId",
                table: "Contracts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_CompanyId",
                table: "Contracts",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Clients_ClientId",
                table: "Contracts",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Companies_CompanyId",
                table: "Contracts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Vehicles_VehicleId",
                table: "Contracts",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Companies_CompanyId",
                table: "Vehicles",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Clients_ClientId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Companies_CompanyId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contracts_Vehicles_VehicleId",
                table: "Contracts");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Companies_CompanyId",
                table: "Vehicles");

            migrationBuilder.DropTable(
                name: "Clients");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_ClientId",
                table: "Contracts");

            migrationBuilder.DropIndex(
                name: "IX_Contracts_CompanyId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Vehicles",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehicles_CompanyId",
                table: "Vehicles",
                newName: "IX_Vehicles_UserId");

            migrationBuilder.RenameColumn(
                name: "VehicleId",
                table: "Contracts",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Contracts_VehicleId",
                table: "Contracts",
                newName: "IX_Contracts_UserId");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Vehicles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PersonalNumber",
                table: "Contracts",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<int>(
                name: "DrivingLicenseNumber",
                table: "Contracts",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateTable(
                name: "BookingContractRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Address = table.Column<string>(type: "TEXT", nullable: false),
                    CompanyId = table.Column<string>(type: "TEXT", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    DateOfIssue = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    DriverFullName = table.Column<string>(type: "TEXT", nullable: false),
                    DrivingLicenseNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", nullable: false),
                    PassportNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PersonalIdNumber = table.Column<string>(type: "TEXT", nullable: false),
                    PlaceOfIssue = table.Column<string>(type: "TEXT", nullable: false),
                    RentLenght = table.Column<int>(type: "INTEGER", nullable: false),
                    RentStartDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Telephone = table.Column<string>(type: "TEXT", nullable: false),
                    TotalPrice = table.Column<double>(type: "REAL", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingContractRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BookingContractRequests_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Username = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BookingContractRequests_VehicleId",
                table: "BookingContractRequests",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contracts_Users_UserId",
                table: "Contracts",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Users_UserId",
                table: "Vehicles",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
