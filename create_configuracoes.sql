-- Criar tabela configuracoes se não existir
CREATE TABLE IF NOT EXISTS configuracoes (
    id SERIAL PRIMARY KEY,
    chave VARCHAR(100) NOT NULL UNIQUE,
    valor VARCHAR(2000) NOT NULL,
    criado_em TIMESTAMP WITH TIME ZONE DEFAULT NOW(),
    atualizado_em TIMESTAMP WITH TIME ZONE DEFAULT NOW()
);

-- Criar índice único na chave
CREATE UNIQUE INDEX IF NOT EXISTS idx_configuracoes_chave_unica ON configuracoes(chave);

-- Inserir configurações iniciais
INSERT INTO configuracoes (chave, valor) VALUES 
('home.titulo', 'Um lugar para encontrar a Cristo, a si mesmo, e ao próximo'),
('home.subtitulo', 'Domingos às 9h e 19h no centro da cidade'),
('home.textoApoio', 'Uma comunidade que busca crescer juntos'),
('home.horarios', 'Domingos: 9h e 19h'),
('igreja.nome', 'Comunidade da Graça'),
('igreja.lema', 'Uma igreja para a cidade'),
('igreja.endereco', 'Rua das Acácias, 248 — Centro'),
('igreja.telefone', '(11) 3000-0000'),
('igreja.email', 'contato@comunidadedagraca.com'),
('sobre.texto', 'Somos uma comunidade...')
ON CONFLICT (chave) DO NOTHING;
