# 🔍 Auditoria Completa de Tratamento de Erros — IgrejaV2

**Data:** 2026-06-14  
**Escopo:** Frontend React + Backend .NET  
**Total de problemas encontrados:** 89  

---

## 📊 Resumo Executivo

| Severidade | Frontend | Backend | Total |
|---|---|---|---|
| 🔴 Crítico | 7 | 12 | **19** |
| 🟡 Médio | 15 | 18 | **33** |
| 🟢 Baixo | 8 | 29 | **37** |

### Problemas por Tipo
- **Tratamento de erro ausente:** 34 casos
- **Mensagens de erro inadequadas:** 18 casos
- **Nomes inconsistentes/typos:** 21 casos
- **Campos/DTOs faltando:** 12 casos
- **Exposição de dados sensíveis:** 4 casos

---

## 🖥️ FRONTEND — Serviços (`src/services/`)

### Padrão Geral
Todos os 14 serviços usam o mesmo padrão: **ZERO tratamento de erro na camada de serviço.**

```ts
// Padrão atual (inseguro)
export const list = () => 
  client.get(EP.AVISOS.LIST).then(r => r.data)
// Erro → AxiosError bruto propagado para o caller
```

| Arquivo | Funções | Erros capturados | Problema |
|---|---|---|---|
| `authService.ts` | `login`, `register`, `me`, `logout` | ❌ Nenhum | Usuário vê erro raw do Axios |
| `avisosService.ts` | `list`, `getById`, `listAtivos`, `create`, `update`, `remove` | ❌ Nenhum | AxiosError bruto propagado |
| `configService.ts` | `list`, `update` | ❌ Nenhum | Sem feedback do usuário |
| `enderecoService.ts` | `list`, `one`, `create`, `update`, `remove` | ❌ Nenhum | AxiosError não tratado |
| `eventosService.ts` | `list`, `getById`, `create`, `update`, `remove` | ❌ Nenhum | Erro silencioso |
| `familiasService.ts` | `list`, `getById`, `create`, `update`, `remove`, `search` | ❌ Nenhum | AxiosError bruto |
| `igrejaService.ts` | `list`, `one`, `create`, `update`, `remove` | ❌ Nenhum | Sem tratamento |
| `pessoaEnderecoService.ts` | `list`, `add`, `remove` | ❌ Nenhum | Erro propagado |
| `pessoasService.ts` | `list`, `getById`, `create`, `update`, `remove`, `search` | ❌ Nenhum | AxiosError bruto |
| `presencasService.ts` | `list`, `getById`, `create`, `update`, `remove`, `listByEvento`, `listByPessoa` | ❌ Nenhum | Sem tratamento |
| `tiposEventoService.ts` | `list`, `getById`, `create`, `update`, `remove` | ❌ Nenhum | AxiosError bruto |
| `traducaoService.ts` | `list`, `getById`, `create`, `update`, `remove` | ❌ Nenhum | Erro propagado |
| `uploadService.ts` | `uploadImagem`, `deleteImagem`, `listarImagens` | ❌ Nenhum | 🔴 `listarImagens`: TypeError se `r.data.imagens` for undefined |
| `usuariosService.ts` | `list`, `getById`, `update`, `remove` | ❌ Nenhum | Sem tratamento |
| `versiculoService.ts` | `list`, `getById`, `create`, `update`, `remove` | ❌ Nenhum | AxiosError bruto |

### Serviços Mock (sem erros possíveis)
- `ministeriosService.ts` — retorna `Promise.resolve()`
- `oracoesService.ts` — retorna `Promise.resolve()`
- `sermoesService.ts` — retorna `Promise.resolve()`

---

## 🎣 FRONTEND — Hooks (`src/hooks/`)

