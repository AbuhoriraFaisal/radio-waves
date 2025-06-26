using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class pendingissues : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsuranceBreakdown");

            migrationBuilder.DropTable(
                name: "InsuranceCompanySettlementViewModel");

            migrationBuilder.DropTable(
                name: "TechnicianSettlementViewModel");

            migrationBuilder.AddColumn<DateTime>(
                name: "SettelmentDate",
                table: "TechnicianSettlements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "InsuranceCompanySettlements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsuranceCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalInsuranceShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SettlementDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceCompanySettlements", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsuranceCompanySettlements");

            migrationBuilder.DropColumn(
                name: "SettelmentDate",
                table: "TechnicianSettlements");

            migrationBuilder.CreateTable(
                name: "InsuranceCompanySettlementViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsuranceCompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NetPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SettlementDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalInsuranceShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceCompanySettlementViewModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TechnicianSettlementViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NetPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SettelmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TechnicianName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalDebtShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalExpenses = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalFromInsurance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalFromReservations = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicianSettlementViewModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InsuranceBreakdown",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InsuranceCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TechnicianSettlementViewModelId = table.Column<int>(type: "int", nullable: true),
                    TechnicianShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InsuranceBreakdown", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InsuranceBreakdown_TechnicianSettlementViewModel_TechnicianSettlementViewModelId",
                        column: x => x.TechnicianSettlementViewModelId,
                        principalTable: "TechnicianSettlementViewModel",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_InsuranceBreakdown_TechnicianSettlementViewModelId",
                table: "InsuranceBreakdown",
                column: "TechnicianSettlementViewModelId");
        }
    }
}
