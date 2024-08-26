using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepairTracker.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Sale",
                table: "Parts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "Parts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sale",
                table: "Parts");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "Parts");
        }
    }
}
