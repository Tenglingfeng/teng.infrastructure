﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Teng.Infrastructure.Localization;
using Teng.Infrastructure.Users;
using Teng.Infrastructure.Users.dtos;
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

        [HttpGet("{userName}")]
        public async Task<ActionResult<AppUserDto>> GetAsync(string userName)
        {
            return Ok(await _appUserAppService.Login(userName, ""));
        }
    }
}