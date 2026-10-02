using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TecnoFix.Src.Data.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRutTelefonoUsuario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Rut",
                table: "Usuarios",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Usuarios",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_Rut",
                table: "Usuarios",
                column: "Rut",
                unique: true,
                filter: "\"Rut\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Usuarios_Rut",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Rut",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Usuarios");
        }
    }
}
