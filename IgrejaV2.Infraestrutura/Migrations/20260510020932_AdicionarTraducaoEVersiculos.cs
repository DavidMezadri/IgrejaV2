using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IgrejaV2.Infraestrutura.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarTraducaoEVersiculos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "enderecos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    rua = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    complemento = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    numero = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    bairro = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    cidade = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    estado = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: true),
                    cep = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    criado_por_id = table.Column<int>(type: "integer", nullable: true),
                    atualizado_por_id = table.Column<int>(type: "integer", nullable: true),
                    deletado = table.Column<bool>(type: "boolean", nullable: false),
                    data_delecao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletado_por_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enderecos", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "tipos_evento",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    descricao = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    publico_alvo = table.Column<int>(type: "integer", nullable: true),
                    requer_presenca = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    criado_por_id = table.Column<int>(type: "integer", nullable: true),
                    atualizado_por_id = table.Column<int>(type: "integer", nullable: true),
                    deletado = table.Column<bool>(type: "boolean", nullable: false),
                    data_delecao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletado_por_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipos_evento", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "traducoes",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    abreviacao = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    descricao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    criado_por_id = table.Column<int>(type: "integer", nullable: true),
                    atualizado_por_id = table.Column<int>(type: "integer", nullable: true),
                    deletado = table.Column<bool>(type: "boolean", nullable: false),
                    data_delecao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletado_por_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_traducoes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "eventos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    tipo_evento_id = table.Column<int>(type: "integer", nullable: false),
                    data_inicio = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_fim = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    local = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    endereco_id = table.Column<int>(type: "integer", nullable: true),
                    capacidade_maxima = table.Column<int>(type: "integer", nullable: true),
                    requer_inscricao = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    criado_por_id = table.Column<int>(type: "integer", nullable: true),
                    atualizado_por_id = table.Column<int>(type: "integer", nullable: true),
                    deletado = table.Column<bool>(type: "boolean", nullable: false),
                    data_delecao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletado_por_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_eventos", x => x.id);
                    table.ForeignKey(
                        name: "fk_eventos_enderecos_endereco_id",
                        column: x => x.endereco_id,
                        principalTable: "enderecos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_eventos_tipos_evento_tipo_evento_id",
                        column: x => x.tipo_evento_id,
                        principalTable: "tipos_evento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "versiculos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    livro = table.Column<int>(type: "integer", nullable: false),
                    capitulo = table.Column<int>(type: "integer", nullable: false),
                    numero = table.Column<int>(type: "integer", nullable: false),
                    texto = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    traducao_id = table.Column<int>(type: "integer", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    criado_por_id = table.Column<int>(type: "integer", nullable: true),
                    atualizado_por_id = table.Column<int>(type: "integer", nullable: true),
                    deletado = table.Column<bool>(type: "boolean", nullable: false),
                    data_delecao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletado_por_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_versiculos", x => x.id);
                    table.ForeignKey(
                        name: "fk_versiculos_traducao_id",
                        column: x => x.traducao_id,
                        principalTable: "traducoes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "familias",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    responsavel_id = table.Column<int>(type: "integer", nullable: true),
                    ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    observacoes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    criado_por_id = table.Column<int>(type: "integer", nullable: true),
                    atualizado_por_id = table.Column<int>(type: "integer", nullable: true),
                    deletado = table.Column<bool>(type: "boolean", nullable: false),
                    data_delecao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletado_por_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_familias", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pessoas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ativo = table.Column<bool>(type: "boolean", nullable: false),
                    nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    data_nascimento = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    sexo = table.Column<int>(type: "integer", nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    data_batismo = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    membro_desde = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado_civil = table.Column<int>(type: "integer", nullable: true),
                    observacoes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    FamiliaId = table.Column<int>(type: "integer", nullable: true),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    criado_por_id = table.Column<int>(type: "integer", nullable: true),
                    atualizado_por_id = table.Column<int>(type: "integer", nullable: true),
                    deletado = table.Column<bool>(type: "boolean", nullable: false),
                    data_delecao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletado_por_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pessoas", x => x.id);
                    table.ForeignKey(
                        name: "fk_pessoas_familias_familia_id",
                        column: x => x.FamiliaId,
                        principalTable: "familias",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "igrejas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cnpj = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    endereco_id = table.Column<int>(type: "integer", nullable: true),
                    pastor_responsavel_id = table.Column<int>(type: "integer", nullable: true),
                    data_fundacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ativa = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    observacoes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    criado_por_id = table.Column<int>(type: "integer", nullable: true),
                    atualizado_por_id = table.Column<int>(type: "integer", nullable: true),
                    deletado = table.Column<bool>(type: "boolean", nullable: false),
                    data_delecao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletado_por_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_igrejas", x => x.id);
                    table.ForeignKey(
                        name: "fk_igrejas_enderecos_endereco_id",
                        column: x => x.endereco_id,
                        principalTable: "enderecos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_igrejas_pessoas_pastor_responsavel_id",
                        column: x => x.pastor_responsavel_id,
                        principalTable: "pessoas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "pessoas_enderecos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    endereco_id = table.Column<int>(type: "integer", nullable: false),
                    pessoa_id = table.Column<int>(type: "integer", nullable: false),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    criado_por_id = table.Column<int>(type: "integer", nullable: true),
                    atualizado_por_id = table.Column<int>(type: "integer", nullable: true),
                    deletado = table.Column<bool>(type: "boolean", nullable: false),
                    data_delecao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletado_por_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pessoas_enderecos", x => x.id);
                    table.ForeignKey(
                        name: "fk_pessoas_enderecos_endereco_id",
                        column: x => x.endereco_id,
                        principalTable: "enderecos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_pessoas_enderecos_pessoa_id",
                        column: x => x.pessoa_id,
                        principalTable: "pessoas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nome_usuario = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    senha = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    tipo_usuario = table.Column<int>(type: "integer", nullable: false),
                    primeiro_acesso = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    ultimo_login = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ip_ultimo_login = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    token_recuperacao_senha = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    token_recuperacao_senha_expiracao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    pessoa_id = table.Column<int>(type: "integer", nullable: true),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    criado_por_id = table.Column<int>(type: "integer", nullable: true),
                    atualizado_por_id = table.Column<int>(type: "integer", nullable: true),
                    deletado = table.Column<bool>(type: "boolean", nullable: false),
                    data_delecao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletado_por_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id);
                    table.ForeignKey(
                        name: "fk_usuarios_pessoas_pessoa_id",
                        column: x => x.pessoa_id,
                        principalTable: "pessoas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "patrimonios",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    igreja_id = table.Column<int>(type: "integer", nullable: true),
                    nome = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    descricao = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    numero_patrimonio = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    categoria = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    valor_aquisicao = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    data_aquisicao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estado_conservacao = table.Column<int>(type: "integer", nullable: true),
                    ativo = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    observacoes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    criado_por_id = table.Column<int>(type: "integer", nullable: true),
                    atualizado_por_id = table.Column<int>(type: "integer", nullable: true),
                    deletado = table.Column<bool>(type: "boolean", nullable: false),
                    data_delecao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletado_por_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patrimonios", x => x.id);
                    table.ForeignKey(
                        name: "fk_patrimonios_igrejas_igreja_id",
                        column: x => x.igreja_id,
                        principalTable: "igrejas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    igreja_id = table.Column<int>(type: "integer", nullable: true),
                    usuario_id = table.Column<int>(type: "integer", nullable: true),
                    acao = table.Column<int>(type: "integer", nullable: false),
                    entidade = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    entidade_id = table.Column<int>(type: "integer", nullable: true),
                    descricao = table.Column<string>(type: "character varying(1500)", maxLength: 1500, nullable: true),
                    dados_anteriores = table.Column<string>(type: "text", nullable: true),
                    dados_novos = table.Column<string>(type: "text", nullable: true),
                    ip = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    user_agent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    criado_por_id = table.Column<int>(type: "integer", nullable: true),
                    atualizado_por_id = table.Column<int>(type: "integer", nullable: true),
                    deletado = table.Column<bool>(type: "boolean", nullable: false),
                    data_delecao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletado_por_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_logs_igrejas_igreja_id",
                        column: x => x.igreja_id,
                        principalTable: "igrejas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "fk_logs_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "presencas",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    evento_id = table.Column<int>(type: "integer", nullable: false),
                    pessoa_id = table.Column<int>(type: "integer", nullable: false),
                    presente = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    registrado_por_id = table.Column<int>(type: "integer", nullable: true),
                    observacao = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    data_criacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    data_atualizacao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    criado_por_id = table.Column<int>(type: "integer", nullable: true),
                    atualizado_por_id = table.Column<int>(type: "integer", nullable: true),
                    deletado = table.Column<bool>(type: "boolean", nullable: false),
                    data_delecao = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    deletado_por_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_presencas", x => x.id);
                    table.ForeignKey(
                        name: "FK_presencas_pessoas_pessoa_id",
                        column: x => x.pessoa_id,
                        principalTable: "pessoas",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_presencas_eventos_evento_id",
                        column: x => x.evento_id,
                        principalTable: "eventos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_presencas_usuarios_registrado_por_id",
                        column: x => x.registrado_por_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_eventos_endereco_id",
                table: "eventos",
                column: "endereco_id");

            migrationBuilder.CreateIndex(
                name: "IX_eventos_tipo_evento_id",
                table: "eventos",
                column: "tipo_evento_id");

            migrationBuilder.CreateIndex(
                name: "IX_familias_responsavel_id",
                table: "familias",
                column: "responsavel_id");

            migrationBuilder.CreateIndex(
                name: "IX_igrejas_endereco_id",
                table: "igrejas",
                column: "endereco_id");

            migrationBuilder.CreateIndex(
                name: "IX_igrejas_pastor_responsavel_id",
                table: "igrejas",
                column: "pastor_responsavel_id");

            migrationBuilder.CreateIndex(
                name: "IX_logs_igreja_id",
                table: "logs",
                column: "igreja_id");

            migrationBuilder.CreateIndex(
                name: "IX_logs_usuario_id",
                table: "logs",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_patrimonios_igreja_id",
                table: "patrimonios",
                column: "igreja_id");

            migrationBuilder.CreateIndex(
                name: "IX_pessoas_FamiliaId",
                table: "pessoas",
                column: "FamiliaId");

            migrationBuilder.CreateIndex(
                name: "IX_pessoas_enderecos_endereco_id",
                table: "pessoas_enderecos",
                column: "endereco_id");

            migrationBuilder.CreateIndex(
                name: "IX_pessoas_enderecos_pessoa_id",
                table: "pessoas_enderecos",
                column: "pessoa_id");

            migrationBuilder.CreateIndex(
                name: "IX_presencas_evento_id",
                table: "presencas",
                column: "evento_id");

            migrationBuilder.CreateIndex(
                name: "IX_presencas_pessoa_id",
                table: "presencas",
                column: "pessoa_id");

            migrationBuilder.CreateIndex(
                name: "IX_presencas_registrado_por_id",
                table: "presencas",
                column: "registrado_por_id");

            migrationBuilder.CreateIndex(
                name: "idx_usuarios_email",
                table: "usuarios",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "idx_usuarios_nome_usuario_unique",
                table: "usuarios",
                column: "nome_usuario",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_pessoa_id",
                table: "usuarios",
                column: "pessoa_id");

            migrationBuilder.CreateIndex(
                name: "IX_versiculos_livro_capitulo_numero_traducao_id",
                table: "versiculos",
                columns: new[] { "livro", "capitulo", "numero", "traducao_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_versiculos_livro_capitulo_traducao_id",
                table: "versiculos",
                columns: new[] { "livro", "capitulo", "traducao_id" });

            migrationBuilder.CreateIndex(
                name: "IX_versiculos_livro_traducao_id",
                table: "versiculos",
                columns: new[] { "livro", "traducao_id" });

            migrationBuilder.CreateIndex(
                name: "IX_versiculos_traducao_id",
                table: "versiculos",
                column: "traducao_id");

            migrationBuilder.AddForeignKey(
                name: "fk_familias_pessoas_responsavel_id",
                table: "familias",
                column: "responsavel_id",
                principalTable: "pessoas",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_familias_pessoas_responsavel_id",
                table: "familias");

            migrationBuilder.DropTable(
                name: "logs");

            migrationBuilder.DropTable(
                name: "patrimonios");

            migrationBuilder.DropTable(
                name: "pessoas_enderecos");

            migrationBuilder.DropTable(
                name: "presencas");

            migrationBuilder.DropTable(
                name: "versiculos");

            migrationBuilder.DropTable(
                name: "igrejas");

            migrationBuilder.DropTable(
                name: "eventos");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "traducoes");

            migrationBuilder.DropTable(
                name: "enderecos");

            migrationBuilder.DropTable(
                name: "tipos_evento");

            migrationBuilder.DropTable(
                name: "pessoas");

            migrationBuilder.DropTable(
                name: "familias");
        }
    }
}
