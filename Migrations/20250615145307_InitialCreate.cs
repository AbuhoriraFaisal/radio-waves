using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Insurances_InsuranceId",
                table: "Patients");

            migrationBuilder.DropIndex(
                name: "IX_Patients_InsuranceId",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "CoverageDetails",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "CoveragedPercentage",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "Provider",
                table: "Insurances");

            migrationBuilder.AddColumn<int>(
                name: "InsuranceId1",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "InsuranceAmount",
                table: "Insurances",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "IsComplete",
                table: "Insurances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "PaidAmount",
                table: "Insurances",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "PatientId",
                table: "Insurances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProviderId",
                table: "Insurances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReservationId",
                table: "Insurances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TechnicianId",
                table: "Insurances",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TechnicianShare",
                table: "Insurances",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "InsuranceCompanies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PolicyNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverageDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoveragedPercentage = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceCompanies", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Patients_Insurances_InsuranceId1",
                table: "Patients");

            migrationBuilder.DropTable(
                name: "InsuranceCompanies");

            migrationBuilder.DropIndex(
                name: "IX_Patients_InsuranceId1",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "InsuranceId1",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "InsuranceAmount",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "IsComplete",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "PaidAmount",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "ProviderId",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "ReservationId",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "TechnicianId",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "TechnicianShare",
                table: "Insurances");

            migrationBuilder.AddColumn<string>(
                name: "CoverageDetails",
                table: "Insurances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "CoveragedPercentage",
                table: "Insurances",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Provider",
                table: "Insurances",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_InsuranceId",
                table: "Patients",
                column: "InsuranceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Patients_Insurances_InsuranceId",
                table: "Patients",
                column: "InsuranceId",
                principalTable: "Insurances",
                principalColumn: "Id");
        }
    }
}
