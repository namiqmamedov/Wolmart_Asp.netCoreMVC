using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Wolmart.Ecommerce.Migrations
{
    public partial class UpdatedFeedbackImageTableAddedBaseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FeedbackImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "FeedbackImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "FeedbackImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "FeedbackImages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "FeedbackImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "FeedbackImages",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedBy",
                table: "FeedbackImages",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FeedbackImages");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "FeedbackImages");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "FeedbackImages");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "FeedbackImages");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "FeedbackImages");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "FeedbackImages");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "FeedbackImages");
        }
    }
}
