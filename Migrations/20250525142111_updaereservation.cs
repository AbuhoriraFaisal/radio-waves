using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class updaereservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InsuranceId",
                table: "Reservations",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InsuranceId",
                table: "Reservations");
        }
    }
}
