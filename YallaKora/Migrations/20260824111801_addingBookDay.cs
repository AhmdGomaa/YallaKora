using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YallaKora.Migrations
{
    /// <inheritdoc />
    public partial class addingBookDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookingDay",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingDay",
                table: "Bookings");
        }
    }
}
