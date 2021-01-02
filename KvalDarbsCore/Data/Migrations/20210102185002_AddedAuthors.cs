using Microsoft.EntityFrameworkCore.Migrations;

namespace KvalDarbsCore.Data.Migrations
{
    public partial class AddedAuthors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Exercises",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Competitions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_AuthorId",
                table: "Exercises",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Competitions_AuthorId",
                table: "Competitions",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Competitions_AspNetUsers_AuthorId",
                table: "Competitions",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_AspNetUsers_AuthorId",
                table: "Exercises",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Competitions_AspNetUsers_AuthorId",
                table: "Competitions");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_AspNetUsers_AuthorId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_AuthorId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Competitions_AuthorId",
                table: "Competitions");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Competitions");
        }
    }
}
