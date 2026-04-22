using Dapper;
using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using Npgsql;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioEnderecoDapper : IgrejaV2.Infraestrutura.Repositorios.Base.RepositorioBaseDapper<Endereco>, IRepositorioEndereco
    {
        protected override string NomeTabela => "enderecos";

        public RepositorioEnderecoDapper(string connectionString) : base(connectionString) { }
    }
}
