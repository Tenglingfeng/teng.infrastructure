using System.Threading.Tasks;
using Teng.Infrastructure.Users.dtos;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Teng.Infrastructure.Users
{
    public interface IAppUserAppService : IApplicationService
    {
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<LoginResultDto> Login(LoginInputDto input);

        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateAsync(UserCreateDto input);

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<AppUserDto>> GetListAsync(GetUserPagedAndSortedResultInput input);
    }
}