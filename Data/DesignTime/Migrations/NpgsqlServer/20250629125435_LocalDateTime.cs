using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SnapManager.Data.DesignTime.Migrations.NpgsqlServer
{
    /// <inheritdoc />
    public partial class LocalDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModificationDate",
                table: "Credentials",
                newName: "ModificationDateUTC");

            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Credentials",
                newName: "CreationDateUTC");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModificationDateUTC",
                table: "Credentials",
                newName: "ModificationDate");

            migrationBuilder.RenameColumn(
                name: "CreationDateUTC",
                table: "Credentials",
                newName: "CreationDate");
        }
    }
}
