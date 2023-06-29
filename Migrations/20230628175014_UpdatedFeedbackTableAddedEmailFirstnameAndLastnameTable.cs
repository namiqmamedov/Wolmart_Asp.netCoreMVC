using Microsoft.EntityFrameworkCore.Migrations;

namespace Wolmart.Ecommerce.Migrations
{
    public partial class UpdatedFeedbackTableAddedEmailFirstnameAndLastnameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Feedbacks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Firstname",
                table: "Feedbacks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Lastname",
                table: "Feedbacks",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "Firstname",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "Lastname",
                table: "Feedbacks");
        }
    }
}
