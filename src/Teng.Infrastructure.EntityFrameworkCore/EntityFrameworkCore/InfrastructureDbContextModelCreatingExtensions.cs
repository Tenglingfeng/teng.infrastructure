using Microsoft.EntityFrameworkCore;
using Volo.Abp;

namespace Teng.Infrastructure.EntityFrameworkCore
{
    public static class InfrastructureDbContextModelCreatingExtensions
    {
        public static void ConfigureInfrastructure(this ModelBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            /* Configure your own tables/entities inside here */

            //builder.Entity<YourEntity>(b =>
            //{
            //    b.ToTable(InfrastructureConsts.DbTablePrefix + "YourEntities", InfrastructureConsts.DbSchema);
            //    b.ConfigureByConvention(); //auto configure for the base class props
            //    //...
            //});
        }
    }
}