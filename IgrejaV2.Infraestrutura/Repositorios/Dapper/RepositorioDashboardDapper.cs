using IgrejaV2.Dominio.Interfaces;

namespace IgrejaV2.Infraestrutura.Repositorios.Dapper
{
    public class RepositorioDashboardDapper : IRepositorioDashboard
    {
        private readonly string _connectionString;
        public RepositorioDashboardDapper(string connectionString)
        {
            _connectionString = connectionString;
        }
    }
}
