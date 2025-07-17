using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ForkliftHub.Migrations
{
    /// <inheritdoc />
    public partial class FixingNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_MachineModels_ModelId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "ModelId",
                table: "Products",
                newName: "MachineModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_ModelId",
                table: "Products",
                newName: "IX_Products_MachineModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MachineModels_MachineModelId",
                table: "Products",
                column: "MachineModelId",
                principalTable: "MachineModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_MachineModels_MachineModelId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "MachineModelId",
                table: "Products",
                newName: "ModelId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_MachineModelId",
                table: "Products",
                newName: "IX_Products_ModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_MachineModels_ModelId",
                table: "Products",
                column: "ModelId",
                principalTable: "MachineModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
