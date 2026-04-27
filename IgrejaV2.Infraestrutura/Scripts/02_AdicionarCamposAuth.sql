-- =============================================================================
-- IgrejaV2 — Migração 02: Campos de autenticação na tabela usuarios
-- =============================================================================

ALTER TABLE usuarios
    ADD COLUMN IF NOT EXISTS email                              VARCHAR(200) NOT NULL DEFAULT '',
    ADD COLUMN IF NOT EXISTS token_recuperacao_senha           VARCHAR(500),
    ADD COLUMN IF NOT EXISTS token_recuperacao_senha_expiracao TIMESTAMP;

-- Índice único no nome de usuário (garante unicidade)
CREATE UNIQUE INDEX IF NOT EXISTS idx_usuarios_nome_usuario_unique
    ON usuarios (nome_usuario)
    WHERE deletado = false;

-- Índice no email para busca rápida por recuperação de senha
CREATE INDEX IF NOT EXISTS idx_usuarios_email
    ON usuarios (email)
    WHERE deletado = false;