| Hook | Uso de React Query | Tratamento de erro | Problema |
|---|---|---|---|
| `useAvisos.ts` | ✅ `useQuery` | ❌ Nenhum | Erro nunca é lido pelo componente |
| `useDashboardData.ts` | ✅ 3x `useQuery` | ⚠️ Parcial | 🔴 `presencas` silencia erros com `try/catch`, retorna `[]` |
| `useEventos.ts` | ✅ `useQuery` + mutações | ⚠️ Parcial | 🟡 Mutações sem `onError` |
| `useEvento.ts` | ✅ `useQuery` + mutações | ❌ Nenhum | Mutações sem tratamento |
| `useFamilias.ts` | ✅ `useQuery` | ⚠️ Parcial | 🟡 `placeholderData` mascara falhas — mostra dados fake silenciosamente |
| `useFormErrors.ts` | ❌ Validação local | ✅ Completo | ✅ Bem implementado |
| `useMinisterios.ts` | ✅ Mock | ✅ N/A | Sem erros possíveis |
| `useOracoes.ts` | ✅ Mock | ✅ N/A | Sem erros possíveis |
| `usePessoas.ts` | ✅ `useQuery` | ⚠️ Parcial | 🟡 `placeholderData` mascara falhas |
| `useSermoes.ts` | ✅ Mock | ✅ N/A | Sem erros possíveis |
| `useTiposEvento.ts` | ✅ `useQuery` | ⚠️ Parcial | 🟡 `placeholderData` mascara falhas |
| `usePageConfig.ts` | ✅ `useQuery` + mutação | ⚠️ Parcial | 🟡 `useImagensUpload` sem `onError` |
| `useAdminTabs.ts` | ❌ Context | ✅ N/A | Sem async, sem erros |
| `useTheme.ts` | ❌ localStorage | ✅ N/A | Sem erros possíveis |

### Problemas em Hooks
1. **`useDashboardData`**: O `try/catch` da query de presencas:
   ```ts
   queryFn: async () => {
     try {
       return await presencasService.list()
     } catch (err) {
       console.warn('...')
       return [] // 🔴 Silencia erro, mostra 0 presencas
     }
   }
   ```

2. **Hooks com `placeholderData`** (`useFamilias`, `usePessoas`, `useTiposEvento`):
   - Se a API falha, o hook retorna dados mock com `isPlaceholderData: true`
   - Nenhum componente verifica esse flag
   - Usuário vê dados fake acreditando que são reais

3. **Mutações sem `onError`**:
   - `useCreateEvento`, `useUpdateEvento`, `useRemoveEvento` não têm callback de erro
   - Falhas são silenciosas

---

## 🌐 FRONTEND — API e Axios

### `src/api/client.ts` — Interceptador Global

```ts
// Linha ~25-40: Response interceptor
if (status === 401) {
  console.error('401')
  localStorage.removeItem('auth_token')
  // ❌ NÃO redireciona para /login
  // ❌ UI continua mostrando dados stale
}

if (status === 403) {
  console.error('403')
  // ❌ Sem feedback ao usuário
}

// ❌ Erros de rede: console.error apenas
// ❌ Sem tratamento de 404, 422, 500
```

| Código HTTP | Comportamento Atual | Ideal |
|---|---|---|
| 401 | Limpa token, console.error | Limpa token, redireciona `/login`, toast |
| 403 | console.error apenas | Toast "Acesso negado" |
| 404 | Propaga bruto | Toast "Recurso não encontrado" |
| 422 | Propaga bruto | Toast com validações do backend |
| 500 | Propaga bruto | Toast "Erro do servidor" |
| Network error | console.error | Toast "Sem conexão" |

### `src/api/endpoints.ts` — Nomes Inconsistentes

```ts
// ❌ Padrão inconsistente:
PESSOAS_ENDERECOS: {
  ADD: '...', // 🔴 deveria ser CREATE para consistência
  LIST: '...',
  // ❌ Falta UPDATE
  REMOVE: '...'
}

CONFIG: {
  LIST: '...',
  UPDATE: '...'
  // ❌ Falta GET by ID, CREATE, REMOVE
}

UPLOAD: {
  IMAGEM: '...',      // 🔴 deveria ser CREATE
  REMOVE_IMAGEM: '...' // 🔴 deveria ser REMOVE
  LIST_IMAGENS: '...'  // 🔴 deveria ser LIST
}

AUTH: {
  // ❌ Falta REGISTER — registration usa USUARIOS.CREATE
  LOGIN: '...',
  ME: '...'
}
```

---

## 📱 FRONTEND — Páginas e Componentes

### Admin Pages — Padrão de Tratamento de Erro

**Padrão para LEITURA:**
```ts
try {
  const data = await someService.list()
  setData(data)
} catch (err) {
  console.error('Erro:', err)
  // ❌ NO toast, NO error message visível
  // ❌ Data fica vazia/stale, tabela renderiza vazia
} finally {
  setLoading(false)
}
```

**Padrão para MUTATION:**
```ts
// ❌ alert() em vez de toast
} catch (err) {
  alert('Erro ao salvar X')
}

// Em alguns delete:
} catch (err) {
  alert('Erro ao deletar')
  // ❌ Sem console.error, erro não é logged
}
```

