using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Identity;

namespace Teng.Infrastructure.Users
{
    public interface IAppUserRepository : IBasicRepository<AppUser, Guid>
    {
        /// <summary>
        /// 根据Id获取角色名称列表 及机构角色名称列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<string>> GetRoleNamesAsync(
            Guid id,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 根据机构Id获取角色名称列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<string>> GetRoleNamesInOrganizationUnitAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据 loginProvider ProviderKey 获取用户信息
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<AppUser> FindByLoginAsync(
            [NotNull] string loginProvider,
            [NotNull] string providerKey,
            bool includeDetails = true,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 根据 Claim 获取用户信息
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<AppUser>> GetListByClaimAsync(
            Claim claim,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 根据条件分页获取用户列表
        /// </summary>
        /// <param name="sorting"></param>
        /// <param name="maxResultCount"></param>
        /// <param name="skipCount"></param>
        /// <param name="filter"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<AppUser>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 根据Id获取角色列表以及机构的角色列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<IdentityRole>> GetRolesAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default
        );

        /// <summary>
        /// 根据Id获取机构列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// 根据机构Id获取用户列表
        /// </summary>
        /// <param name="organizationUnitId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<AppUser>> GetUsersInOrganizationUnitAsync(
            Guid organizationUnitId,
            CancellationToken cancellationToken = default
            );

        /// <summary>
        /// 根据机构列表获取用户列表
        /// </summary>
        /// <param name="organizationUnitIds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<AppUser>> GetUsersInOrganizationsListAsync(
            List<Guid> organizationUnitIds,
            CancellationToken cancellationToken = default
            );

        /// <summary>
        /// 根据机构编码获取当前机构及下级机构的用户列表
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<AppUser>> GetUsersInOrganizationUnitWithChildrenAsync(
            string code,
            CancellationToken cancellationToken = default
            );

        /// <summary>
        /// 根据条件获取数量
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<long> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default
        );
    }
}