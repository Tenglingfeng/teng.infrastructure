using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Teng.Infrastructure.EntityFrameworkCore;
using Volo.Abp.IdentityServer;

namespace Teng.Infrastructure.Ids.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */

    public class InfrastructureMigrationsIdsDbContextFactory : IDesignTimeDbContextFactory<InfrastructureMigrationIdsDbContext>
    {
        public InfrastructureMigrationIdsDbContext CreateDbContext(string[] args)
        {
            InfrastructureEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<InfrastructureMigrationIdsDbContext>()
                .UseMySql(configuration.GetConnectionString(AbpIdentityServerDbProperties.ConnectionStringName));

            return new InfrastructureMigrationIdsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Teng.Infrastructure.DbMigrator/"))
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}