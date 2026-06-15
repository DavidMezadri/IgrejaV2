using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IgrejaV2.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class AddAvisoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "avisos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    titulo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    resumo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    conteudo = table.Column<string>(type: "text", nullable: true),
                    data = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    categoria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    criado_por_id = table.Column<int>(type: "integer", nullable: true),
                    atualizado_por_id = table.Column<int>(type: "integer", nullable: true),
                    deletado = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    data_delecao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletado_por_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_avisos", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_avisos_ativo",
                table: "avisos",
                column: "ativo");

            migrationBuilder.CreateIndex(
                name: "idx_avisos_categoria",
                table: "avisos",
                column: "categoria");

            migrationBuilder.CreateIndex(
                name: "idx_avisos_data",
                table: "avisos",
                column: "data");

            migrationBuilder.CreateIndex(
                name: "idx_avisos_deletado",
                table: "avisos",
                column: "deletado");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "avisos");
        }
    }
}
