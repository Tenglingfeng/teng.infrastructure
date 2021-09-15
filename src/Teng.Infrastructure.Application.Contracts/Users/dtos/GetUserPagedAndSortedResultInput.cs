using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Teng.Infrastructure.Users.dtos
{
    public class GetUserPagedAndSortedResultInput : PagedAndSortedResultRequestDto
    {
        /// <summary>
        /// 过滤条件: 用户名/名称/手机号
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public Guid? RoleId { get; set; }

        /// <summary>
        /// 机构
        /// </summary>
        public Guid? OrganizationId { get; set; }
    }
}