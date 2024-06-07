using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Siyasett.Data.Data.Migrations
{
    public partial class migration_user1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndDateOfMembership",
                schema: "users",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserTypeId",
                schema: "users",
                table: "AspNetUsers",
                type: "integer",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDateOfMembership",
                schema: "users",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserTypeId",
                schema: "users",
                table: "AspNetUsers");
        }
    }
}
