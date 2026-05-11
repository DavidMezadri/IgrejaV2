-- =============================================================================
-- IgrejaV2 — Migração 03: Tabelas de Traduções e Versículos da Bíblia
-- =============================================================================

-- Tabela de Traduções (ex: ACF, NVI, ARA, etc)
CREATE TABLE IF NOT EXISTS traducoes (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    abreviacao VARCHAR(10) NOT NULL,
    descricao VARCHAR(500),

    data_criacao TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP WITH TIME ZONE,
    criado_por_id INTEGER,
    atualizado_por_id INTEGER,
    deletado BOOLEAN NOT NULL DEFAULT false,
    data_delecao TIMESTAMP WITH TIME ZONE,
    deletado_por_id INTEGER
);

-- Índice único na abreviação para rápida busca por tradução
CREATE UNIQUE INDEX IF NOT EXISTS idx_traducoes_abreviacao_unique
    ON traducoes (abreviacao)
    WHERE deletado = false;

-- Tabela de Versículos (Gênesis 1:1, Mateus 5:3, etc)
CREATE TABLE IF NOT EXISTS versiculos (
    id SERIAL PRIMARY KEY,
    livro INTEGER NOT NULL,
    capitulo INTEGER NOT NULL,
    numero INTEGER NOT NULL,
    texto VARCHAR(5000) NOT NULL,
    traducao_id INTEGER NOT NULL REFERENCES traducoes(id) ON DELETE CASCADE,

    data_criacao TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP WITH TIME ZONE,
    criado_por_id INTEGER,
    atualizado_por_id INTEGER,
    deletado BOOLEAN NOT NULL DEFAULT false,
    data_delecao TIMESTAMP WITH TIME ZONE,
    deletado_por_id INTEGER
);

-- Índice único para garantir que não há versículos duplicados por tradução
CREATE UNIQUE INDEX IF NOT EXISTS idx_versiculos_livro_capitulo_numero_traducao_unique
    ON versiculos (livro, capitulo, numero, traducao_id)
    WHERE deletado = false;

-- Índices para otimizar buscas frequentes
CREATE INDEX IF NOT EXISTS idx_versiculos_livro_traducao
    ON versiculos (livro, traducao_id)
    WHERE deletado = false;

CREATE INDEX IF NOT EXISTS idx_versiculos_livro_capitulo_traducao
    ON versiculos (livro, capitulo, traducao_id)
    WHERE deletado = false;

CREATE INDEX IF NOT EXISTS idx_versiculos_traducao_id
    ON versiculos (traducao_id)
    WHERE deletado = false;
