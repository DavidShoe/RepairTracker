using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepairTracker.Migrations
{
    /// <inheritdoc />
    public partial class AddRepairDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepairDate",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "RepairNotes",
                table: "Repairs");

            migrationBuilder.AddColumn<DateOnly>(
                name: "FinishedDate",
                table: "Repairs",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ReceivedDate",
                table: "Repairs",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Repairs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RepairNote",
                columns: table => new
                {
                    RepairNoteID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairID = table.Column<int>(type: "int", nullable: false),
                    NoteDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepairNote", x => x.RepairNoteID);
                    table.ForeignKey(
                        name: "FK_RepairNote_Repairs_RepairID",
                        column: x => x.RepairID,
                        principalTable: "Repairs",
                        principalColumn: "RepairID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RepairNote_RepairID",
                table: "RepairNote",
                column: "RepairID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RepairNote");

            migrationBuilder.DropColumn(
                name: "FinishedDate",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "ReceivedDate",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Repairs");

            migrationBuilder.AddColumn<DateTime>(
                name: "RepairDate",
                table: "Repairs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RepairNotes",
                table: "Repairs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
