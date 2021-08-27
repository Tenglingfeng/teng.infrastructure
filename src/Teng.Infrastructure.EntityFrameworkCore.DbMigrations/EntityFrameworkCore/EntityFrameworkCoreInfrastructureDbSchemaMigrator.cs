using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Teng.Infrastructure.Data;
using Volo.Abp.DependencyInjection;

namespace Teng.Infrastructure.EntityFrameworkCore
{
    public class EntityFrameworkCoreInfrastructureDbSchemaMigrator
        : IInfrastructureDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreInfrastructureDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the InfrastructureMigrationsDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<InfrastructureMigrationsDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}