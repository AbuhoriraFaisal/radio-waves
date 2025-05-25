using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class secaadpatienttodb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Debts_Patient_PatientId",
                table: "Debts");

            migrationBuilder.DropForeignKey(
                name: "FK_Patient_Insurances_InsuranceId",
                table: "Patient");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Patient_PatientId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Patient_PatientId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patient",
                table: "Patient");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "Debts");

            migrationBuilder.RenameTable(
                name: "Patient",
                newName: "Patients");

            migrationBuilder.RenameIndex(
                name: "IX_Patient_InsuranceId",
                table: "Patients",
                newName: "IX_Patients_InsuranceId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patients",
                table: "Patients",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Debts_Patients_PatientId",
                table: "Debts",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Insurances_InsuranceId",
                table: "Patients",
                column: "InsuranceId",
                principalTable: "Insurances",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Patients_PatientId",
                table: "Payment",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Patients_PatientId",
                table: "Reservations",
                column: "PatientId",
                principalTable: "Patients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Debts_Patients_PatientId",
                table: "Debts");

            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Insurances_InsuranceId",
                table: "Patients");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Patients_PatientId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Patients_PatientId",
                table: "Reservations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Patients",
                table: "Patients");

            migrationBuilder.RenameTable(
                name: "Patients",
                newName: "Patient");

            migrationBuilder.RenameIndex(
                name: "IX_Patients_InsuranceId",
                table: "Patient",
                newName: "IX_Patient_InsuranceId");

            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "Debts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Patient",
                table: "Patient",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Debts_Patient_PatientId",
                table: "Debts",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Patient_Insurances_InsuranceId",
                table: "Patient",
                column: "InsuranceId",
                principalTable: "Insurances",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Patient_PatientId",
                table: "Payment",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Patient_PatientId",
                table: "Reservations",
                column: "PatientId",
                principalTable: "Patient",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
