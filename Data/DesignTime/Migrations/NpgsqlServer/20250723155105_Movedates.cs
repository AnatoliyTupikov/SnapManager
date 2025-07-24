using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnapManager.Data.DesignTime.Migrations.NpgsqlServer
{
    /// <inheritdoc />
    public partial class Movedates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "folders");

            migrationBuilder.DropColumn(
                name: "ModificationDate",
                table: "folders");

            migrationBuilder.DropColumn(
                name: "CreationDateUTC",
                table: "credentials");

            migrationBuilder.DropColumn(
                name: "ModificationDateUTC",
                table: "credentials");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateUTC",
                table: "hierarchy_scheme",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDateUTC",
                table: "hierarchy_scheme",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreationDateUTC",
                table: "hierarchy_scheme");

            migrationBuilder.DropColumn(
                name: "ModificationDateUTC",
                table: "hierarchy_scheme");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "folders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDate",
                table: "folders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDateUTC",
                table: "credentials",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ModificationDateUTC",
                table: "credentials",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
