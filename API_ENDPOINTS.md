# Documentação de Endpoints da API IgrejaV2

## 📌 Base URL
```
http://localhost:{PORT}/api
```

## 🔐 Autenticação

Todos os endpoints (exceto autenticação e criação de usuário) requerem um token JWT.

**Header obrigatório:**
```
Authorization: Bearer {token_jwt}
```

---

## 🔑 Autenticação (`/api/auth`)

### POST `/login`
Autentica um usuário e retorna um token JWT.

**Request:**
```json
{
  "nomeUsuario": "admin",
  "senha": "MinhaSenh@123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiracao": "2026-05-05T10:30:00Z",
  "usuario": {
    "id": 1,
    "nomeUsuario": "admin",
    "email": "admin@exemplo.com",
    "tipoUsuario": 0,
    "primeiroAcesso": false,
    "ultimoLogin": "2026-05-04T10:30:00Z",
    "dataCriacao": "2026-01-01T00:00:00Z"
  }
}
```

**Erros:**
- `401 Unauthorized`: Credenciais inválidas

---

### POST `/recuperar-senha`
Solicita recuperação de senha por e-mail.

**Request:**
```json
{
  "email": "usuario@exemplo.com"
}
```

**Response (200 OK):**
```json
{
  "mensagem": "Se o e-mail estiver cadastrado, você receberá as instruções para redefinir a senha.",
  "token": "abc123..." // apenas em desenvolvimento
}
```

---

### POST `/resetar-senha`
Redefine a senha usando o token de recuperação.

**Request:**
```json
{
  "token": "abc123...",
  "novaSenha": "NovaSenh@456",
  "confirmarNovaSenha": "NovaSenh@456"
}
```

**Response (204 No Content):**
Sem corpo de resposta

**Erros:**
- `400 Bad Request`: Token inválido ou expirado

---

## 👥 Usuários (`/api/usuarios`)

### POST `/` - Criar usuário (Público)
Cria um novo usuário no sistema.

**Request:**
```json
{
  "nomeUsuario": "joao.silva",
  "email": "joao@exemplo.com",
  "senha": "MinhaSenh@123",
  "confirmarSenha": "MinhaSenh@123",
  "tipoUsuario": 4
}
```

**Tipos de usuário:**
- `0` - Administrador
- `1` - Pastor
- `2` - Presidente
- `3` - Comissão
- `4` - Membro (padrão)
- `5` - Visitante

**Response (201 Created):**
```json
{
  "id": 10,
  "nomeUsuario": "joao.silva",
  "email": "joao@exemplo.com",
  "tipoUsuario": 4,
  "primeiroAcesso": true,
  "ultimoLogin": null,
  "dataCriacao": "2026-05-04T10:30:00Z"
}
```

**Erros:**
- `400 Bad Request`: Dados inválidos
- `409 Conflict`: Nome de usuário ou e-mail já cadastrado

---

