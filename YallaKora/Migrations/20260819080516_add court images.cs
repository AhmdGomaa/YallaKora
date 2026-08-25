using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaKora.Migrations
{
    /// <inheritdoc />
    public partial class addcourtimages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourtImage",
                table: "Courts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 1,
                column: "CourtImage",
                value: "");

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 2,
                column: "CourtImage",
                value: "");

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 3,
                column: "CourtImage",
                value: "");

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 4,
                column: "CourtImage",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourtImage",
                table: "Courts");
        }
    }
}
