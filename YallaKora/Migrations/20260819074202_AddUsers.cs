using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace YallaKora.Migrations
{
    /// <inheritdoc />
    public partial class AddUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 1,
                column: "CourtName",
                value: "Court 1");

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 2,
                column: "CourtName",
                value: "Court 2");

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 3,
                columns: new[] { "CourtName", "PricePerHour" },
                values: new object[] { "Court 3", 400m });

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 4,
                columns: new[] { "CourtName", "IsAvailable", "PricePerHour" },
                values: new object[] { "Court 4", false, 1000m });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "Age", "Email", "FirstName", "LastName", "Password", "PhoneNumber", "ProfileImage", "Role", "UserName", "UserPosition" },
                values: new object[,]
                {
                    { 1, "ringroad", 20, "ahmed@gmail.com", "Ahmed", "Gomaa", "1234", "01032455", "D:\\.NET Web diplome\\Advanced C#\\YallaKora\\YallaKora\\wwwroot\\ball-soccer-soccer-ball-1530417 (1).jpg", "Admin", "Ahmed Gomaa", "defender" },
                    { 2, "ringroad ", 45, "Ayman@gmail.com", "Ayman", "Refaat", "12345", "01032455", "D:\\.NET Web diplome\\Advanced C#\\YallaKora\\YallaKora\\wwwroot\\ball-soccer-soccer-ball-1530417 (1).jpg", "User", "Ayman_Refaat23", "defender" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2);

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
                columns: new[] { "CourtImage", "CourtName" },
                values: new object[] { "", "ملعب 1" });

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 2,
                columns: new[] { "CourtImage", "CourtName" },
                values: new object[] { "", "ملعب 2" });

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 3,
                columns: new[] { "CourtImage", "CourtName", "PricePerHour" },
                values: new object[] { "", "ملعب 3", 200m });

            migrationBuilder.UpdateData(
                table: "Courts",
                keyColumn: "CourtId",
                keyValue: 4,
                columns: new[] { "CourtImage", "CourtName", "IsAvailable", "PricePerHour" },
                values: new object[] { "", "ملعب 4", true, 200m });
        }
    }
}
