using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaKora.Migrations
{
    /// <inheritdoc />
    public partial class Adddecriptiontocourt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Courts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 1,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 2,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 3,
                column: "Description",
                value: "");

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 4,
                column: "Description",
                value: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Courts");
        }
    }
}
