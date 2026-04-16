using IgrejaV2.Dominio.Interfaces;
using IgrejaV2.Infraestrutura.Contexto;

namespace IgrejaV2.Infraestrutura.Repositorios
{
    public class RepositorioDashboard : IRepositorioDashboard
    {
        private readonly IgrejaContexto _contexto;
        public RepositorioDashboard(IgrejaContexto contexto)
        {
            _contexto = contexto;
        }
    }
}
