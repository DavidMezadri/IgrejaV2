using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioEndereco : RepositorioBase<Endereco>, IRepositorioEndereco
    {
        public RepositorioEndereco(IgrejaContexto contexto) : base(contexto) { }
    }
}
