-- =============================================================================
-- IgrejaV2 — Script de Criação Inicial das Tabelas
-- Banco de Dados: PostgreSQL
-- Gerado com base nas entidades de domínio e mapeamentos EF Core
-- =============================================================================
-- Enum reference (armazenados como INTEGER — valores do C# enum):
--
--   SexoEnum:             1=Masculino, 2=Feminino, 3=Outro
--   EstadoCivilEnum:      1=Solteiro, 2=Casado, 3=Divorciado, 4=Viuvo
--   TipoUsuarioEnum:      0=Administrador, 1=Pastor, 2=Presidente, 3=Comissao, 4=Membro, 5=Visitante
--   PublicoAlvoEnum:      0=Geral, 1=Liderancas, 2=Crianças, 3=Jovens, 4=Adultos, 5=Idosos
--   EstadoConservacaoEnum:1=Novo, 2=Bom, 3=Regular, 4=Ruim
--   AcaoLogEnum:          1=Criacao, 2=Edicao, 3=Delecao, 4=Login, 5=Checkin
-- =============================================================================

-- ---------------------------------------------------------------------------
-- 1. enderecos
--    Referenciada por: pessoas_enderecos, igrejas, eventos
-- ---------------------------------------------------------------------------
CREATE TABLE enderecos (
    id                  SERIAL          PRIMARY KEY,
    rua                 VARCHAR(250),
    numero              VARCHAR(50),
    complemento         VARCHAR(150),
    bairro              VARCHAR(150),
    cidade              VARCHAR(150),
    estado              VARCHAR(2),
    cep                 VARCHAR(20),

    -- EntidadeBase
    data_criacao        TIMESTAMP       NOT NULL DEFAULT NOW(),
    data_atualizacao    TIMESTAMP,
    criado_por_id       INTEGER,
    atualizado_por_id   INTEGER,
    deletado            BOOLEAN         NOT NULL DEFAULT FALSE,
    data_delecao        TIMESTAMP,
    deletado_por_id     INTEGER
);

-- ---------------------------------------------------------------------------
-- 2. pessoas
--    Referenciada por: familias (responsavel_id), usuarios, presencas,
--                      pessoas_enderecos, logs
--    Referencia:        familias (familia_id) — FK adicionada após familias
-- ---------------------------------------------------------------------------
CREATE TABLE pessoas (
    id                  SERIAL          PRIMARY KEY,
    nome                VARCHAR(200)    NOT NULL,
    data_nascimento     TIMESTAMP,
    sexo                SMALLINT,                       -- SexoEnum
    email               VARCHAR(100),
    telefone            VARCHAR(20),
    data_batismo        TIMESTAMP,
    membro_desde        TIMESTAMP,
    estado_civil        SMALLINT,                       -- EstadoCivilEnum
    observacoes         VARCHAR(500),
    ativo               BOOLEAN         NOT NULL DEFAULT TRUE,
    familia_id          INTEGER,                        -- FK: familias (adicionada abaixo)

    -- EntidadeBase
    data_criacao        TIMESTAMP       NOT NULL DEFAULT NOW(),
    data_atualizacao    TIMESTAMP,
    criado_por_id       INTEGER,
    atualizado_por_id   INTEGER,
    deletado            BOOLEAN         NOT NULL DEFAULT FALSE,
    data_delecao        TIMESTAMP,
    deletado_por_id     INTEGER
);

-- ---------------------------------------------------------------------------
-- 3. familias
--    Referencia: pessoas (responsavel_id)
--    Referenciada por: pessoas (familia_id)
-- ---------------------------------------------------------------------------
CREATE TABLE familias (
    id                  SERIAL          PRIMARY KEY,
    nome                VARCHAR(200)    NOT NULL,
    responsavel_id      INTEGER,                        -- FK: pessoas
    ativo               BOOLEAN         NOT NULL DEFAULT TRUE,
    observacoes         VARCHAR(500),

    -- EntidadeBase
    data_criacao        TIMESTAMP       NOT NULL DEFAULT NOW(),
    data_atualizacao    TIMESTAMP,
    criado_por_id       INTEGER,
    atualizado_por_id   INTEGER,
    deletado            BOOLEAN         NOT NULL DEFAULT FALSE,
    data_delecao        TIMESTAMP,
    deletado_por_id     INTEGER
);

-- ---------------------------------------------------------------------------
-- 4. usuarios
--    Referencia: pessoas (pessoa_id)
--    Referenciada por: presencas, logs
-- ---------------------------------------------------------------------------
CREATE TABLE usuarios (
    id                  SERIAL          PRIMARY KEY,
    nome_usuario        VARCHAR(150)    NOT NULL,
    senha               VARCHAR(500)    NOT NULL,
    tipo_usuario        SMALLINT        NOT NULL,       -- TipoUsuarioEnum
    primeiro_acesso     BOOLEAN         NOT NULL DEFAULT TRUE,
    ultimo_login        TIMESTAMP,
    ip_ultimo_login     VARCHAR(50),
    pessoa_id           INTEGER,                        -- FK: pessoas

    -- EntidadeBase
    data_criacao        TIMESTAMP       NOT NULL DEFAULT NOW(),
    data_atualizacao    TIMESTAMP,
    criado_por_id       INTEGER,
    atualizado_por_id   INTEGER,
    deletado            BOOLEAN         NOT NULL DEFAULT FALSE,
    data_delecao        TIMESTAMP,
    deletado_por_id     INTEGER
);

-- ---------------------------------------------------------------------------
-- 5. igrejas
--    Referencia: enderecos, pessoas (pastor_responsavel_id)
--    Referenciada por: patrimonios, logs
-- ---------------------------------------------------------------------------
CREATE TABLE igrejas (
    id                      SERIAL          PRIMARY KEY,
    nome                    VARCHAR(200)    NOT NULL,
    cnpj                    VARCHAR(20),
    telefone                VARCHAR(20),
    email                   VARCHAR(100),
    data_fundacao           TIMESTAMP,
    ativa                   BOOLEAN         NOT NULL DEFAULT TRUE,
    observacoes             VARCHAR(500),
    endereco_id             INTEGER,                    -- FK: enderecos
    pastor_responsavel_id   INTEGER,                    -- FK: pessoas

    -- EntidadeBase
    data_criacao            TIMESTAMP       NOT NULL DEFAULT NOW(),
    data_atualizacao        TIMESTAMP,
    criado_por_id           INTEGER,
    atualizado_por_id       INTEGER,
    deletado                BOOLEAN         NOT NULL DEFAULT FALSE,
    data_delecao            TIMESTAMP,
    deletado_por_id         INTEGER
);

-- ---------------------------------------------------------------------------
-- 6. patrimonios
--    Referencia: igrejas
-- ---------------------------------------------------------------------------
CREATE TABLE patrimonios (
    id                  SERIAL          PRIMARY KEY,
    igreja_id           INTEGER,                        -- FK: igrejas
    nome                VARCHAR(200)    NOT NULL,
    descricao           VARCHAR(1000),
    numero_patrimonio   VARCHAR(100),
    categoria           VARCHAR(100),
    valor_aquisicao     NUMERIC(18,2),
    data_aquisicao      TIMESTAMP,
    estado_conservacao  SMALLINT,                       -- EstadoConservacaoEnum
    ativo               BOOLEAN         NOT NULL DEFAULT TRUE,
    observacoes         VARCHAR(1000),

    -- EntidadeBase
    data_criacao        TIMESTAMP       NOT NULL DEFAULT NOW(),
    data_atualizacao    TIMESTAMP,
    criado_por_id       INTEGER,
    atualizado_por_id   INTEGER,
    deletado            BOOLEAN         NOT NULL DEFAULT FALSE,
    data_delecao        TIMESTAMP,
    deletado_por_id     INTEGER
);

-- ---------------------------------------------------------------------------
-- 7. tipos_evento
--    Referenciada por: eventos
-- ---------------------------------------------------------------------------
CREATE TABLE tipos_evento (
    id                  SERIAL          PRIMARY KEY,
    nome                VARCHAR(200)    NOT NULL,
    descricao           VARCHAR(1500),
    publico_alvo        SMALLINT,                       -- PublicoAlvoEnum
    requer_presenca     BOOLEAN         NOT NULL DEFAULT TRUE,
    ativo               BOOLEAN         NOT NULL DEFAULT TRUE,

    -- EntidadeBase
    data_criacao        TIMESTAMP       NOT NULL DEFAULT NOW(),
    data_atualizacao    TIMESTAMP,
    criado_por_id       INTEGER,
    atualizado_por_id   INTEGER,
    deletado            BOOLEAN         NOT NULL DEFAULT FALSE,
    data_delecao        TIMESTAMP,
    deletado_por_id     INTEGER
);

-- ---------------------------------------------------------------------------
-- 8. eventos
--    Referencia: tipos_evento, enderecos (shadow property no EF)
--    Referenciada por: presencas
-- ---------------------------------------------------------------------------
CREATE TABLE eventos (
    id                  SERIAL          PRIMARY KEY,
    nome                VARCHAR(200)    NOT NULL,
    descricao           VARCHAR(1000),
    tipo_evento_id      INTEGER         NOT NULL,       -- FK: tipos_evento
    data_inicio         TIMESTAMP       NOT NULL,
    data_fim            TIMESTAMP,
    local               VARCHAR(200),
    endereco_id         INTEGER,                        -- FK: enderecos
    capacidade_maxima   INTEGER,
    requer_inscricao    BOOLEAN         NOT NULL DEFAULT FALSE,
    ativo               BOOLEAN         NOT NULL DEFAULT TRUE,

    -- EntidadeBase
    data_criacao        TIMESTAMP       NOT NULL DEFAULT NOW(),
    data_atualizacao    TIMESTAMP,
    criado_por_id       INTEGER,
    atualizado_por_id   INTEGER,
    deletado            BOOLEAN         NOT NULL DEFAULT FALSE,
    data_delecao        TIMESTAMP,
    deletado_por_id     INTEGER
);

-- ---------------------------------------------------------------------------
-- 9. presencas
--    Referencia: eventos, pessoas, usuarios
-- ---------------------------------------------------------------------------
CREATE TABLE presencas (
    id                  SERIAL          PRIMARY KEY,
    evento_id           INTEGER         NOT NULL,       -- FK: eventos
    pessoa_id           INTEGER         NOT NULL,       -- FK: pessoas
    presente            BOOLEAN         NOT NULL DEFAULT FALSE,
    registrado_por_id   INTEGER,                        -- FK: usuarios
    observacao          VARCHAR(500),

    -- EntidadeBase
    data_criacao        TIMESTAMP       NOT NULL DEFAULT NOW(),
    data_atualizacao    TIMESTAMP,
    criado_por_id       INTEGER,
    atualizado_por_id   INTEGER,
    deletado            BOOLEAN         NOT NULL DEFAULT FALSE,
    data_delecao        TIMESTAMP,
    deletado_por_id     INTEGER
);

-- ---------------------------------------------------------------------------
-- 10. pessoas_enderecos (tabela de junção N:N)
--    Referencia: pessoas, enderecos
-- ---------------------------------------------------------------------------
CREATE TABLE pessoas_enderecos (
    id                  SERIAL          PRIMARY KEY,
    pessoa_id           INTEGER         NOT NULL,       -- FK: pessoas
    endereco_id         INTEGER         NOT NULL,       -- FK: enderecos

    -- EntidadeBase
    data_criacao        TIMESTAMP       NOT NULL DEFAULT NOW(),
    data_atualizacao    TIMESTAMP,
    criado_por_id       INTEGER,
    atualizado_por_id   INTEGER,
    deletado            BOOLEAN         NOT NULL DEFAULT FALSE,
    data_delecao        TIMESTAMP,
    deletado_por_id     INTEGER
);

-- ---------------------------------------------------------------------------
-- 11. logs
--    Referencia: igrejas, usuarios
-- ---------------------------------------------------------------------------
CREATE TABLE logs (
    id                  SERIAL          PRIMARY KEY,
    igreja_id           INTEGER,                        -- FK: igrejas
    usuario_id          INTEGER,                        -- FK: usuarios
    acao                SMALLINT        NOT NULL,       -- AcaoLogEnum
    entidade            VARCHAR(150),
    entidade_id         INTEGER,
    descricao           VARCHAR(1500),
    dados_anteriores    TEXT,
    dados_novos         TEXT,
    ip                  VARCHAR(50),
    user_agent          VARCHAR(500),

    -- EntidadeBase
    data_criacao        TIMESTAMP       NOT NULL DEFAULT NOW(),
    data_atualizacao    TIMESTAMP,
    criado_por_id       INTEGER,
    atualizado_por_id   INTEGER,
    deletado            BOOLEAN         NOT NULL DEFAULT FALSE,
    data_delecao        TIMESTAMP,
    deletado_por_id     INTEGER
);

-- =============================================================================
-- FOREIGN KEYS
-- Separadas da criação das tabelas para resolver dependências circulares
-- (pessoas ↔ familias)
-- =============================================================================

-- pessoas → familias (circular: criada após familias)
ALTER TABLE pessoas
    ADD CONSTRAINT fk_pessoas_familias_familia_id
    FOREIGN KEY (familia_id) REFERENCES familias (id) ON DELETE SET NULL;

-- familias → pessoas
ALTER TABLE familias
    ADD CONSTRAINT fk_familias_pessoas_responsavel_id
    FOREIGN KEY (responsavel_id) REFERENCES pessoas (id) ON DELETE RESTRICT;

-- usuarios → pessoas
ALTER TABLE usuarios
    ADD CONSTRAINT fk_usuarios_pessoas_pessoa_id
    FOREIGN KEY (pessoa_id) REFERENCES pessoas (id) ON DELETE SET NULL;

-- igrejas → enderecos
ALTER TABLE igrejas
    ADD CONSTRAINT fk_igrejas_enderecos_endereco_id
    FOREIGN KEY (endereco_id) REFERENCES enderecos (id) ON DELETE RESTRICT;

-- igrejas → pessoas (pastor responsável)
ALTER TABLE igrejas
    ADD CONSTRAINT fk_igrejas_pessoas_pastor_responsavel_id
    FOREIGN KEY (pastor_responsavel_id) REFERENCES pessoas (id) ON DELETE RESTRICT;

-- patrimonios → igrejas
ALTER TABLE patrimonios
    ADD CONSTRAINT fk_patrimonios_igrejas_igreja_id
    FOREIGN KEY (igreja_id) REFERENCES igrejas (id) ON DELETE SET NULL;

-- eventos → tipos_evento
ALTER TABLE eventos
    ADD CONSTRAINT fk_eventos_tipos_evento_tipo_evento_id
    FOREIGN KEY (tipo_evento_id) REFERENCES tipos_evento (id) ON DELETE RESTRICT;

-- eventos → enderecos
ALTER TABLE eventos
    ADD CONSTRAINT fk_eventos_enderecos_endereco_id
    FOREIGN KEY (endereco_id) REFERENCES enderecos (id) ON DELETE SET NULL;

-- presencas → eventos
ALTER TABLE presencas
    ADD CONSTRAINT fk_presencas_eventos_evento_id
    FOREIGN KEY (evento_id) REFERENCES eventos (id) ON DELETE RESTRICT;

-- presencas → pessoas
ALTER TABLE presencas
    ADD CONSTRAINT fk_presencas_pessoas_pessoa_id
    FOREIGN KEY (pessoa_id) REFERENCES pessoas (id) ON DELETE RESTRICT;

-- presencas → usuarios
ALTER TABLE presencas
    ADD CONSTRAINT fk_presencas_usuarios_registrado_por_id
    FOREIGN KEY (registrado_por_id) REFERENCES usuarios (id) ON DELETE SET NULL;

-- pessoas_enderecos → pessoas
ALTER TABLE pessoas_enderecos
    ADD CONSTRAINT fk_pessoas_enderecos_pessoa_id
    FOREIGN KEY (pessoa_id) REFERENCES pessoas (id) ON DELETE CASCADE;

-- pessoas_enderecos → enderecos
ALTER TABLE pessoas_enderecos
    ADD CONSTRAINT fk_pessoas_enderecos_endereco_id
    FOREIGN KEY (endereco_id) REFERENCES enderecos (id) ON DELETE CASCADE;

-- logs → igrejas
ALTER TABLE logs
    ADD CONSTRAINT fk_logs_igrejas_igreja_id
    FOREIGN KEY (igreja_id) REFERENCES igrejas (id) ON DELETE SET NULL;

-- logs → usuarios
ALTER TABLE logs
    ADD CONSTRAINT fk_logs_usuarios_usuario_id
    FOREIGN KEY (usuario_id) REFERENCES usuarios (id) ON DELETE SET NULL;

-- =============================================================================
-- ÍNDICES
-- =============================================================================

-- Soft delete — índices para filtrar registros não deletados nos SELECTs mais comuns
CREATE INDEX idx_pessoas_deletado            ON pessoas             (deletado);
CREATE INDEX idx_familias_deletado           ON familias            (deletado);
CREATE INDEX idx_usuarios_deletado           ON usuarios            (deletado);
CREATE INDEX idx_eventos_deletado            ON eventos             (deletado);
CREATE INDEX idx_presencas_deletado          ON presencas           (deletado);
CREATE INDEX idx_igrejas_deletado            ON igrejas             (deletado);
CREATE INDEX idx_patrimonios_deletado        ON patrimonios         (deletado);
CREATE INDEX idx_tipos_evento_deletado       ON tipos_evento        (deletado);
CREATE INDEX idx_pessoas_enderecos_deletado  ON pessoas_enderecos   (deletado);
CREATE INDEX idx_logs_deletado               ON logs                (deletado);

-- FKs mais consultadas
CREATE INDEX idx_presencas_evento_id         ON presencas           (evento_id);
CREATE INDEX idx_presencas_pessoa_id         ON presencas           (pessoa_id);
CREATE INDEX idx_pessoas_familia_id          ON pessoas             (familia_id);
CREATE INDEX idx_usuarios_pessoa_id          ON usuarios            (pessoa_id);
CREATE INDEX idx_patrimonios_igreja_id       ON patrimonios         (igreja_id);
CREATE INDEX idx_logs_usuario_id             ON logs                (usuario_id);
CREATE INDEX idx_logs_igreja_id              ON logs                (igreja_id);
CREATE INDEX idx_pessoas_enderecos_pessoa    ON pessoas_enderecos   (pessoa_id);
CREATE INDEX idx_pessoas_enderecos_endereco  ON pessoas_enderecos   (endereco_id);
