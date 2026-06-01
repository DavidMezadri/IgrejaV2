using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IgrejaV2.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class AddConfiguracaoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FamiliaId",
                table: "pessoas",
                newName: "familia_id");

            migrationBuilder.RenameIndex(
                name: "IX_pessoas_FamiliaId",
                table: "pessoas",
                newName: "IX_pessoas_familia_id");

            migrationBuilder.CreateTable(
                name: "configuracoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    chave = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    valor = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    criado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    atualizado_em = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_configuracoes", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_nome_familia_unique",
                table: "familias",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_configuracoes_chave_unica",
                table: "configuracoes",
                column: "chave",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "configuracoes");

            migrationBuilder.DropIndex(
                name: "idx_nome_familia_unique",
                table: "familias");

            migrationBuilder.RenameColumn(
                name: "familia_id",
                table: "pessoas",
                newName: "FamiliaId");

            migrationBuilder.RenameIndex(
                name: "IX_pessoas_familia_id",
                table: "pessoas",
                newName: "IX_pessoas_FamiliaId");
        }
    }
}