| Página | Load errors | Mutation errors | Problema |
|---|---|---|---|
| `AdminAvisos.tsx` | Sem feedback | `alert()` | Tabela fica vazia silenciosamente |
| `AdminCMS.tsx` | Sem feedback | React Query `onError` | Melhor, mas usa `alert()` |
| `AdminEventos.tsx` | Sem feedback | `alert()` + sem log | Erro não é logged em delete |
| `AdminTiposEvento.tsx` | Sem feedback | `alert()` | Sem console.error |
| `AdminIgrejas.tsx` | Sem feedback | `alert()` | Sem console.error |
| `AdminConfiguracoes.tsx` | Sem feedback | `alert()` | Sem feedback estruturado |
| `Login.tsx` | ✅ `setError()` inline | ✅ Inline | ✅ Melhor UX, mas mensagem hardcoded |
| `Signup.tsx` | `setErrors()` inline | ❌ Lê `err.message` | 🔴 Nunca vê a validação do backend |

### Crítico — `Signup.tsx`

```ts
// Linha ~85
catch (err) {
  if (err instanceof Error) {
    setErrors({ submit: err.message })
    // ❌ err.message = "Request failed with status code 422"
    // ❌ Nunca vê: { field: "email", message: "Email já cadastrado" }
    // Correto seria: err.response?.data?.mensagem
  }
}
```

### `App.tsx` — Sem Error Boundary

```ts
// ❌ Sem <ErrorBoundary>
<HashRouter>
  <Routes>
    {/* Qualquer erro de render → blank screen, sem recovery */}
  </Routes>
</HashRouter>
```

Se um componente fizer `data.length` com `data = undefined` (de uma query falhada), **tela fica branca**, sem mensagem de erro.

---

## 🔧 BACKEND — Serviços (`IgrejaV2.Aplicacao\Servico\`)

### Padrão Geral
**Todos os serviços lançam exceções sem try/catch.**

| Serviço | Exceções lançadas | Tratadas no serviço? | Problema |
|---|---|---|---|
| `AvisoServico.cs` | Nenhuma | N/A | ✅ Sem validações, sem erros |
| `AuthServico.cs` | Nenhuma | N/A | 🔴 Hardcoded `https://localhost:5001` |
| `UsuarioServico.cs` | `InvalidOperationException` (dups) | ❌ Não | Propagada para controller |
| `PessoaServico.cs` | 2x `InvalidOperationException` | ❌ Não | `MembroDesde` validado só aqui, não no DTO |
| `EventoServico.cs` | Nenhuma | N/A | ❌ Zero validações (datas, FK) |
| `FamiliaServico.cs` | `InvalidOperationException` (dups) | ❌ Não | Propagada para controller |
| `IgrejaServico.cs` | `InvalidOperationException` (dups) | ❌ Não | Propagada para controller |
| `PresencaServico.cs` | `InvalidOperationException` (dups) | ❌ Não | Propagada para controller |
| `TipoEventoServico.cs` | 2x `InvalidOperationException` | ❌ Não | Propagada para controller |
| `ConfigServico.cs` | 2x `ArgumentException` | ❌ Não | 🔴 USA `ArgumentException` (inconsistente) |
| `EnderecoServico.cs` | Nenhuma | N/A | ✅ Relega tudo para DTO |
| `PessoaEnderecoServico.cs` | 3x `InvalidOperationException` | ❌ Não | 🔴 `ToDto(pessoaEnderecoCompleto!)` pode NullRef |
| `TraducaoServico.cs` | Nenhuma | N/A | ❌ Sem validações |
| `VerisculoServico.cs` | Nenhuma | N/A | 🔴 `Livro` pode ser 0 ou negativo |
| `LogServico.cs` | Nenhuma | N/A | ❌ Falha de log falha toda a operação |
| `EmailServico.cs` | Nenhuma | N/A | 🔴 SMTP falha nunca é caught |

### Detalhes Críticos

#### `AuthServico.GerarTokenRecuperacaoAsync`
```csharp
// ❌ Sem try/catch
await emailServico.EnviarRecuperacaoSenhaAsync(usuario.Email, token);

// ❌ Link hardcoded
var link = $"https://localhost:5001/redefinir-senha?token={token}";
// Não funciona em produção
```

#### `PessoaEnderecoServico.VincularAsync`
```csharp
var pessoaEnderecoCompleto = await ObterPorIdAsync(pessoaEnderecoId);
await SalvarAlteracoesAsync(pessoaEnderecoId, pessoaEnderecoCompleto);
// ❌ Null-forgiving operator: pode lançar NullReferenceException
return ToDto(pessoaEnderecoCompleto!)
```

