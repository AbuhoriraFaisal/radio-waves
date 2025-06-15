using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class Innsurance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Insurances_InsuranceId1",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_InsuranceId1",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "InsuranceId1",
                table: "Patients");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_InsuranceId",
                table: "Patients",
                column: "InsuranceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_InsuranceCompanies_InsuranceId",
                table: "Patients",
                column: "InsuranceId",
                principalTable: "InsuranceCompanies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_InsuranceCompanies_InsuranceId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_InsuranceId",
                table: "Patients");

            migrationBuilder.AddColumn<int>(
                name: "InsuranceId1",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Patients_InsuranceId1",
                table: "Patients",
                column: "InsuranceId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Insurances_InsuranceId1",
                table: "Patients",
                column: "InsuranceId1",
                principalTable: "Insurances",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
