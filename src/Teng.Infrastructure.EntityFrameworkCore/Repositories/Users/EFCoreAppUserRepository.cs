using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teng.Infrastructure.EntityFrameworkCore;
using Teng.Infrastructure.Users;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Teng.Infrastructure.Repositories.Users
{
    public class EfCoreAppUserRepository : EfCoreRepository<InfrastructureDbContext, AppUser, Guid>, IAppUserRepository
    {
        private readonly IIdentityDbContext _identityDbContext;

        public EfCoreAppUserRepository(IDbContextProvider<InfrastructureDbContext> dbContextProvider, IIdentityDbContext identityDbContext) : base(dbContextProvider)
        {
            _identityDbContext = identityDbContext;
        }

        /// <summary>
        /// 根据Id获取角色名称列表 及机构角色名称列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<string>> GetRoleNamesAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = from userRole in _identityDbContext.Set<IdentityUserRole>()
                        join role in _identityDbContext.Roles on userRole.RoleId equals role.Id
                        where userRole.UserId == id
                        select role.Name;
            var organizationUnitIds = DbContext.Set<IdentityUserOrganizationUnit>().Where(q => q.UserId == id).Select(q => q.OrganizationUnitId).ToArray();

            var organizationRoleIds = await (
                from ouRole in DbContext.Set<OrganizationUnitRole>()
                join ou in DbContext.Set<OrganizationUnit>() on ouRole.OrganizationUnitId equals ou.Id
                where organizationUnitIds.Contains(ouRole.OrganizationUnitId)
                select ouRole.RoleId
            ).ToListAsync(GetCancellationToken(cancellationToken));

            var orgUnitRoleNameQuery = _identityDbContext.Roles.Where(r => organizationRoleIds.Contains(r.Id)).Select(n => n.Name);
            var resultQuery = query.Union(orgUnitRoleNameQuery);
            return await resultQuery.ToListAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 根据机构Id获取角色名称列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<string>> GetRoleNamesInOrganizationUnitAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            var query = from userOu in DbContext.Set<IdentityUserOrganizationUnit>()
                        join roleOu in DbContext.Set<OrganizationUnitRole>() on userOu.OrganizationUnitId equals roleOu.OrganizationUnitId
                        join ou in DbContext.Set<OrganizationUnit>() on roleOu.OrganizationUnitId equals ou.Id
                        join userOuRoles in _identityDbContext.Roles on roleOu.RoleId equals userOuRoles.Id
                        where userOu.UserId == id
                        select userOuRoles.Name;

            var result = await query.ToListAsync(GetCancellationToken(cancellationToken));

            return result;
        }

        /// <summary>
        /// 根据 loginProvider ProviderKey 获取用户信息
        /// </summary>
        /// <param name="loginProvider"></param>
        /// <param name="providerKey"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<AppUser> FindByLoginAsync(
            string loginProvider,
            string providerKey,
            bool includeDetails = true,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                //.Where(u => u.Logins.Any(login => login.LoginProvider == loginProvider && login.ProviderKey == providerKey))
                .FirstOrDefaultAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 根据 Claim 获取用户信息
        /// </summary>
        /// <param name="claim"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<AppUser>> GetListByClaimAsync(
            Claim claim,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .Where(u => u.Claims.Any(c => c.ClaimType == claim.Type && c.ClaimValue == claim.Value))
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

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
        public virtual async Task<List<AppUser>> GetListAsync(
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            string filter = null,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            return await DbSet
                .IncludeDetails(includeDetails)
                .WhereIf(
                    !filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        (u.Name != null && u.Name.Contains(filter)) ||
                        (u.Surname != null && u.Surname.Contains(filter)) ||
                        (u.PhoneNumber != null && u.PhoneNumber.Contains(filter)) ||
                        (u.Introduction != null && u.Introduction.Contains(filter))
                )
                .OrderBy(sorting ?? nameof(AppUser.LastModificationTime))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 根据Id获取角色列表以及机构的角色列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<IdentityRole>> GetRolesAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from userRole in DbContext.Set<IdentityUserRole>()
                        join role in _identityDbContext.Roles.IncludeDetails(includeDetails) on userRole.RoleId equals role.Id
                        where userRole.UserId == id
                        select role;

            //TODO: Needs improvement
            var userOrganizationsQuery = from userOrg in DbContext.Set<IdentityUserOrganizationUnit>()
                                         join ou in _identityDbContext.OrganizationUnits.IncludeDetails(includeDetails) on userOrg.OrganizationUnitId equals ou.Id
                                         where userOrg.UserId == id
                                         select ou;

            var orgUserRoleQuery = DbContext.Set<OrganizationUnitRole>()
                .Where(q => userOrganizationsQuery
                .Select(t => t.Id)
                .Contains(q.OrganizationUnitId))
                .Select(t => t.RoleId);

            var orgRoles = _identityDbContext.Roles.Where(q => orgUserRoleQuery.Contains(q.Id));
            var resultQuery = query.Union(orgRoles);
            return await resultQuery.ToListAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 根据条件获取数量
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<long> GetCountAsync(
            string filter = null,
            CancellationToken cancellationToken = default)
        {
            return await this.WhereIf(!filter.IsNullOrWhiteSpace(),
                    u =>
                        u.UserName.Contains(filter) ||
                        u.Email.Contains(filter) ||
                        (u.Name != null && u.Name.Contains(filter)) ||
                        (u.Surname != null && u.Surname.Contains(filter)) ||
                        (u.PhoneNumber != null && u.PhoneNumber.Contains(filter))
                ).LongCountAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 根据Id获取机构列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeDetails"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<OrganizationUnit>> GetOrganizationUnitsAsync(
            Guid id,
            bool includeDetails = false,
            CancellationToken cancellationToken = default)
        {
            var query = from userOu in DbContext.Set<IdentityUserOrganizationUnit>()
                        join ou in _identityDbContext.OrganizationUnits.IncludeDetails(includeDetails) on userOu.OrganizationUnitId equals ou.Id
                        where userOu.UserId == id
                        select ou;

            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 根据机构Id获取用户列表
        /// </summary>
        /// <param name="organizationUnitId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<AppUser>> GetUsersInOrganizationUnitAsync(
            Guid organizationUnitId,
            CancellationToken cancellationToken = default
            )
        {
            var query = from userOu in DbContext.Set<IdentityUserOrganizationUnit>()
                        join user in DbSet on userOu.UserId equals user.Id
                        where userOu.OrganizationUnitId == organizationUnitId
                        select user;
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 根据机构列表获取用户列表
        /// </summary>
        /// <param name="organizationUnitIds"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<List<AppUser>> GetUsersInOrganizationsListAsync(
            List<Guid> organizationUnitIds,
            CancellationToken cancellationToken = default
            )
        {
            var query = from userOu in DbContext.Set<IdentityUserOrganizationUnit>()
                        join user in DbSet on userOu.UserId equals user.Id
                        where organizationUnitIds.Contains(userOu.OrganizationUnitId)
                        select user;
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 根据机构编码获取当前机构及下级机构的用户列表
        /// </summary>
        /// <param name="code"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public virtual async Task<List<AppUser>> GetUsersInOrganizationUnitWithChildrenAsync(
            string code,
            CancellationToken cancellationToken = default
            )
        {
            var query = from userOu in DbContext.Set<IdentityUserOrganizationUnit>()
                        join user in DbSet on userOu.UserId equals user.Id
                        join ou in DbContext.Set<OrganizationUnit>() on userOu.OrganizationUnitId equals ou.Id
                        where ou.Code.StartsWith(code)
                        select user;
            return await query.ToListAsync(GetCancellationToken(cancellationToken));
        }

        /// <summary>
        /// 明细查询
        /// </summary>
        /// <returns></returns>
        public override IQueryable<AppUser> WithDetails()
        {
            return GetQueryable().IncludeDetails();
        }
    }
}