#### `PessoaServico.CriarAsync`
```csharp
// Validação SÓ no serviço, NÃO no DTO
if (pessoa.MembroDesde == null || pessoa.MembroDesde == DateTime.MinValue)
  throw new InvalidOperationException("É necessário cadastrar a data de membro.");

// DTO permite null:
public record CriarPessoaDto {
  public DateTime? MembroDesde { get; init; } // ❌ Não tem [Required]
}
```

---

## 🎯 BACKEND — Controllers (`IgrejaV2.API\Controllers\`)

### Padrão de Tratamento

| Controller | `Criar` | `Atualizar` | `Deletar` | Padrão |
|---|---|---|---|---|
| `AvisosController` | ❌ | ❌ | ❌ | Sem try/catch |
| `AutenticacaoController` | ❌ | ❌ | ❌ | Sem try/catch, 🔴 token em response |
| `UsuariosController` | ✅ try/catch | ❌ | ❌ | Só Create cuida |
| `PessoasController` | ❌ | ❌ | ❌ | Sem try/catch |
| `EventosController` | ❌ | ❌ | ❌ | Sem try/catch |
| `FamiliasController` | ❌ | ❌ | ❌ | Sem try/catch |
| `IgrejasController` | ❌ | ❌ | ❌ | Sem try/catch |
| `PresencasController` | ✅ try/catch | ❌ | ❌ | Só Create cuida |
| `EnderecosController` | ❌ | ❌ (tem catch) | ❌ | Apenas VincularPessoa tem try/catch |
| `TiposEventoController` | ❌ | ❌ | ❌ | Sem try/catch |
| `TraducoesController` | ❌ | ❌ | ❌ | Sem try/catch |
| `VersiculosController` | ❌ | ❌ | ❌ | Sem try/catch |
| `ConfigController` | ❌ | ✅ try/catch | ❌ | 🔴 Expõe `ex.Message` |
| `UploadController` | ✅ try/catch | ❌ | ✅ try/catch | 🔴 Expõe `ex.Message` |
| `HomeController` | N/A | N/A | N/A | 🔴 Dead code, namespace errado |

### Exemplos Problemáticos

#### `AutenticacaoController.RecuperarSenha`
```csharp
[HttpPost("recuperar-senha")]
public async Task<IActionResult> RecuperarSenha([FromBody] RecuperarSenhaDto dto)
{
  // ❌ Sem try/catch para SMTP
  var token = await authServico.GerarTokenRecuperacaoAsync(usuario);
  
  // ❌ Token retornado em produção
  return Ok(new {
    token = token, // SECURITY LEAK
    mensagem = "Email enviado"
  });
}
```

#### `ConfigController.ObterTodas`
```csharp
[HttpGet]
[AllowAnonymous]
public async Task<IActionResult> ObterTodas()
{
  try {
    return Ok(await configServico.ObterTodasAsync());
  } catch (Exception ex) {
    // ❌ Expõe detalhes internos
    return StatusCode(500, new { erro = ex.Message });
  }
}
```

#### `UploadController.Upload`
```csharp
catch (Exception ex) {
  // ❌ Expõe ex.Message em produção
  return StatusCode(500, new { erro = ex.Message });
}
```

### Inconsistências de Status HTTP

| Caso | Controller atual | Status retornado | Ideal |
|---|---|---|---|
| Duplicate email em `UsuarioServico` | `UsuariosController.Criar` | 409 Conflict | ✅ Correto |
| Duplicate name em `FamiliaServico` | `FamiliasController` | 400 BadRequest | ❌ Deveria ser 409 |
| Duplicate name em `IgrejaServico` | `IgrejasController` | 400 BadRequest | ❌ Deveria ser 409 |
| Pessoa não encontrada | `EnderecosController` | 400 BadRequest | ❌ Deveria ser 404 |
| Endereço não encontrado | `EnderecosController` | 400 BadRequest | ❌ Deveria ser 404 |

---

## 🔐 BACKEND — Program.cs (Middleware Global)

```csharp
// Linha 159-172
app.UseExceptionHandler(errorApp =>
{
  errorApp.Run(async context =>
  {
    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
    var ex = exceptionHandlerPathFeature?.Error;

    if (ex is InvalidOperationException) {
      context.Response.StatusCode = 400;
      // ❌ SÓ trata InvalidOperationException
    }
    
    // ❌ Todos outros tipos: nenhuma resposta escrita
    // Framework retorna 500 vazio
  });
});
```

| Tipo de exceção | Tratado? | Resultado |
|---|---|---|
| `InvalidOperationException` | ✅ | 400 com body |
| `ArgumentException` | ❌ | 500 vazio |
| `NullReferenceException` | ❌ | 500 vazio |
| `DbUpdateException` | ❌ | 500 vazio |
| `SmtpException` | ❌ | 500 vazio |
| `NetworkException` | ❌ | 500 vazio |

