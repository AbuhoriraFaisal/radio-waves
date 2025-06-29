using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class updatepatient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_InsuranceCompanies_InsuranceId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Patients_PatientId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Payment_PatientId",
                table: "Payment");

            migrationBuilder.DropIndex(
                name: "IX_Patients_InsuranceId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Payment");

            migrationBuilder.DropColumn(
                name: "HasInsurance",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "InsuranceId",
                table: "Patients");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Payment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasInsurance",
                table: "Patients",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "InsuranceId",
                table: "Patients",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payment_PatientId",
                table: "Payment",
                column: "PatientId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Patients_PatientId",
                table: "Payment",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");
        }
    }
}
