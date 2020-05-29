using Microsoft.EntityFrameworkCore.Migrations;

namespace MyDeckAPI.Migrations
{
    public partial class FixedSomeBugs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_Role_Name1",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Role_Name1",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role_Name1",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Role_Name",
                table: "Users",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role_Name",
                table: "Users",
                column: "Role_Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_Role_Name",
                table: "Users",
                column: "Role_Name",
                principalTable: "Roles",
                principalColumn: "Role_Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_Role_Name",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_Role_Name",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Role_Name",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role_Name1",
                table: "Users",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role_Name1",
                table: "Users",
                column: "Role_Name1");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_Role_Name1",
                table: "Users",
                column: "Role_Name1",
                principalTable: "Roles",
                principalColumn: "Role_Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
