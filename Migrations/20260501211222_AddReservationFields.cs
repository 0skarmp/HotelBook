using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Plate",
                table: "Reservations",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "GuestName",
                table: "Reservations",
                newName: "Passport");

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Reservations",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Reservations",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Reservations",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MiddleName",
                table: "Reservations",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Country",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "MiddleName",
                table: "Reservations");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Reservations",
                newName: "Plate");

            migrationBuilder.RenameColumn(
                name: "Passport",
                table: "Reservations",
                newName: "GuestName");
        }
    }
}
