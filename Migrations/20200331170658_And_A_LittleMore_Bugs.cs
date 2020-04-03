using Microsoft.EntityFrameworkCore.Migrations;

namespace MyDeckAPI.Migrations
{
    public partial class And_A_LittleMore_Bugs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GoogleId",
                table: "Users",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GoogleId",
                table: "Users",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
