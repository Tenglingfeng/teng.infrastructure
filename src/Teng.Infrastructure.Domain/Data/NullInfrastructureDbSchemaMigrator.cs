using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Teng.Infrastructure.Data
{
    /* This is used if database provider does't define
     * IInfrastructureDbSchemaMigrator implementation.
     */
    public class NullInfrastructureDbSchemaMigrator : IInfrastructureDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}