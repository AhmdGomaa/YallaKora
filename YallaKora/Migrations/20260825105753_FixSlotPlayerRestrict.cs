using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaKora.Migrations
{
    /// <inheritdoc />
    public partial class FixSlotPlayerRestrict : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_FromUserId",
                table: "Reviews");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_FromUserId",
                table: "Reviews",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Users_FromUserId",
                table: "Reviews");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Users_FromUserId",
                table: "Reviews",
                column: "FromUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
