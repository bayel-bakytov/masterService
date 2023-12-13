using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterServiceBack.Migrations
{
    public partial class cli : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Login",
                table: "Clients",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Login",
                table: "Clients");
        }
    }
}
