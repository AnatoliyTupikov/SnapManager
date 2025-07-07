using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SnapManager.Data.DesignTime.Migrations.NpgsqlServer
{
    /// <inheritdoc />
    public partial class NewScheme : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CredentialId",
                table: "Credentials",
                newName: "Id");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Credentials",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "TreeItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Folders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModificationDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Folders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Folders_TreeItems_Id",
                        column: x => x.Id,
                        principalTable: "TreeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TreeItemBaseTreeItemBase",
                columns: table => new
                {
                    ChildrenId = table.Column<int>(type: "integer", nullable: false),
                    ParentsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeItemBaseTreeItemBase", x => new { x.ChildrenId, x.ParentsId });
                    table.ForeignKey(
                        name: "FK_TreeItemBaseTreeItemBase_TreeItems_ChildrenId",
                        column: x => x.ChildrenId,
                        principalTable: "TreeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreeItemBaseTreeItemBase_TreeItems_ParentsId",
                        column: x => x.ParentsId,
                        principalTable: "TreeItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoldersWithCredentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoldersWithCredentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoldersWithCredentials_Folders_Id",
                        column: x => x.Id,
                        principalTable: "Folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreeItemBaseTreeItemBase_ParentsId",
                table: "TreeItemBaseTreeItemBase",
                column: "ParentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Credentials_TreeItems_Id",
                table: "Credentials",
                column: "Id",
                principalTable: "TreeItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credentials_TreeItems_Id",
                table: "Credentials");

            migrationBuilder.DropTable(
                name: "FoldersWithCredentials");

            migrationBuilder.DropTable(
                name: "TreeItemBaseTreeItemBase");

            migrationBuilder.DropTable(
                name: "Folders");

            migrationBuilder.DropTable(
                name: "TreeItems");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Credentials",
                newName: "CredentialId");

            migrationBuilder.AlterColumn<int>(
                name: "CredentialId",
                table: "Credentials",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
