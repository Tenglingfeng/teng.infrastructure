using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teng.Infrastructure.Localization;
using Teng.Infrastructure.Users;
using Teng.Infrastructure.Users.dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;

namespace Teng.Infrastructure.Controllers
{
    /* Inherit your controllers from this class.
     */

    [ControllerName("AppUser")]
    [Area("AppUser")]
    [Route("api/app-user")]
    public class AppUserController : InfrastructureController
    {
        private readonly IAppUserAppService _appUserAppService;

        public AppUserController(IAppUserAppService appUserAppService)
        {
            _appUserAppService = appUserAppService;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<SuccessResult<LoginResultDto>> LoginAsync([FromBody] LoginInputDto input)
        {
            var result = await _appUserAppService.Login(input);
            return Ok(result);
        }

        /// <summary>
        /// 新增用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost()]
        [Authorize(IdentityPermissions.Users.Create)]
        public async Task CreateAsync([FromBody] UserCreateDto input)
        {
            await _appUserAppService.CreateAsync(input);
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet()]
        [Authorize()]
        public async Task<SuccessResult<PagedResultDto<AppUserDto>>> GetListAsync([FromQuery] GetUserPagedAndSortedResultInput input)
        {
            return Ok(await _appUserAppService.GetListAsync(input));
        }
    }
}