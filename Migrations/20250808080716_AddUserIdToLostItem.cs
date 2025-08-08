using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LostAndFoundApp.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToLostItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "LostItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_LostItems_UserId",
                table: "LostItems",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LostItems_Users_UserId",
                table: "LostItems",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LostItems_Users_UserId",
                table: "LostItems");

            migrationBuilder.DropIndex(
                name: "IX_LostItems_UserId",
                table: "LostItems");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LostItems");
        }
    }
}