### GET `/` - Listar usuários
Lista todos os usuários cadastrados (requer autenticação).

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "nomeUsuario": "admin",
    "email": "admin@exemplo.com",
    "tipoUsuario": 0,
    "primeiroAcesso": false,
    "ultimoLogin": "2026-05-04T10:30:00Z",
    "dataCriacao": "2026-01-01T00:00:00Z"
  }
]
```

---

### GET `/{id}` - Obter usuário por ID
Obtém dados de um usuário específico (requer autenticação).

**Response (200 OK):**
```json
{
  "id": 1,
  "nomeUsuario": "admin",
  "email": "admin@exemplo.com",
  "tipoUsuario": 0,
  "primeiroAcesso": false,
  "ultimoLogin": "2026-05-04T10:30:00Z",
  "dataCriacao": "2026-01-01T00:00:00Z"
}
```

**Erros:**
- `404 Not Found`: Usuário não encontrado

---

### PUT `/{id}` - Atualizar usuário
Atualiza nome de usuário e tipo de um usuário (requer autenticação).

**Request:**
```json
{
  "nomeUsuario": "joao.silva.novo",
  "tipoUsuario": 3
}
```

**Response (200 OK):**
```json
{
  "id": 10,
  "nomeUsuario": "joao.silva.novo",
  "email": "joao@exemplo.com",
  "tipoUsuario": 3,
  "primeiroAcesso": false,
  "ultimoLogin": "2026-05-04T10:30:00Z",
  "dataCriacao": "2026-05-04T10:30:00Z"
}
```

**Erros:**
- `404 Not Found`: Usuário não encontrado

---

### DELETE `/{id}` - Remover usuário
Remove um usuário (soft delete - requer autenticação).

**Response (204 No Content):**
Sem corpo de resposta

**Erros:**
- `404 Not Found`: Usuário não encontrado

---

## 👤 Pessoas (`/api/pessoas`)

⚠️ **Todos os endpoints requerem autenticação JWT**

### POST `/` - Criar pessoa
Cadastra uma nova pessoa (membro ou visitante).

**Request:**
```json
{
  "nome": "João Silva",
  "dataNascimento": "1990-05-15",
  "cpf": "12345678900",
  "email": "joao@exemplo.com",
  "telefone": "(11) 99999-9999",
  "familiaId": 1
}
```

**Response (201 Created):**
```json
{
  "id": 5,
  "nome": "João Silva",
  "dataNascimento": "1990-05-15",
  "cpf": "12345678900",
  "email": "joao@exemplo.com",
  "telefone": "(11) 99999-9999",
  "familiaId": 1,
  "ativo": true,
  "dataCriacao": "2026-05-04T10:30:00Z"
}
```

**Erros:**
- `400 Bad Request`: Dados inválidos

---

### GET `/` - Listar todas as pessoas
Lista todas as pessoas cadastradas.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "nome": "João Silva",
    "dataNascimento": "1990-05-15",
    "cpf": "12345678900",
    "email": "joao@exemplo.com",
    "telefone": "(11) 99999-9999",
    "familiaId": 1,
    "ativo": true,
    "dataCriacao": "2026-05-04T10:30:00Z"
  }
]
```

---

### GET `/ativos` - Listar apenas pessoas ativas
Lista apenas as pessoas marcadas como ativas.

**Response (200 OK):** Array de pessoas ativas

---

### GET `/buscar?nome={nome}` - Buscar pessoa por nome
Busca pessoas pelo nome (parcial).

**Parameters:**
- `nome` (obrigatório): Nome ou parte do nome

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "nome": "João Silva",
    "dataNascimento": "1990-05-15",
    "cpf": "12345678900",
    "email": "joao@exemplo.com",
    "telefone": "(11) 99999-9999",
    "familiaId": 1,
    "ativo": true,
    "dataCriacao": "2026-05-04T10:30:00Z"
  }
]
```

**Erros:**
- `400 Bad Request`: Parâmetro 'nome' ausente ou inválido

---

### GET `/familia/{familiaId}` - Listar pessoas de uma família
Lista todas as pessoas de uma família específica.

**Parameters:**
- `familiaId` (obrigatório): ID da família

**Response (200 OK):** Array de pessoas da família

---

### GET `/{id}` - Obter pessoa por ID
Obtém dados de uma pessoa específica.

**Response (200 OK):**
```json
{
  "id": 1,
  "nome": "João Silva",
  "dataNascimento": "1990-05-15",
  "cpf": "12345678900",
  "email": "joao@exemplo.com",
  "telefone": "(11) 99999-9999",
  "familiaId": 1,
  "ativo": true,
  "dataCriacao": "2026-05-04T10:30:00Z"
}
```

**Erros:**
- `404 Not Found`: Pessoa não encontrada

---

### PUT `/{id}` - Atualizar pessoa
Atualiza os dados de uma pessoa.

**Request:**
```json
{
  "nome": "João Silva Santos",
  "dataNascimento": "1990-05-15",
  "cpf": "12345678900",
  "email": "joao.novo@exemplo.com",
  "telefone": "(11) 98888-8888",
  "familiaId": 2,
  "ativo": true
}
```

**Response (200 OK):** Pessoa atualizada

**Erros:**
- `404 Not Found`: Pessoa não encontrada

---

### DELETE `/{id}` - Remover pessoa
Remove uma pessoa (hard delete).

**Response (204 No Content):**
Sem corpo de resposta

**Erros:**
- `404 Not Found`: Pessoa não encontrada

---

## 👨‍👩‍👧‍👦 Famílias (`/api/familias`)

⚠️ **Todos os endpoints requerem autenticação JWT**

### POST `/` - Criar família
Cadastra uma nova família.

**Request:**
```json
{
  "nomeFamilia": "Silva",
  "observacoes": "Família de membros ativos"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "nomeFamilia": "Silva",
  "observacoes": "Família de membros ativos",
  "dataCriacao": "2026-05-04T10:30:00Z"
}
```

**Erros:**
- `400 Bad Request`: Dados inválidos

---

### GET `/` - Listar famílias
Lista todas as famílias cadastradas.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "nomeFamilia": "Silva",
    "observacoes": "Família de membros ativos",
    "dataCriacao": "2026-05-04T10:30:00Z"
  }
]
```

