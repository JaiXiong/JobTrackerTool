using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobTracker.API.Tool.Migrations
{
    /// <inheritdoc />
    public partial class init_102 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Test",
                table: "JobProfiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Test",
                table: "JobProfiles");
        }
    }
}
