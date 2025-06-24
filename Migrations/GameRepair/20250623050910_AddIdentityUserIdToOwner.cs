using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepairTracker.Migrations.GameRepair
{
    /// <inheritdoc />
    public partial class AddIdentityUserIdToOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Owners");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "Owners",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId1",
                table: "Owners",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RepairTrackerUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairTrackerUser", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Owners_IdentityUserId",
                table: "Owners",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Owners_IdentityUserId1",
                table: "Owners",
                column: "IdentityUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_RepairTrackerUser_IdentityUserId",
                table: "Owners",
                column: "IdentityUserId",
                principalTable: "RepairTrackerUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Owners_RepairTrackerUser_IdentityUserId1",
                table: "Owners",
                column: "IdentityUserId1",
                principalTable: "RepairTrackerUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Owners_RepairTrackerUser_IdentityUserId",
                table: "Owners");

            migrationBuilder.DropForeignKey(
                name: "FK_Owners_RepairTrackerUser_IdentityUserId1",
                table: "Owners");

            migrationBuilder.DropTable(
                name: "RepairTrackerUser");

            migrationBuilder.DropIndex(
                name: "IX_Owners_IdentityUserId",
                table: "Owners");

            migrationBuilder.DropIndex(
                name: "IX_Owners_IdentityUserId1",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Owners");

            migrationBuilder.DropColumn(
                name: "IdentityUserId1",
                table: "Owners");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Owners",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
