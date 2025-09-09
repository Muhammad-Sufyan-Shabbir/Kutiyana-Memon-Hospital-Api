using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kutiyana_Memon_Hospital_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddColumInUserresetToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetPasswordToken",
                table: "ApplicationUser",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpiry",
                table: "ApplicationUser",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetPasswordToken",
                table: "ApplicationUser");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpiry",
                table: "ApplicationUser");
        }
    }
}
