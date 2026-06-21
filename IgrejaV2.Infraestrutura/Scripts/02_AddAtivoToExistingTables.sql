-- =============================================================================
-- IgrejaV2 — Script 02: Adicionar coluna ATIVO às tabelas existentes
-- =============================================================================
-- Este script adiciona a coluna 'ativo' (parte de EntidadeBase) às tabelas
-- que ainda não a possuem. Usa IF NOT EXISTS para evitar erros.
-- Executa toda vez que a aplicação inicia, mas sem falhar.

ALTER TABLE usuarios ADD COLUMN IF NOT EXISTS ativo BOOLEAN NOT NULL DEFAULT TRUE;
ALTER TABLE usuarios ADD COLUMN IF NOT EXISTS email VARCHAR(200);
ALTER TABLE enderecos ADD COLUMN IF NOT EXISTS ativo BOOLEAN NOT NULL DEFAULT TRUE;
ALTER TABLE igrejas ADD COLUMN IF NOT EXISTS ativo BOOLEAN NOT NULL DEFAULT TRUE;
ALTER TABLE presencas ADD COLUMN IF NOT EXISTS ativo BOOLEAN NOT NULL DEFAULT TRUE;
ALTER TABLE pessoas_enderecos ADD COLUMN IF NOT EXISTS principal BOOLEAN NOT NULL DEFAULT FALSE;
ALTER TABLE pessoas_enderecos ADD COLUMN IF NOT EXISTS ativo BOOLEAN NOT NULL DEFAULT TRUE;
ALTER TABLE logs ADD COLUMN IF NOT EXISTS ativo BOOLEAN NOT NULL DEFAULT TRUE;
ALTER TABLE traducoes ADD COLUMN IF NOT EXISTS ativo BOOLEAN NOT NULL DEFAULT TRUE;
ALTER TABLE versiculos ADD COLUMN IF NOT EXISTS ativo BOOLEAN NOT NULL DEFAULT TRUE;