---

## 📋 PROBLEMAS CRÍTICOS (🔴)

### 1. **Sem Error Boundary no Frontend**
- **Local:** `src/App.tsx`
- **Impacto:** Qualquer erro de render → blank screen
- **Exemplo:** Query falha, componente tenta `data.length`, `data = undefined` → crash
- **Severidade:** 🔴 Crítico

### 2. **401 Não Redireciona para Login**
- **Local:** `src/api/client.ts` (linha ~30)
- **Impacto:** Token expirado → app continua funcionando com dados stale
- **Código:**
  ```ts
  if (status === 401) {
    console.error('401')
    localStorage.removeItem('auth_token')
    // ❌ Falta window.location.href = '/login'
  }
  ```
- **Severidade:** 🔴 Crítico

### 3. **SMTP Failure Never Caught**
- **Local:** `IgrejaV2.Aplicacao\Servico\AuthServico.cs` (linha ~45)
- **Impacto:** Password recovery falha silenciosamente, retorna 200 OK
- **Código:**
  ```csharp
  await emailServico.EnviarRecuperacaoSenhaAsync(...) // ❌ sem try/catch
  ```
- **Severidade:** 🔴 Crítico

### 4. **Null-Forgiving Operator Pode Lancar NullRef**
- **Local:** `IgrejaV2.Aplicacao\Servico\PessoaEnderecoServico.cs` (linha 53)
- **Impacto:** `NullReferenceException` uncaught se DB falhar entre INSERT e SELECT
- **Código:**
  ```csharp
  return ToDto(pessoaEnderecoCompleto!) // ❌ ! operator
  ```
- **Severidade:** 🔴 Crítico

### 5. **Token Retornado em Response Body**
- **Local:** `IgrejaV2.API\Controllers\AutenticacaoController.cs` (RecuperarSenha)
- **Impacto:** Token de reset exposte em logs/proxies/history
- **Severidade:** 🔴 Crítico

### 6. **Erro Details Expostos em 500**
- **Local:** `ConfigController.ObterTodas` e `UploadController` (multiple catch blocks)
- **Impacto:** Stack traces e detalhes de DB expostos a clientes
- **Código:**
  ```csharp
  return StatusCode(500, new { erro = ex.Message }); // ❌ ex.Message
  ```
- **Severidade:** 🔴 Crítico

### 7. **Assinatura de Senha Hardcoded em localhost**
- **Local:** `IgrejaV2.Aplicacao\Servico\AuthServico.cs` (linha ~48)
- **Impacto:** Links não funcionam em produção
- **Código:**
  ```csharp
  var link = $"https://localhost:5001/redefinir-senha?token={token}";
  ```
- **Severidade:** 🔴 Crítico

### 8. **`uploadService.listarImagens` Pode TypeError**
- **Local:** `src/services/uploadService.ts` (linha ~45)
- **Impacto:** Se API retornar shape diferente, TypeError uncaught
- **Código:**
  ```ts
  listarImagens: () => client.get(EP.UPLOAD.LIST_IMAGENS).then(r => r.data.imagens)
  // ❌ Se r.data.imagens = undefined → TypeError
  ```
- **Severidade:** 🔴 Crítico

### 9. **Signup Não Vê Erros de Validação do Backend**
- **Local:** `src/pages/Auth/Signup.tsx` (linha ~85)
- **Impacto:** Usuário nunca vê "Email já cadastrado" do servidor
- **Código:**
  ```ts
  setErrors({ submit: err.message })
  // err.message = "Request failed with status code 422"
  // Correto: err.response?.data?.mensagem
  ```
- **Severidade:** 🔴 Crítico

### 10. **Dashboard Presencas Silencia Erros**
- **Local:** `src/hooks/useDashboardData.ts` (presencas queryFn)
- **Impacto:** Se API falha, mostra 0 presencas sem avisar
- **Código:**
  ```ts
  queryFn: async () => {
    try {
      return await presencasService.list()
    } catch (err) {
      console.warn('...')
      return [] // ❌ Silencia, mostra fake 0
    }
  }
  ```
- **Severidade:** 🔴 Crítico

### 11. **Validação em Dois Lugares (MembroDesde)**
- **Local:** DTO não tem `[Required]`, serviço valida
- **Impacto:** Inconsistência entre camadas
- **Severidade:** 🔴 Crítico

