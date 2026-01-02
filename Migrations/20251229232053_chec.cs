using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace briefit.Migrations
{
    /// <inheritdoc />
    public partial class chec : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortUrl",
                table: "ShortUrls",
                newName: "ShortCode");

            migrationBuilder.AddColumn<int>(
                name: "ClickCount",
                table: "ShortUrls",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastClickedAt",
                table: "ShortUrls",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClickCount",
                table: "ShortUrls");

            migrationBuilder.DropColumn(
                name: "LastClickedAt",
                table: "ShortUrls");

            migrationBuilder.RenameColumn(
                name: "ShortCode",
                table: "ShortUrls",
                newName: "ShortUrl");
        }
    }
}
