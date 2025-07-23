using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class InitClean8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RadiologyTypeId",
                table: "Expenditures",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenditures_RadiologyTypeId",
                table: "Expenditures",
                column: "RadiologyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Expenditures_RadiologyTypes_RadiologyTypeId",
                table: "Expenditures",
                column: "RadiologyTypeId",
                principalTable: "RadiologyTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expenditures_RadiologyTypes_RadiologyTypeId",
                table: "Expenditures");

            migrationBuilder.DropIndex(
                name: "IX_Expenditures_RadiologyTypeId",
                table: "Expenditures");

            migrationBuilder.DropColumn(
                name: "RadiologyTypeId",
                table: "Expenditures");
        }
    }
}
