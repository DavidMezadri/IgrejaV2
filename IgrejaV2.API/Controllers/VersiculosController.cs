using IgrejaV2.Aplicacao.DTOs.Versiculos;
using IgrejaV2.Aplicacao.Servico;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaV2.API.Controllers;

/// <summary>
/// CRUD de versículos da bíblia.
/// </summary>
[ApiController]
[Route("api/versiculos")]
[Authorize]
[Produces("application/json")]
[Tags("Versículos")]
public class VersiculosController(VerisculoServico servico) : ControllerBase
{
    /// <summary>Cria um novo versículo.</summary>
    /// <response code="201">Versículo criado.</response>
    /// <response code="400">Dados inválidos.</response>
    [HttpPost]
    [ProducesResponseType(typeof(VericuloResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Criar([FromBody] CriarVericuloDto dto, CancellationToken ct)
    {
        var vericulo = await servico.CriarAsync(dto, ct);
        return CreatedAtAction(nameof(ObterPorId), new { id = vericulo.Id }, vericulo);
    }

    /// <summary>Lista todos os versículos (sem paginação — não recomendado para produção).</summary>
    /// <response code="200">Lista completa de versículos.</response>
    [HttpGet("todos")]
    [ProducesResponseType(typeof(IEnumerable<VericuloResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListarTodos(CancellationToken ct)
    {
        var versiculos = await servico.ListarTodosAsync(ct);
        return Ok(versiculos);
    }

    /// <summary>Lista versículos com paginação e filtros.</summary>
    /// <param name="pagina">Número da página (padrão: 1).</param>
    /// <param name="tamanhoPagina">Quantidade por página (padrão: 50, máximo: 500).</param>
    /// <param name="livro">Filtrar por número do livro (1-66).</param>
    /// <param name="capitulo">Filtrar por capítulo.</param>
    /// <param name="traducaoId">Filtrar por tradução.</param>
    /// <response code="200">Página de versículos.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ListarVersiculosPaginadoResponseDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Listar(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanhoPagina = 50,
        [FromQuery] int? livro = null,
        [FromQuery] int? capitulo = null,
        [FromQuery] int? traducaoId = null,
        CancellationToken ct = default)
    {
        var resultado = await servico.ListarPaginadoAsync(pagina, tamanhoPagina, livro, capitulo, traducaoId, ct);
        return Ok(resultado);
    }

    /// <summary>Obtém um versículo pelo ID.</summary>
    /// <param name="id">Identificador único do versículo.</param>
    /// <response code="200">Dados do versículo.</response>
    /// <response code="404">Versículo não encontrado.</response>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(VericuloResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ObterPorId(int id, CancellationToken ct)
    {
        var vericulo = await servico.ObterPorIdAsync(id, ct);
        return vericulo is null ? NotFound() : Ok(vericulo);
    }

    /// <summary>Atualiza um versículo.</summary>
    /// <param name="id">Identificador único do versículo.</param>
    /// <response code="200">Versículo atualizado.</response>
    /// <response code="404">Versículo não encontrado.</response>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(VericuloResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Atualizar(int id, [FromBody] AtualizarVericuloDto dto, CancellationToken ct)
    {
        var vericulo = await servico.AtualizarAsync(id, dto, ct);
        return vericulo is null ? NotFound() : Ok(vericulo);
    }

    /// <summary>Remove um versículo.</summary>
    /// <param name="id">Identificador único do versículo.</param>
    /// <response code="204">Versículo removido com sucesso.</response>
    /// <response code="404">Versículo não encontrado.</response>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Remover(int id, CancellationToken ct)
    {
        var removido = await servico.RemoverAsync(id, ct);
        return removido ? NoContent() : NotFound();
    }

    /// <summary>Obtém todos os versículos de um livro em uma tradução específica.</summary>
    /// <param name="livro">Número do livro (1-66).</param>
    /// <param name="traducaoId">Identificador da tradução.</param>
    /// <response code="200">Lista de versículos do livro.</response>
    [HttpGet("livro/{livro:int}/traducao/{traducaoId:int}")]
    [ProducesResponseType(typeof(IEnumerable<VericuloResponseDto>), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> ObterPorLivro(int livro, int traducaoId, CancellationToken ct)
    {
        var versiculos = await servico.ObterPorLivroAsync(livro, traducaoId, ct);
        return Ok(versiculos);
    }

    /// <summary>Obtém todos os versículos de um capítulo específico em uma tradução.</summary>
    /// <param name="livro">Número do livro (1-66).</param>
    /// <param name="capitulo">Número do capítulo.</param>
    /// <param name="traducaoId">Identificador da tradução.</param>
    /// <response code="200">Lista de versículos do capítulo.</response>
    [HttpGet("livro/{livro:int}/capitulo/{capitulo:int}/traducao/{traducaoId:int}")]
    [ProducesResponseType(typeof(IEnumerable<VericuloResponseDto>), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> ObterPorLivroCapitulo(int livro, int capitulo, int traducaoId, CancellationToken ct)
    {
        var versiculos = await servico.ObterPorLivroCaptituloAsync(livro, capitulo, traducaoId, ct);
        return Ok(versiculos);
    }

    /// <summary>Obtém um intervalo de versículos de um capítulo específico.</summary>
    /// <param name="livro">Número do livro (1-66).</param>
    /// <param name="capitulo">Número do capítulo.</param>
    /// <param name="traducaoId">Identificador da tradução.</param>
    /// <param name="inicio">Número do versículo inicial (inclusivo).</param>
    /// <param name="fim">Número do versículo final (inclusivo).</param>
    /// <response code="200">Lista de versículos no intervalo especificado.</response>
    [HttpGet("livro/{livro:int}/capitulo/{capitulo:int}/traducao/{traducaoId:int}/intervalo")]
    [ProducesResponseType(typeof(IEnumerable<VericuloResponseDto>), StatusCodes.Status200OK)]
    [AllowAnonymous]
    public async Task<IActionResult> ObterIntervaloVersiculos(
        int livro,
        int capitulo,
        int traducaoId,
        [FromQuery] int inicio = 1,
        [FromQuery] int fim = 10,
        CancellationToken ct = default)
    {
        var versiculos = await servico.ObterIntervaloVersiculosAsync(livro, capitulo, traducaoId, inicio, fim, ct);
        return Ok(versiculos);
    }
}
