using IgrejaV2.Dominio.Entidades;
using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;
using IgrejaV2.Infraestrutura.Repositorios.Base;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioPresenca : RepositorioBase<Presenca>, IRepositorioPresenca
    {
        public RepositorioPresenca(IgrejaContexto contexto) : base(contexto) { }
    }
}
