using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Siyasett.Data.Data.Migrations
{
    public partial class AppUser_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Token",
                schema: "users",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                schema: "users",
                table: "AspNetUsers");
        }
    }
}
