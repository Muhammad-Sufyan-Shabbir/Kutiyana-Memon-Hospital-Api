using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kutiyana_Memon_Hospital_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddColumInUserSalt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PasswordSalt",
                table: "ApplicationUser",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordSalt",
                table: "ApplicationUser");
        }
    }
}
