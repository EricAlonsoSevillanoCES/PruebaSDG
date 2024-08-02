using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiPoblacion.Migrations
{
    /// <inheritdoc />
    public partial class NameUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NameCommon",
                table: "Countries",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_NameCommon",
                table: "Countries",
                column: "NameCommon",
                unique: true,
                filter: "[NameCommon] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Countries_NameCommon",
                table: "Countries");

            migrationBuilder.AlterColumn<string>(
                name: "NameCommon",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
