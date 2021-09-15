using System.Net.Http.Headers;
using AutoMapper;
using Teng.Infrastructure.Users.dtos;

namespace Teng.Infrastructure.Users
{
    public class AppUserAutoMapperProfile : Profile
    {
        public AppUserAutoMapperProfile()
        {
            CreateMap<AppUser, AppUserDto>();
        }
    }
}