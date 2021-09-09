using AutoMapper;
using Teng.Infrastructure.Users;
using Teng.Infrastructure.Users.dtos;
using Volo.Abp.Identity;

namespace Teng.Infrastructure
{
    public class InfrastructureApplicationAutoMapperProfile : Profile
    {
        public InfrastructureApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */

            CreateMap<IdentityUser, AppUserDto>();
        }
    }
}