---

### GET `/{id}` - Obter família por ID
Obtém dados de uma família (inclui membros).

**Response (200 OK):**
```json
{
  "id": 1,
  "nomeFamilia": "Silva",
  "observacoes": "Família de membros ativos",
  "dataCriacao": "2026-05-04T10:30:00Z",
  "membros": [
    {
      "id": 1,
      "nome": "João Silva",
      "dataNascimento": "1990-05-15",
      "cpf": "12345678900",
      "email": "joao@exemplo.com",
      "telefone": "(11) 99999-9999",
      "ativo": true
    }
  ]
}
```

**Erros:**
- `404 Not Found`: Família não encontrada

---

### PUT `/{id}` - Atualizar família
Atualiza os dados de uma família.

**Request:**
```json
{
  "nomeFamilia": "Silva Santos",
  "observacoes": "Família atualizada"
}
```

**Response (200 OK):** Família atualizada

**Erros:**
- `404 Not Found`: Família não encontrada

---

### DELETE `/{id}` - Remover família
Remove uma família.

**Response (204 No Content):**
Sem corpo de resposta

**Erros:**
- `404 Not Found`: Família não encontrada

---

## 📅 Eventos (`/api/eventos`)

⚠️ **Todos os endpoints requerem autenticação JWT**

### POST `/` - Criar evento
Cadastra um novo evento.

**Request:**
```json
{
  "nome": "Culto Dominical",
  "descricao": "Culto semanal de adoração",
  "dataInicio": "2026-05-05T19:00:00Z",
  "dataFim": "2026-05-05T21:00:00Z",
  "local": "Auditório Principal",
  "tipoEventoId": 1,
  "ativo": true
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "nome": "Culto Dominical",
  "descricao": "Culto semanal de adoração",
  "dataInicio": "2026-05-05T19:00:00Z",
  "dataFim": "2026-05-05T21:00:00Z",
  "local": "Auditório Principal",
  "tipoEventoId": 1,
  "ativo": true,
  "dataCriacao": "2026-05-04T10:30:00Z"
}
```

**Erros:**
- `400 Bad Request`: Dados inválidos

---

