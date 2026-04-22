using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using Npgsql;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioPessoaEnderecoDapper : IgrejaV2.Infraestrutura.Repositorios.Base.RepositorioBaseDapper<PessoaEndereco>, IRepositorioPessoaEndereco
    {
        protected override string NomeTabela => "pessoas_enderecos";

        public RepositorioPessoaEnderecoDapper(string connectionString) : base(connectionString) { }

        public async Task<IEnumerable<PessoaEndereco>> ObterPorPessoaAsync(int pessoaId, CancellationToken ct = default)
        {
            var sql = @"
                SELECT
                    pe.id, pe.pessoa_id, pe.endereco_id,
                    e.id, e.rua, e.numero, e.complemento, e.bairro, e.cidade, e.estado, e.cep
                FROM pessoas_enderecos pe
                INNER JOIN enderecos e ON e.id = pe.endereco_id AND e.deletado = false
                WHERE pe.pessoa_id = @PessoaId AND pe.deletado = false";

            using var conn = CriarConexao();
            var resultado = new List<PessoaEndereco>();

            await conn.QueryAsync<PessoaEndereco, Endereco, PessoaEndereco>(
                new CommandDefinition(sql, new { PessoaId = pessoaId }, cancellationToken: ct),
                (pe, endereco) =>
                {
                    pe.Endereco = endereco;
                    resultado.Add(pe);
                    return pe;
                },
                splitOn: "id"
            );

            return resultado;
        }
    }
}
