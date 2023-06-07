using Microsoft.EntityFrameworkCore.Migrations;

namespace Wolmart.Ecommerce.Migrations
{
    public partial class UpdatedCartsTableRelatedUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserID",
                table: "Carts",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Carts_AppUserID",
                table: "Carts",
                column: "AppUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Carts_AspNetUsers_AppUserID",
                table: "Carts",
                column: "AppUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Carts_AspNetUsers_AppUserID",
                table: "Carts");

            migrationBuilder.DropIndex(
                name: "IX_Carts_AppUserID",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "AppUserID",
                table: "Carts");
        }
    }
}
