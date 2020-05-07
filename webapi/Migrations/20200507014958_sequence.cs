using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.Migrations
{
    public partial class sequence : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sequence",
                table: "MenuItemRelation",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "sequence",
                table: "MenuItemRelation");
        }
    }
}
