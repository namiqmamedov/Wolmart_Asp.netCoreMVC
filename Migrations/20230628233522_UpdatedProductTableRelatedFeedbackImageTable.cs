using Microsoft.EntityFrameworkCore.Migrations;

namespace Wolmart.Ecommerce.Migrations
{
    public partial class UpdatedProductTableRelatedFeedbackImageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserID",
                table: "FeedbackImages",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackImages_AppUserID",
                table: "FeedbackImages",
                column: "AppUserID");

            migrationBuilder.AddForeignKey(
                name: "FK_FeedbackImages_AspNetUsers_AppUserID",
                table: "FeedbackImages",
                column: "AppUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FeedbackImages_AspNetUsers_AppUserID",
                table: "FeedbackImages");

            migrationBuilder.DropIndex(
                name: "IX_FeedbackImages_AppUserID",
                table: "FeedbackImages");

            migrationBuilder.DropColumn(
                name: "AppUserID",
                table: "FeedbackImages");
        }
    }
}
