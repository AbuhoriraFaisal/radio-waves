using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class NewMigrationName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Reservations",
                newName: "PaiedAmount");

            migrationBuilder.AddColumn<bool>(
                name: "IsCommission",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDebt",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCommission",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "IsDebt",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "PaiedAmount",
                table: "Reservations",
                newName: "TotalPrice");
        }
    }
}
