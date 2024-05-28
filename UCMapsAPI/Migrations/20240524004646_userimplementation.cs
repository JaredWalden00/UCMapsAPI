using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UCMapsAPI.Migrations
{
    /// <inheritdoc />
    public partial class userimplementation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Marker",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TokenInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TokenInfo", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Marker_UserId",
                table: "Marker",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Marker_Users_UserId",
                table: "Marker",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marker_Users_UserId",
                table: "Marker");

            migrationBuilder.DropTable(
                name: "TokenInfo");

            migrationBuilder.DropIndex(
                name: "IX_Marker_UserId",
                table: "Marker");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Marker");
        }
    }
}
