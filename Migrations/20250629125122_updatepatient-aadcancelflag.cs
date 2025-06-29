using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class updatepatientaadcancelflag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Insurances",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                table: "Debts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Insurances");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                table: "Debts");
        }
    }
}
