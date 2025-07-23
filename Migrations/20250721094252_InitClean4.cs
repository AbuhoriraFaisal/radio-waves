using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class InitClean4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PartnerServices_PartnerId",
                table: "PartnerServices");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerServices_PartnerId_ServiceId",
                table: "PartnerServices",
                columns: new[] { "PartnerId", "ServiceId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PartnerServices_PartnerId_ServiceId",
                table: "PartnerServices");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerServices_PartnerId",
                table: "PartnerServices",
                column: "PartnerId");
        }
    }
}
