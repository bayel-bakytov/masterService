using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterServiceBack.Migrations
{
    public partial class exec : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Executor",
                table: "Applications",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Executor",
                table: "Applications");
        }
    }
}
