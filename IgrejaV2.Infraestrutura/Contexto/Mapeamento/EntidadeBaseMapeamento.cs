using IgrejaV2.Dominio.Entidades;
using Microsoft.EntityFrameworkCore;

namespace IgrejaV2.Infraestrutura.Contexto.Mapeamento
{
    public static class EntidadeBaseMapeamento
    {
        public static void ApplyBaseEntityConfiguration(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Verifica se a entidade herda de EntidadeBase
                if (typeof(EntidadeBase).IsAssignableFrom(entityType.ClrType))
                {
                    var builder = modelBuilder.Entity(entityType.ClrType);

                    // Mapeamento de nomes de coluna (Snake Case manual)
                    builder.Property(nameof(EntidadeBase.Id)).HasColumnName("id");
                    builder.Property(nameof(EntidadeBase.DataCriacao)).HasColumnName("data_criacao");
                    builder.Property(nameof(EntidadeBase.DataAtualizacao)).HasColumnName("data_atualizacao");
                    builder.Property(nameof(EntidadeBase.CriadoPorId)).HasColumnName("criado_por_id");
                    builder.Property(nameof(EntidadeBase.AtualizadoPorId)).HasColumnName("atualizado_por_id");
                    builder.Property(nameof(EntidadeBase.Deletado)).HasColumnName("deletado");
                    builder.Property(nameof(EntidadeBase.DataDelecao)).HasColumnName("data_delecao");
                    builder.Property(nameof(EntidadeBase.DeletadoPorId)).HasColumnName("deletado_por_id");

                    // Configuração do Soft Delete Global
                    var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                    var propertyAccess = System.Linq.Expressions.Expression.Property(parameter, nameof(EntidadeBase.Deletado));
                    var equal = System.Linq.Expressions.Expression.Equal(propertyAccess, System.Linq.Expressions.Expression.Constant(false));
                    var lambda = System.Linq.Expressions.Expression.Lambda(equal, parameter);

                    builder.HasQueryFilter(lambda);
                }
            }
        }
    }
}
