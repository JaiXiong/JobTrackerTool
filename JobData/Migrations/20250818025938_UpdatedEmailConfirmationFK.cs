using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobData.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEmailConfirmationFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "token",
                table: "EmailConfirmations",
                newName: "Token");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "EmailConfirmations",
                newName: "UserProfileId");

            // Clean up orphaned EmailConfirmation records before creating foreign key
            migrationBuilder.Sql(@"
                DELETE FROM EmailConfirmations 
                WHERE UserProfileId NOT IN (SELECT Id FROM UserProfiles)
            ");

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfirmations_UserProfileId",
                table: "EmailConfirmations",
                column: "UserProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmailConfirmations_UserProfiles_UserProfileId",
                table: "EmailConfirmations",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailConfirmations_UserProfiles_UserProfileId",
                table: "EmailConfirmations");

            migrationBuilder.DropIndex(
                name: "IX_EmailConfirmations_UserProfileId",
                table: "EmailConfirmations");

            migrationBuilder.RenameColumn(
                name: "Token",
                table: "EmailConfirmations",
                newName: "token");

            migrationBuilder.RenameColumn(
                name: "UserProfileId",
                table: "EmailConfirmations",
                newName: "UserId");
        }
    }
}
