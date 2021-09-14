using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Identity;

namespace Teng.Infrastructure.Users.dtos
{
    public class UserCreateDto : IdentityUserCreateDto
    {
        /// <summary>
        /// 介绍
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }
    }
}