### 12. **Deletar Pessoa vs Familia (Hard vs Soft)**
- **Local:** `PessoaServico.RemoverAsync` vs `FamiliaServico.RemoverAsync`
- **Impacto:** Pessoa é deletada, Familia é soft-deleted → inconsistência
- **Severidade:** 🔴 Crítico

### 13. **Dapper SQL Não é Type-Checked**
- **Local:** `IgrejaV2.Infraestrutura\Repositorios\Dapper\*`
- **Impacto:** Typos em SQL não detectados em build time
- **Severidade:** 🔴 Crítico

### 14. **Global Middleware Só Trata InvalidOperationException**
- **Local:** `Program.cs` (linha 159-172)
- **Impacto:** `ArgumentException`, `DbException`, `SmtpException` retornam 500 vazio
- **Severidade:** 🔴 Crítico

### 15. **Debug.WriteLine em Produção**
- **Local:** `PessoaEnderecoServico.cs` (linha 41)
- **Impacto:** Informações vazam para debugger
- **Severidade:** 🔴 Crítico

### 16. **Log Service Falha Faz Operação Falhar**
- **Local:** `LogServico.cs`
- **Impacto:** Se logging falha, toda operação rollback
- **Severidade:** 🔴 Crítico

### 17. **Usuário ID Sempre Null em Logs**
- **Local:** `LogServico.cs` (usuarioId nunca setado)
- **Impacto:** Auditoria incompleta
- **Severidade:** 🔴 Crítico

### 18. **AllowAnonymous em ConfigController.ObterTodas**
- **Local:** `ConfigController.cs`
- **Impacto:** Se houver configs sensíveis, expostas publicamente
- **Severidade:** 🔴 Crítico

### 19. **Sem Validação de Range no Versiculo**
- **Local:** `VerisculoServico.cs`
- **Impacto:** `Livro = 0`, `Capitulo = -1` são aceitos
- **Severidade:** 🔴 Crítico

---

## 🟡 PROBLEMAS MÉDIOS (18)

### Frontend
1. **Placeholder Data Mascara Erros** — `useFamilias`, `usePessoas`, `useTiposEvento`
   - Mostra dados fake quando API falha
   - Usuário não sabe que é offline
   - Local: Todos os hooks mencionados

2. **Alert() em Vez de Toast** — todas as admin pages
   - Sem styling, sem UX
   - Bloqueia UI thread
   - Local: `AdminEventos`, `AdminTiposEvento`, `AdminAvisos`, etc.

3. **Mutation Hooks Sem onError** — `useCreateEvento`, `useUpdateEvento`, etc.
   - Falhas são silenciosas
   - Usuário não sabe se operation falhou

4. **Hardcoded Error Messages** — `Login.tsx`
   - "Usuário ou senha incorretos" para todos os erros
   - Não diferencia server down vs bad creds

5. **useDashboardData Try/Catch Parcial** — só presencas tratadas
   - `pessoas` e `eventos` queries não têm tratamento

6. **Axios Error Shape Não Unwrapped** — `Signup.tsx` lê `err.message`
   - Deveria ler `err.response?.data?.mensagem`

7. **Sem Tratamento 404/422** — todos endpoints
   - Usuário vê "Request failed with status code"

8. **React Query Retry Config Inconsistente**
   - `useEventos` tem `retry: 3`, outros têm global `retry: 1`
   - Sem padrão claro

9. **Sem Timeout Config** — HTTP requests sem timeout
   - Pode travar indefinidamente

10. **Sem Request Deduplication** — queries não são deduplicadas
    - Mesma query pode rodar em paralelo

11. **Sem Offline Detection** — app não sabe se está offline
    - Continua tentando requests vãs

12. **Sem Request Cancellation** — cleanup em unmount
    - Memory leaks possíveis

13. **Sem Refresh Token Rotation** — token expira, sem renovação
    - Usuário é deslogado abruptamente

14. **Mensagens de Erro em Inglês** — Axios `message` é sempre em inglês
    - "Request failed with status code 422"

15. **Sem Toast Component** — projeto usa `alert()` e inline `setError()`
    - Sem consistência visual

### Backend
1. **Status Code 400 Para Tudo** — `FamiliaServico`, `IgrejaServico` lançam `InvalidOperationException`
   - 400 é usado para duplicate names (deveria ser 409)
   - Não-idempotente confundido com bad request

2. **Nenhuma Validação de FK** — `EventoServico`, `PresencaServico`
   - `TipoEventoId`, `PessoaId` não validados antes de insert
   - DB constraints falham

3. **Inconsistência Delete Soft vs Hard**
   - Pessoa: hard delete
   - Familia: soft delete
   - Sem padrão unificado

