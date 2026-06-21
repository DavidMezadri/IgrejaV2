namespace IgrejaV2.Aplicacao.DTOs.Versiculos;

public class ListarVersiculosPaginadoResponseDto
{
    public int Pagina { get; set; }
    public int TamanhoPagina { get; set; }
    public int Total { get; set; }
    public int TotalPaginas { get; set; }
    public bool TemProxima { get; set; }
    public List<VericuloResponseDto> Dados { get; set; } = new();
}