### GET `/` - Listar eventos
Lista todos os eventos cadastrados.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "nome": "Culto Dominical",
    "descricao": "Culto semanal de adoração",
    "dataInicio": "2026-05-05T19:00:00Z",
    "dataFim": "2026-05-05T21:00:00Z",
    "local": "Auditório Principal",
    "tipoEventoId": 1,
    "ativo": true,
    "dataCriacao": "2026-05-04T10:30:00Z"
  }
]
```

---

### GET `/ativos` - Listar eventos ativos
Lista apenas os eventos ativos, ordenados pela data de início.

**Response (200 OK):** Array de eventos ativos

---

### GET `/{id}` - Obter evento por ID
Obtém dados de um evento específico.

**Response (200 OK):**
```json
{
  "id": 1,
  "nome": "Culto Dominical",
  "descricao": "Culto semanal de adoração",
  "dataInicio": "2026-05-05T19:00:00Z",
  "dataFim": "2026-05-05T21:00:00Z",
  "local": "Auditório Principal",
  "tipoEventoId": 1,
  "ativo": true,
  "dataCriacao": "2026-05-04T10:30:00Z"
}
```

**Erros:**
- `404 Not Found`: Evento não encontrado

---

### PUT `/{id}` - Atualizar evento
Atualiza os dados de um evento.

**Request:**
```json
{
  "nome": "Culto Dominical - Atualizado",
  "descricao": "Culto semanal com louvor",
  "dataInicio": "2026-05-05T19:30:00Z",
  "dataFim": "2026-05-05T21:30:00Z",
  "local": "Auditório Novo",
  "tipoEventoId": 1,
  "ativo": true
}
```

**Response (200 OK):** Evento atualizado

**Erros:**
- `404 Not Found`: Evento não encontrado

---

### DELETE `/{id}` - Remover evento
Remove um evento.

**Response (204 No Content):**
Sem corpo de resposta

**Erros:**
- `404 Not Found`: Evento não encontrado

---

## ✅ Presenças (`/api/presencas`)

⚠️ **Todos os endpoints requerem autenticação JWT**

### POST `/` - Registrar presença
Registra a presença de uma pessoa em um evento.

**Request:**
```json
{
  "pessoaId": 1,
  "eventoId": 1,
  "presente": true,
  "observacao": "Presente com visitante"
}
```

**Response (201 Created):**
```json
{
  "id": 1,
  "pessoaId": 1,
  "eventoId": 1,
  "presente": true,
  "observacao": "Presente com visitante",
  "dataRegistro": "2026-05-04T10:30:00Z"
}
```

**Erros:**
- `400 Bad Request`: Dados inválidos
- `409 Conflict`: Presença já registrada para esta pessoa neste evento

---

### GET `/{id}` - Obter presença por ID
Obtém dados de um registro de presença.

**Response (200 OK):**
```json
{
  "id": 1,
  "pessoaId": 1,
  "eventoId": 1,
  "presente": true,
  "observacao": "Presente com visitante",
  "dataRegistro": "2026-05-04T10:30:00Z"
}
```

**Erros:**
- `404 Not Found`: Presença não encontrada

---

### GET `/evento/{eventoId}` - Listar presenças de um evento
Lista todas as presenças registradas em um evento.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "pessoaId": 1,
    "eventoId": 1,
    "presente": true,
    "observacao": "Presente com visitante",
    "dataRegistro": "2026-05-04T10:30:00Z"
  }
]
```

---

### GET `/pessoa/{pessoaId}` - Listar presenças de uma pessoa
Lista todas as presenças registradas de uma pessoa.

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "pessoaId": 1,
    "eventoId": 1,
    "presente": true,
    "observacao": "Presente com visitante",
    "dataRegistro": "2026-05-04T10:30:00Z"
  }
]
```

---

### PUT `/{id}` - Atualizar presença
Atualiza o status de presença (presente/ausente) e observação.

**Request:**
```json
{
  "presente": false,
  "observacao": "Ausente justificado"
}
```

**Response (200 OK):** Presença atualizada

**Erros:**
- `404 Not Found`: Presença não encontrada

---

### DELETE `/{id}` - Remover presença
Remove um registro de presença.

**Response (204 No Content):**
Sem corpo de resposta

**Erros:**
- `404 Not Found`: Presença não encontrada

---

## 🔗 Relações entre Recursos

```
Usuários
├── Pessoas (cada pessoa é associada a um usuário)
│   ├── Famílias (cada pessoa pertence a uma família)
│   └── Presenças (registro de presença em eventos)
└── Eventos
    └── Presenças
```

---

## ⚠️ Tratamento de Erros Padrão

### 400 Bad Request
```json
{
  "mensagem": "Dados inválidos ou obrigatórios ausentes"
}
```

### 401 Unauthorized
```json
{
  "mensagem": "Token ausente, inválido ou expirado"
}
```

### 404 Not Found
```json
{
  "mensagem": "Recurso não encontrado"
}
```

### 409 Conflict
```json
{
  "mensagem": "Violação de restrição (ex: duplicidade)"
}
```

---

## 📝 Notas Importantes

- ✅ Autenticação é via **JWT Bearer Token**
- ✅ Datas estão em formato **ISO 8601** (UTC)
- ✅ Senhas são **hasheadas com BCrypt** (não retornadas em resposta)
- ✅ Paginação: **não implementada** (retorna todos os resultados)
- ✅ Alguns endpoints usam **soft delete** (Usuários), outros **hard delete** (Pessoas, Eventos)
- ✅ Tokens JWT expiram em **24 horas** (configurável)
- ✅ Token de recuperação de senha expira em **2 horas**

---

**Última atualização:** 2026-05-04
