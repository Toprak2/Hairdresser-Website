using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hairdresser_Website.Migrations
{
    public partial class CreateAppointment : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the existing Status column
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Appointments");

            // Add the Status column again with the correct type (integer)
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Appointments",
                type: "integer",
                nullable: false,
                defaultValue: 0); // You can set a default value if needed
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverse the operations for rollback
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Appointments");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Appointments",
                type: "text",
                nullable: false);
        }
    }
}
