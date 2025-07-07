using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SnapManager.Data.DesignTime.Migrations.NpgsqlServer
{
    /// <inheritdoc />
    public partial class FoldersWereRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credentials_Folder_FolderId",
                table: "Credentials");

            migrationBuilder.DropTable(
                name: "Folder");

            migrationBuilder.DropIndex(
                name: "IX_Credentials_FolderId",
                table: "Credentials");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Credentials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FolderId",
                table: "Credentials",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Folder",
                columns: table => new
                {
                    FolderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    ModificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folder", x => x.FolderId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Credentials_FolderId",
                table: "Credentials",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Credentials_Folder_FolderId",
                table: "Credentials",
                column: "FolderId",
                principalTable: "Folder",
                principalColumn: "FolderId");
        }
    }
}
