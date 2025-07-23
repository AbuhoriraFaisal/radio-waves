using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class InitClean5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Insurances_PatientId",
                table: "Insurances",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Insurances_Patients_PatientId",
                table: "Insurances",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurances_Patients_PatientId",
                table: "Insurances");

            migrationBuilder.DropIndex(
                name: "IX_Insurances_PatientId",
                table: "Insurances");
        }
    }
}
