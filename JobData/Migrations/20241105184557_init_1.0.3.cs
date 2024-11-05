using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobData.Migrations
{
    /// <inheritdoc />
    public partial class init_103 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employers_JobProfileId",
                table: "Employers");

            migrationBuilder.CreateIndex(
                name: "IX_Employers_JobProfileId",
                table: "Employers",
                column: "JobProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Employers_JobProfileId",
                table: "Employers");

            migrationBuilder.CreateIndex(
                name: "IX_Employers_JobProfileId",
                table: "Employers",
                column: "JobProfileId",
                unique: true);
        }
    }
}
