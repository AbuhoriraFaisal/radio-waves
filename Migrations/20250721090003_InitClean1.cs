using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class InitClean1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartnerServices_Partners_PartnerId1",
                table: "PartnerServices");

            migrationBuilder.DropIndex(
                name: "IX_PartnerServices_PartnerId1",
                table: "PartnerServices");

            migrationBuilder.DropColumn(
                name: "PartnerId1",
                table: "PartnerServices");

            migrationBuilder.AlterColumn<int>(
                name: "PartnerId",
                table: "PartnerServices",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount_Percentage",
                table: "PartnerServices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_PartnerServices_PartnerId",
                table: "PartnerServices",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerServices_Partners_PartnerId",
                table: "PartnerServices",
                column: "PartnerId",
                principalTable: "Partners",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartnerServices_Partners_PartnerId",
                table: "PartnerServices");

            migrationBuilder.DropIndex(
                name: "IX_PartnerServices_PartnerId",
                table: "PartnerServices");

            migrationBuilder.DropColumn(
                name: "Amount_Percentage",
                table: "PartnerServices");

            migrationBuilder.AlterColumn<decimal>(
                name: "PartnerId",
                table: "PartnerServices",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PartnerId1",
                table: "PartnerServices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PartnerServices_PartnerId1",
                table: "PartnerServices",
                column: "PartnerId1");

            migrationBuilder.AddForeignKey(
                name: "FK_PartnerServices_Partners_PartnerId1",
                table: "PartnerServices",
                column: "PartnerId1",
                principalTable: "Partners",
                principalColumn: "Id");
        }
    }
}
