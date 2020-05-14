using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.Migrations
{
    public partial class song : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Song");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Song",
                newName: "Url");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Song",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Song",
                nullable: true);
        }
    }
}
