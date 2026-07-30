using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleNavigationToBookingRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BookingContractRequests_VehicleId",
                table: "BookingContractRequests",
                column: "VehicleId");

            migrationBuilder.AddForeignKey(
                name: "FK_BookingContractRequests_Vehicles_VehicleId",
                table: "BookingContractRequests",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookingContractRequests_Vehicles_VehicleId",
                table: "BookingContractRequests");

            migrationBuilder.DropIndex(
                name: "IX_BookingContractRequests_VehicleId",
                table: "BookingContractRequests");
        }
    }
}
