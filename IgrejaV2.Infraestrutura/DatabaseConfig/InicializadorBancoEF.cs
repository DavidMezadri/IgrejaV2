using IgrejaV2.Infraestrutura.Contexto;

namespace IgrejaV2.Infraestrutura.DatabaseConfig
{
    public class InicializadorBancoEF : IInicializadorBanco
    {
        private readonly IgrejaContexto _contexto;

        public InicializadorBancoEF(IgrejaContexto contexto)
        {
            _contexto = contexto;
        }

        public async Task InicializarAsync()
        {
            await _contexto.Database.MigrateAsync();
        }
    }
}
