using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnapManager.Data.DesignTime.Migrations.NpgsqlServer
{
    /// <inheritdoc />
    public partial class OneParent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreeItemBaseTreeItemBase");

            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "TreeItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TreeItems_ParentId",
                table: "TreeItems",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_TreeItems_TreeItems_ParentId",
                table: "TreeItems",
                column: "ParentId",
                principalTable: "TreeItems",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreeItems_TreeItems_ParentId",
                table: "TreeItems");

            migrationBuilder.DropIndex(
                name: "IX_TreeItems_ParentId",
                table: "TreeItems");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "TreeItems");

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

            migrationBuilder.CreateIndex(
                name: "IX_TreeItemBaseTreeItemBase_ParentsId",
                table: "TreeItemBaseTreeItemBase",
                column: "ParentsId");
        }
    }
}
