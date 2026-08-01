using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarPortal.Migrations
{
    /// <inheritdoc />
    public partial class ModelsCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DriverFullName",
                table: "Contracts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RentLenght",
                table: "BookingContractRequests",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "RentStartDate",
                table: "BookingContractRequests",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverFullName",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "RentLenght",
                table: "BookingContractRequests");

            migrationBuilder.DropColumn(
                name: "RentStartDate",
                table: "BookingContractRequests");
        }
    }
}
