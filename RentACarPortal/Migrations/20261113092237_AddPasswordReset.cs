using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentACarPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordReset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpiresAt",
                table: "Companies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetTokenHash",
                table: "Companies",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpiresAt",
                table: "Clients",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResetTokenHash",
                table: "Clients",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetTokenExpiresAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ResetTokenHash",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpiresAt",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "ResetTokenHash",
                table: "Clients");
        }
    }
}
