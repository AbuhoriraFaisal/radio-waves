using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class InitClean7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Debts_TechnicianId",
                table: "Debts",
                column: "TechnicianId");

            migrationBuilder.AddForeignKey(
                name: "FK_Debts_Technicians_TechnicianId",
                table: "Debts",
                column: "TechnicianId",
                principalTable: "Technicians",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Debts_Technicians_TechnicianId",
                table: "Debts");

            migrationBuilder.DropIndex(
                name: "IX_Debts_TechnicianId",
                table: "Debts");
        }
    }
}
