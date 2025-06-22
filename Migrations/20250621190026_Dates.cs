using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class Dates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "Expenditures");

            migrationBuilder.RenameColumn(
                name: "Amount",
                table: "PartnerSettlements",
                newName: "Amount_Percentage");

            migrationBuilder.CreateTable(
                name: "TechnicianSettlements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnicianId = table.Column<int>(type: "int", nullable: false),
                    TechnicianName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalFromReservations = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalFromInsurance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDebtShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalExpenses = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TechnicianSettlements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TechnicianSettlements_Technicians_TechnicianId",
                        column: x => x.TechnicianId,
                        principalTable: "Technicians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TechnicianSettlementViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TechnicianName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalFromReservations = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalFromInsurance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDebtShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalExpenses = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetPayable = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
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
                    TechnicianShare = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TechnicianSettlementViewModelId = table.Column<int>(type: "int", nullable: true)
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

            migrationBuilder.CreateIndex(
                name: "IX_TechnicianSettlements_TechnicianId",
                table: "TechnicianSettlements",
                column: "TechnicianId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InsuranceBreakdown");

            migrationBuilder.DropTable(
                name: "TechnicianSettlements");

            migrationBuilder.DropTable(
                name: "TechnicianSettlementViewModel");

            migrationBuilder.RenameColumn(
                name: "Amount_Percentage",
                table: "PartnerSettlements",
                newName: "Amount");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Expenditures",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
