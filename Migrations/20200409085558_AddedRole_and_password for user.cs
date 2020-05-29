using Microsoft.EntityFrameworkCore.Migrations;

namespace MyDeckAPI.Migrations
{
    public partial class AddedRole_and_passwordforuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Category_Category_Name",
                table: "Decks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role_Name",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role_Name1",
                table: "Users",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Category_Name");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Role_Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Role_Name);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                column: "Role_Name",
                values: new object[]
                {
                    "Owner",
                    "Administrator",
                    "Support",
                    "Content Maker",
                    "User"
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role_Name1",
                table: "Users",
                column: "Role_Name1");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Categories_Category_Name",
                table: "Decks",
                column: "Category_Name",
                principalTable: "Categories",
                principalColumn: "Category_Name",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_Role_Name1",
                table: "Users",
                column: "Role_Name1",
                principalTable: "Roles",
                principalColumn: "Role_Name",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Decks_Categories_Category_Name",
                table: "Decks");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_Role_Name1",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Users_Role_Name1",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role_Name",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Role_Name1",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Category_Name");

            migrationBuilder.AddForeignKey(
                name: "FK_Decks_Category_Category_Name",
                table: "Decks",
                column: "Category_Name",
                principalTable: "Category",
                principalColumn: "Category_Name",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
