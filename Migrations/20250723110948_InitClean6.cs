using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class InitClean6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProviderId",
                table: "Insurances",
                newName: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Insurances_CompanyId",
                table: "Insurances",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Insurances_InsuranceCompanies_CompanyId",
                table: "Insurances",
                column: "CompanyId",
                principalTable: "InsuranceCompanies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Insurances_InsuranceCompanies_CompanyId",
                table: "Insurances");

            migrationBuilder.DropIndex(
                name: "IX_Insurances_CompanyId",
                table: "Insurances");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Insurances",
                newName: "ProviderId");
        }
    }
}
