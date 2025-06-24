using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepairTracker.Migrations.GameRepair
{
    /// <inheritdoc />
    public partial class AddTechnicianIdentityUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Only add the IdentityUserId column to the existing Technicians table
            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Technicians",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the IdentityUserId column if rolling back
            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Technicians");
        }
    }
}
