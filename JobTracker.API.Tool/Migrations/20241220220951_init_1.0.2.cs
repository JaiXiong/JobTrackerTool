using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobTracker.API.tool.Migrations
{
    /// <inheritdoc />
    public partial class init_102 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "UserProfiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "UserProfiles");
        }
    }
}
