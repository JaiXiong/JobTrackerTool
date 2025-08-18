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

            // SAFE cleanup - only delete records where UserProfileId is NULL or doesn't exist
            // First, check if there are any orphaned records AFTER the column rename
            migrationBuilder.Sql(@"
                DELETE FROM EmailConfirmations 
                WHERE UserProfileId IS NULL 
                   OR UserProfileId NOT IN (SELECT Id FROM UserProfiles WHERE Id IS NOT NULL)
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
