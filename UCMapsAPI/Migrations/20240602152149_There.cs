using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UCMapsAPI.Migrations
{
    /// <inheritdoc />
    public partial class There : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StillThere",
                table: "Marker",
                newName: "StillThereCount");

            migrationBuilder.RenameColumn(
                name: "NotThere",
                table: "Marker",
                newName: "NotThereCount");

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarkerId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsStillThere = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votes_Marker_MarkerId",
                        column: x => x.MarkerId,
                        principalTable: "Marker",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Votes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Votes_MarkerId_UserId",
                table: "Votes",
                columns: new[] { "MarkerId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId",
                table: "Votes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.RenameColumn(
                name: "StillThereCount",
                table: "Marker",
                newName: "StillThere");

            migrationBuilder.RenameColumn(
                name: "NotThereCount",
                table: "Marker",
                newName: "NotThere");
        }
    }
}
