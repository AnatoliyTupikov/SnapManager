using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SnapManager.Data.DesignTime.Migrations.NpgsqlServer
{
    /// <inheritdoc />
    public partial class SepModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Credentials_TreeItems_Id",
                table: "Credentials");

            migrationBuilder.DropForeignKey(
                name: "FK_Folders_TreeItems_Id",
                table: "Folders");

            migrationBuilder.DropTable(
                name: "FoldersWithCredentials");

            migrationBuilder.DropTable(
                name: "TreeItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Folders",
                table: "Folders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Credentials",
                table: "Credentials");

            migrationBuilder.RenameTable(
                name: "Folders",
                newName: "folders");

            migrationBuilder.RenameTable(
                name: "Credentials",
                newName: "credentials");

            migrationBuilder.AddPrimaryKey(
                name: "PK_folders",
                table: "folders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_credentials",
                table: "credentials",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "folders_with_credentials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_folders_with_credentials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_folders_with_credentials_folders_Id",
                        column: x => x.Id,
                        principalTable: "folders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "hierarchy_scheme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ParentId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_hierarchy_scheme", x => x.Id);
                    table.ForeignKey(
                        name: "FK_hierarchy_scheme_hierarchy_scheme_ParentId",
                        column: x => x.ParentId,
                        principalTable: "hierarchy_scheme",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_hierarchy_scheme_ParentId",
                table: "hierarchy_scheme",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_credentials_hierarchy_scheme_Id",
                table: "credentials",
                column: "Id",
                principalTable: "hierarchy_scheme",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_folders_hierarchy_scheme_Id",
                table: "folders",
                column: "Id",
                principalTable: "hierarchy_scheme",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_credentials_hierarchy_scheme_Id",
                table: "credentials");

            migrationBuilder.DropForeignKey(
                name: "FK_folders_hierarchy_scheme_Id",
                table: "folders");

            migrationBuilder.DropTable(
                name: "folders_with_credentials");

            migrationBuilder.DropTable(
                name: "hierarchy_scheme");

            migrationBuilder.DropPrimaryKey(
                name: "PK_folders",
                table: "folders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_credentials",
                table: "credentials");

            migrationBuilder.RenameTable(
                name: "folders",
                newName: "Folders");

            migrationBuilder.RenameTable(
                name: "credentials",
                newName: "Credentials");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Folders",
                table: "Folders",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Credentials",
                table: "Credentials",
                column: "Id");

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

            migrationBuilder.CreateTable(
                name: "TreeItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ParentId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreeItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TreeItems_TreeItems_ParentId",
                        column: x => x.ParentId,
                        principalTable: "TreeItems",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreeItems_ParentId",
                table: "TreeItems",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Credentials_TreeItems_Id",
                table: "Credentials",
                column: "Id",
                principalTable: "TreeItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_TreeItems_Id",
                table: "Folders",
                column: "Id",
                principalTable: "TreeItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
