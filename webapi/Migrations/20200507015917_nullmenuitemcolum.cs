using Microsoft.EntityFrameworkCore.Migrations;

namespace webapi.Migrations
{
    public partial class nullmenuitemcolum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemRelation_MenuItem_MenuItemFatherId",
                table: "MenuItemRelation");

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemFatherId",
                table: "MenuItemRelation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemRelation_MenuItem_MenuItemFatherId",
                table: "MenuItemRelation",
                column: "MenuItemFatherId",
                principalTable: "MenuItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MenuItemRelation_MenuItem_MenuItemFatherId",
                table: "MenuItemRelation");

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemFatherId",
                table: "MenuItemRelation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MenuItemRelation_MenuItem_MenuItemFatherId",
                table: "MenuItemRelation",
                column: "MenuItemFatherId",
                principalTable: "MenuItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
