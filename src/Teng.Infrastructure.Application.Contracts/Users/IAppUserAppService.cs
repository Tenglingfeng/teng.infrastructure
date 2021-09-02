using System.Threading.Tasks;
using Teng.Infrastructure.Users.dtos;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Teng.Infrastructure.Users
{
    public interface IAppUserAppService : IApplicationService
    {
        Task<AppUserDto> Login(string userName, string passWord);
    }
}