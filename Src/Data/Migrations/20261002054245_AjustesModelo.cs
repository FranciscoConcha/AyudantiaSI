using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TecnoFix.Src.Data.Migrations
{
    /// <inheritdoc />
    public partial class AjustesModelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Rut",
                table: "Usuarios");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Rut",
                table: "Usuarios",
                column: "Rut",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Rut",
                table: "Usuarios");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Rut",
                table: "Usuarios",
                column: "Rut",
                unique: true,
                filter: "\"Rut\" IS NOT NULL");
        }
    }
}
