using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepairTracker.Migrations
{
    /// <inheritdoc />
    public partial class FixWallTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairNote_Repairs_RepairID",
                table: "RepairNote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RepairNote",
                table: "RepairNote");

            migrationBuilder.RenameTable(
                name: "RepairNote",
                newName: "RepairNotes");

            migrationBuilder.RenameIndex(
                name: "IX_RepairNote_RepairID",
                table: "RepairNotes",
                newName: "IX_RepairNotes_RepairID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RepairNotes",
                table: "RepairNotes",
                column: "RepairNoteID");

            migrationBuilder.CreateTable(
                name: "WallTimes",
                columns: table => new
                {
                    WallTimeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairID = table.Column<int>(type: "int", nullable: false),
                    TechnicianID = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WallTimes", x => x.WallTimeID);
                    table.ForeignKey(
                        name: "FK_WallTimes_Repairs_RepairID",
                        column: x => x.RepairID,
                        principalTable: "Repairs",
                        principalColumn: "RepairID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WallTimes_RepairID",
                table: "WallTimes",
                column: "RepairID");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairNotes_Repairs_RepairID",
                table: "RepairNotes",
                column: "RepairID",
                principalTable: "Repairs",
                principalColumn: "RepairID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RepairNotes_Repairs_RepairID",
                table: "RepairNotes");

            migrationBuilder.DropTable(
                name: "WallTimes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RepairNotes",
                table: "RepairNotes");

            migrationBuilder.RenameTable(
                name: "RepairNotes",
                newName: "RepairNote");

            migrationBuilder.RenameIndex(
                name: "IX_RepairNotes_RepairID",
                table: "RepairNote",
                newName: "IX_RepairNote_RepairID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RepairNote",
                table: "RepairNote",
                column: "RepairNoteID");

            migrationBuilder.AddForeignKey(
                name: "FK_RepairNote_Repairs_RepairID",
                table: "RepairNote",
                column: "RepairID",
                principalTable: "Repairs",
                principalColumn: "RepairID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
