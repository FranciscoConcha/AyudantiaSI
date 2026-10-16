using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TecnoFix.Src.Data.Migrations
{
    /// <inheritdoc />
    public partial class ActTecOr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Especialidad",
                table: "Usuarios",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Ordenes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdCliente = table.Column<int>(type: "integer", nullable: false),
                    IdTecnico = table.Column<int>(type: "integer", nullable: false),
                    TipoEquipo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Marca = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Modelo = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    NumeroSerie = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    DescripcionFalla = table.Column<string>(type: "text", nullable: false),
                    AccesoriosEntregados = table.Column<string>(type: "text", nullable: true),
                    Estado = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    FechaRecepcion = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ordenes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ordenes_Usuarios_IdCliente",
                        column: x => x.IdCliente,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Ordenes_Usuarios_IdTecnico",
                        column: x => x.IdTecnico,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bitacora",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdOrden = table.Column<int>(type: "integer", nullable: false),
                    FechaHora = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IdResponsable = table.Column<int>(type: "integer", nullable: false),
                    EstadoAnterior = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    EstadoNuevo = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Comentario = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bitacora", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bitacora_Ordenes_IdOrden",
                        column: x => x.IdOrden,
                        principalTable: "Ordenes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bitacora_Usuarios_IdResponsable",
                        column: x => x.IdResponsable,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bitacora_IdOrden",
                table: "Bitacora",
                column: "IdOrden");

            migrationBuilder.CreateIndex(
                name: "IX_Bitacora_IdResponsable",
                table: "Bitacora",
                column: "IdResponsable");

            migrationBuilder.CreateIndex(
                name: "IX_Ordenes_IdCliente",
                table: "Ordenes",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Ordenes_IdTecnico",
                table: "Ordenes",
                column: "IdTecnico");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bitacora");

            migrationBuilder.DropTable(
                name: "Ordenes");

            migrationBuilder.DropColumn(
                name: "Especialidad",
                table: "Usuarios");
        }
    }
}
