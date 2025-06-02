using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class debtUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TechnicianId",
                table: "Debts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TechnicianShare",
                table: "Debts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TechnicianId",
                table: "Debts");

            migrationBuilder.DropColumn(
                name: "TechnicianShare",
                table: "Debts");
        }
    }
}
