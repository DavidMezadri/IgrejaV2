-- Script: Corrigir constraints RESTRICT para SET NULL (PostgreSQL não suporta RESTRICT)
-- Data: 2026-05-30
-- Problema: PostgreSQL lança erro "Sintaxe incorreta próxima a 'RESTRICT'"
-- Solução: Usar SET NULL em vez de RESTRICT para soft-delete funcionar

-- Remover constraints com RESTRICT
ALTER TABLE familias DROP CONSTRAINT IF EXISTS fk_familias_pessoas_responsavel_id;
ALTER TABLE igrejas DROP CONSTRAINT IF EXISTS fk_igrejas_enderecos_endereco_id;
ALTER TABLE igrejas DROP CONSTRAINT IF EXISTS fk_igrejas_pessoas_pastor_responsavel_id;
ALTER TABLE eventos DROP CONSTRAINT IF EXISTS fk_eventos_tipos_evento_tipo_evento_id;
ALTER TABLE presencas DROP CONSTRAINT IF EXISTS fk_presencas_eventos_evento_id;
ALTER TABLE presencas DROP CONSTRAINT IF EXISTS fk_presencas_pessoas_pessoa_id;

-- Re-criar com SET NULL
ALTER TABLE familias
    ADD CONSTRAINT fk_familias_pessoas_responsavel_id
    FOREIGN KEY (responsavel_id) REFERENCES pessoas (id) ON DELETE SET NULL;

ALTER TABLE igrejas
    ADD CONSTRAINT fk_igrejas_enderecos_endereco_id
    FOREIGN KEY (endereco_id) REFERENCES enderecos (id) ON DELETE SET NULL;

ALTER TABLE igrejas
    ADD CONSTRAINT fk_igrejas_pessoas_pastor_responsavel_id
    FOREIGN KEY (pastor_responsavel_id) REFERENCES pessoas (id) ON DELETE SET NULL;

ALTER TABLE eventos
    ADD CONSTRAINT fk_eventos_tipos_evento_tipo_evento_id
    FOREIGN KEY (tipo_evento_id) REFERENCES tipos_evento (id) ON DELETE SET NULL;

ALTER TABLE presencas
    ADD CONSTRAINT fk_presencas_eventos_evento_id
    FOREIGN KEY (evento_id) REFERENCES eventos (id) ON DELETE SET NULL;

ALTER TABLE presencas
    ADD CONSTRAINT fk_presencas_pessoas_pessoa_id
    FOREIGN KEY (pessoa_id) REFERENCES pessoas (id) ON DELETE SET NULL;
