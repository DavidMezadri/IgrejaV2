-- Script: Criar tabela avisos para mural da congregação
-- Data: 2026-06-14
-- Propósito: Armazenar avisos e notícias da congregação

-- Criar tabela avisos
CREATE TABLE IF NOT EXISTS avisos (
    id SERIAL PRIMARY KEY,
    titulo VARCHAR(200) NOT NULL,
    resumo VARCHAR(500) NOT NULL,
    conteudo TEXT,
    data TIMESTAMP WITH TIME ZONE NOT NULL,
    categoria VARCHAR(100),
    ativo BOOLEAN NOT NULL DEFAULT true,

    -- Campos de auditoria (herança de EntidadeBase)
    data_criacao TIMESTAMP WITH TIME ZONE NOT NULL DEFAULT CURRENT_TIMESTAMP,
    data_atualizacao TIMESTAMP WITH TIME ZONE,
    criado_por_id INTEGER,
    atualizado_por_id INTEGER,
    deletado BOOLEAN NOT NULL DEFAULT false,
    data_delecao TIMESTAMP WITH TIME ZONE,
    deletado_por_id INTEGER
);

-- Criar índices
CREATE INDEX IF NOT EXISTS idx_avisos_data ON avisos(data DESC);
CREATE INDEX IF NOT EXISTS idx_avisos_ativo ON avisos(ativo) WHERE ativo = true AND deletado = false;
CREATE INDEX IF NOT EXISTS idx_avisos_categoria ON avisos(categoria);
CREATE INDEX IF NOT EXISTS idx_avisos_deletado ON avisos(deletado);

-- Comentário na tabela
COMMENT ON TABLE avisos IS 'Avisos/mural da congregação';
COMMENT ON COLUMN avisos.titulo IS 'Título do aviso';
COMMENT ON COLUMN avisos.resumo IS 'Resumo curto do aviso';
COMMENT ON COLUMN avisos.conteudo IS 'Conteúdo completo do aviso';
COMMENT ON COLUMN avisos.data IS 'Data de publicação do aviso';
COMMENT ON COLUMN avisos.categoria IS 'Categoria/classificação do aviso (ex: Importante, Agenda, etc)';
COMMENT ON COLUMN avisos.ativo IS 'Indica se o aviso está ativo para exibição';
