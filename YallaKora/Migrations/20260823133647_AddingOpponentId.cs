using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaKora.Migrations
{
    /// <inheritdoc />
    public partial class AddingOpponentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.AddColumn<int>(
                name: "OpponentUserId",
                table: "Bookings",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OpponentUserId",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "MaxPlayers",
                table: "Slots",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
