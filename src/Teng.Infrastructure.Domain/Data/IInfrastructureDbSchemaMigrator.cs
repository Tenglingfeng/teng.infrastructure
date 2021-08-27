using System.Threading.Tasks;

namespace Teng.Infrastructure.Data
{
    public interface IInfrastructureDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
