using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobData.Migrations
{
    /// <inheritdoc />
    public partial class init_104 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "JobActions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LatestUpdate",
                table: "JobActions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Employers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "LatestUpdate",
                table: "Employers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "JobActions");

            migrationBuilder.DropColumn(
                name: "LatestUpdate",
                table: "JobActions");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Employers");

            migrationBuilder.DropColumn(
                name: "LatestUpdate",
                table: "Employers");
        }
    }
}