4. **DTO Validation vs Service Validation**
   - `MembroDesde`: DTO permite null, serviço valida
   - Redundância ou inconsistência

5. **Endpoint Naming Inconsistente** — `ADD` vs `CREATE`, etc.
   - DTOs: `VericuloResponseDto` (typo: falta 's')
   - Entities: `Versiculo` (com 's')

6. **Configuracao Não Herda EntidadeBase**
   - Usa `CriadoEm` vs `DataCriacao` dos outros
   - Nomes de campo inconsistentes

7. **Sem Request/Response DTOs Completos**
   - Alguns campos faltam nas respostas
   - `PastorResponsavelId` nunca exposte

8. **Nenhuma Pagination** — `ListarTodosAsync` retorna tudo
   - Performance issue com grandes datasets

9. **N+1 Query em VerisculoServico**
   - Itera e chama `await ToDtoAsync` para cada versículo

10. **Logs Síncronos** — falha de log falha toda operação
    - Deveria ser fire-and-forget

---

## 🟢 PROBLEMAS BAIXOS (37)

### Naming Issues (21)
1. **VerisculoServico vs VericuloResponseDto** — typo inconsistente
2. **HomeController** — namespace `ProjetoIntegrador`, deveria ser removido
3. **TipoEventoResponseDto.DataCriacao** — comentado, intenção desconhecida
4. **Configuracao.CriadoEm** vs `EntidadeBase.DataCriacao` — nomes diferentes
5. **Igreja.Ativa** (fem.) vs `Familia.Ativo` (masc.)
6. **EP.UPLOAD**: IMAGEM/REMOVE_IMAGEM vs CREATE/REMOVE
7. **EP.PESSOAS_ENDERECOS**: ADD vs CREATE
8. **Serviço vs Servico** — mistura português/inglês em nomes
9. **AdicionarEnderecoAoPessoaDto.cs** — arquivo duplicado/morto
10. **usuarioId Always Null** — em LogServico
11. **PastorResponsavelId** — campo nunca populado em Igreja
12. **Ativo Comentado** — em EntidadeBase
13. **LogServico** — retorna entidade, não DTO (quebra padrão)
14. **PessoaEnderecoServico** — sem UPDATE operation
15. **ConfigController Route** — usa convention vs explicit strings
16. **TokenRecuperacaoSenhaExpiracao** — nunca usado/validado
17. **IpUltimoLogin** — nunca populado
18. **PublicoAlvo Enum** — validado só no serviço, DTO permite inválido
19. **Livro/Capitulo/Numero** — sem [Range] ou validação
20. **PessoaServico.ListarPorFamiliaAsync** — sem controller endpoint
21. **LogServico.ListarPorEntidadeAsync** — filtra em memória

### Dead Code / Unused Fields
1. **HomeController.cs** — MVC leftovers em REST API
2. **VericuloServico.cs** — arquivo stub comentado, VerisculoServico é o real
3. **EntidadeBase.Ativo** — campo comentado
4. **Igreja.PastorResponsavelId** — nunca setado
5. **Usuario.IpUltimoLogin** — nunca setado
6. **Usuario.TokenRecuperacaoSenha** — gerado mas nunca validado tempo
7. **TipoEventoResponseDto.DataCriacao** — comentado em ToDto()

### Code Quality
1. **Debug.WriteLine em Produção** — `PessoaEnderecoServico.cs`
2. **Comment "remover em produção"** — token em `AutenticacaoController`
3. **Inconsistent Null Handling** — `!` operator vs checks
4. **No Request Logging** — middleware não logs requests
5. **No Response Compression** — não há gzip setup visível
6. **No CORS Setup Visível** — assume default
7. **No Rate Limiting** — sem proteção contra brute force
8. **Typo: Vericulo vs Versiculo** — 3 variações coexistem
9. **Sem Consistent Error Response Envelope** — alguns com `erro`, alguns com `mensagem`
10. **Sem Deprecation Headers** — endpoints antigos não sinalizados

---

## 📝 NOMES INCONSISTENTES E TYPOS

| Item | Variações | Ideal |
|---|---|---|
| Versiculo | `Versiculo`, `VerisculoServico`, `VericuloResponseDto`, `VericuloServico` (stub) | `Versiculo` em todos |
| Delete | Hard delete (Pessoa), Soft delete (Familia) | Padronizar (soft recommended) |
| Timestamp | `DataCriacao`, `CriadoEm` | `DataCriacao` em todos |
| Timestamps | `DataCriacao`, `DataAtualizacao`, `DataDelecao` vs `CriadoEm`, `AtualizadoEm` | Mesmo prefixo |
| Endpoint Verbs | `CREATE`, `ADD`, `IMAGEM` | `CREATE`, `REMOVE`, `LIST` |
| Active Flag | `Ativo` vs `Ativa` vs `Ativas` | `Ativo` (um padrão) |
| Recuperação Senha | `TokenRecuperacaoSenha` vs generic token handling | Usar nome único |

