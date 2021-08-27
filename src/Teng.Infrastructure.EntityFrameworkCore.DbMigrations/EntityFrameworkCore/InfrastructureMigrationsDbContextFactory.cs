using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Teng.Infrastructure.EntityFrameworkCore
{
    /* This class is needed for EF Core console commands
     * (like Add-Migration and Update-Database commands) */
    public class InfrastructureMigrationsDbContextFactory : IDesignTimeDbContextFactory<InfrastructureMigrationsDbContext>
    {
        public InfrastructureMigrationsDbContext CreateDbContext(string[] args)
        {
            InfrastructureEfCoreEntityExtensionMappings.Configure();

            var configuration = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<InfrastructureMigrationsDbContext>()
                .UseMySql(configuration.GetConnectionString("Default"));

            return new InfrastructureMigrationsDbContext(builder.Options);
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
