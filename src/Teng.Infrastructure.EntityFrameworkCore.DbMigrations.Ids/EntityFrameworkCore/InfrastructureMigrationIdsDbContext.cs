using Microsoft.EntityFrameworkCore;
using Teng.Infrastructure.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.IdentityServer;
using Volo.Abp.IdentityServer.EntityFrameworkCore;

namespace Teng.Infrastructure.Ids.EntityFrameworkCore
{
    [ConnectionStringName(AbpIdentityServerDbProperties.ConnectionStringName)]
    public class InfrastructureMigrationIdsDbContext : AbpDbContext<InfrastructureMigrationIdsDbContext>
    {
        public InfrastructureMigrationIdsDbContext(DbContextOptions<InfrastructureMigrationIdsDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureIdentityServer();
            modelBuilder.ConfigureInfrastructure();
        }
    }
}