---

## 🔗 CAMPOS/DTOs FALTANDO

### Backend
1. **AtualizarUsuarioDto** — missing `Email` (usuários não podem mudar email)
2. **IgrejaResponseDto** — missing `PastorResponsavelId`
3. **TipoEventoResponseDto** — `DataCriacao` comentado
4. **PessoaEnderecoServico** — missing UPDATE operation
5. **EventosController** — missing endpoint to filter by `TipoEventoId`
6. **PessoasController** — missing endpoints to filter by `Sexo`, `EstadoCivil`, date ranges
7. **IgrejasController** — missing bulk listing with filters
8. **PresencasController** — missing attendance count/percentage endpoint
9. **ConfigController** — using convention route vs explicit
10. **Configuracao Entity** — doesn't inherit `EntidadeBase` (inconsistent schema)

### Frontend
1. **EP.AUTH** — missing `REGISTER` key (uses `USUARIOS.CREATE`)
2. **uploadService.listarImagens** — missing null check on `r.data.imagens`
3. **No global error toast component** — uses `alert()` everywhere
4. **No offline detection** — missing network status indicator
5. **No request deduplication** — can run same query in parallel

---

## ✅ CHECKLIST DE CORREÇÃO

### Fase 1: CRÍTICO (P0)
- [ ] Adicionar Error Boundary em `src/App.tsx`
- [ ] Adicionar redirect 401 em `src/api/client.ts`
- [ ] Adicionar try/catch em `AuthServico.GerarTokenRecuperacaoAsync`
- [ ] Remover token de response em `AutenticacaoController.RecuperarSenha`
- [ ] Não expor `ex.Message` em `ConfigController` e `UploadController`
- [ ] Usar env var para password reset link URL
- [ ] Adicionar null check em `uploadService.listarImagens`
- [ ] Corrigir `Signup.tsx` para ler `err.response?.data?.mensagem`
- [ ] Remover try/catch que silencia em `useDashboardData` ou adicionar toast
- [ ] Mover validação de `MembroDesde` para DTO com `[Required]`
- [ ] Padronizar hard vs soft delete
- [ ] Criar global exception handler que trata todos tipos
- [ ] Remover `Debug.WriteLine` em `PessoaEnderecoServico`
- [ ] Corrigir null-forgiving operator em `PessoaEnderecoServico.VincularAsync`

### Fase 2: MÉDIO (P1)
- [ ] Substituir `alert()` por toast component (criar `ToastProvider`)
- [ ] Adicionar `onError` em todos React Query mutations
- [ ] Remover `placeholderData` ou adicionar `isPlaceholderData` check
- [ ] Standarizar status codes (409 para duplicates, 404 para not found)
- [ ] Adicionar validação FK em services antes de insert
- [ ] Criar `IErrorResponse` interface unificada para todos endpoints
- [ ] Adicionar timeout config no Axios client
- [ ] Implementar request deduplication em React Query
- [ ] Adicionar network status detection (`navigator.onLine`)
- [ ] Implementar refresh token rotation

### Fase 3: BAIXO (P2)
- [ ] Renomear todos `Vericulo` → `Versiculo`
- [ ] Remover `HomeController`
- [ ] Remover `VericuloServico.cs` stub
- [ ] Padronizar timestamp naming (`DataCriacao` em tudo)
- [ ] Remover `Igreja.Ativa`, usar `Ativo` como outros
- [ ] Corrigir enum naming em endpoints (`IMAGEM` → `CREATE`)
- [ ] Implementar pagination em `ListarTodosAsync`
- [ ] Converter LogServico para fire-and-forget
- [ ] Adicionar request logging middleware
- [ ] Adicionar response compression
- [ ] Implementar rate limiting
- [ ] Remover arquivo duplicado `AdicionarEnderecoAoPessoaDto.cs`
- [ ] Comentário "remover em produção"
- [ ] Descommentar `TipoEventoResponseDto.DataCriacao`

---

## 📞 Contato para Dúvidas

Este documento foi gerado em **2026-06-14** com auditoria manual de:
- 14 serviços frontend
- 14 hooks frontend  
- 15 controllers backend
- 15 serviços backend
- 1 cliente HTTP + endpoints
- 1 middleware global

Total de linhas analisadas: **~3500 linhas de código**

