using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace radio_waves.Migrations
{
    /// <inheritdoc />
    public partial class InitClean2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount_Percentage",
                table: "Partners");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "Partners",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Comment",
                table: "Partners");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount_Percentage",
                table: "Partners",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
