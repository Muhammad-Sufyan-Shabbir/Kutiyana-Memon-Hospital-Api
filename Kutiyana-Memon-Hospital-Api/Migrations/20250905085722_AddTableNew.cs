using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kutiyana_Memon_Hospital_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddTableNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuleName",
                table: "roleModuleAccess");

            migrationBuilder.AddColumn<int>(
                name: "ModuleId",
                table: "roleModuleAccess",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GlobalId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_roleModuleAccess_ModuleId",
                table: "roleModuleAccess",
                column: "ModuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_roleModuleAccess_Modules_ModuleId",
                table: "roleModuleAccess",
                column: "ModuleId",
                principalTable: "Modules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_roleModuleAccess_Modules_ModuleId",
                table: "roleModuleAccess");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropIndex(
                name: "IX_roleModuleAccess_ModuleId",
                table: "roleModuleAccess");

            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "roleModuleAccess");

            migrationBuilder.AddColumn<string>(
                name: "ModuleName",
                table: "roleModuleAccess",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
