using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kutiyana_Memon_Hospital_Api.Migrations
{
    /// <inheritdoc />
    public partial class ModuletableUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Icon",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "Modules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Modules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Modules",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Modules_ParentId",
                table: "Modules",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Modules_Modules_ParentId",
                table: "Modules",
                column: "ParentId",
                principalTable: "Modules",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Modules_Modules_ParentId",
                table: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_Modules_ParentId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Icon",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Modules");
        }
    }
}
