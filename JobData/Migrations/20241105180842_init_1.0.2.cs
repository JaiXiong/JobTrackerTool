using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobData.Migrations
{
    /// <inheritdoc />
    public partial class init_102 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobProfileId",
                table: "Details",
                newName: "EmployerProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Details_EmployerProfileId",
                table: "Details",
                column: "EmployerProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Details_Employers_EmployerProfileId",
                table: "Details",
                column: "EmployerProfileId",
                principalTable: "Employers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Details_Employers_EmployerProfileId",
                table: "Details");

            migrationBuilder.DropIndex(
                name: "IX_Details_EmployerProfileId",
                table: "Details");

            migrationBuilder.RenameColumn(
                name: "EmployerProfileId",
                table: "Details",
                newName: "JobProfileId");
        }
    }
}
