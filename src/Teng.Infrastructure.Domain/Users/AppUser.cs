﻿using System;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Users;

namespace Teng.Infrastructure.Users
{
    /* This entity shares the same table/collection ("AbpUsers" by default) with the
     * IdentityUser entity of the Identity module.
     *
     * - You can define your custom properties into this class.
     * - You never create or delete this entity, because it is Identity module's job.
     * - You can query users from database with this entity.
     * - You can update values of your custom properties.
     */

    public class AppUser : FullAuditedAggregateRoot<Guid>, IUser
    {
        #region Base properties

        /* These properties are shared with the IdentityUser entity of the Identity module.
         * Do not change these properties through this class. Instead, use Identity module
         * services (like IdentityUserManager) to change them.
         * So, this properties are designed as read only!
         */

        public virtual Guid? TenantId { get; private set; }

        public virtual string UserName { get; private set; }

        public virtual string Name { get; private set; }

        public virtual string Surname { get; private set; }

        public virtual string Email { get; private set; }

        public virtual bool EmailConfirmed { get; private set; }

        public virtual string PhoneNumber { get; private set; }

        public virtual bool PhoneNumberConfirmed { get; private set; }

        #endregion Base properties

        /* Add your own properties here. Example:
         *
         * public string MyProperty { get; set; }
         *
         * If you add a property and using the EF Core, remember these;
         *
         * 1. Update InfrastructureDbContext.OnModelCreating
         * to configure the mapping for your new property
         * 2. Update InfrastructureEfCoreEntityExtensionMappings to extend the IdentityUser entity
         * and add your new property to the migration.
         * 3. Use the Add-Migration to add a new database migration.
         * 4. Run the .DbMigrator project (or use the Update-Database command) to apply
         * schema change to the database.
         */

        public string HeadPortrait { get; private set; }

        public string Avatar { get; set; }

        public string Introduction { get; set; }

        /// <summary>
        /// Navigation property for the roles this user belongs to.
        /// </summary>
        public virtual ICollection<IdentityUserRole> Roles { get; protected set; }

        /// <summary>
        /// Navigation property for the claims this user possesses.
        /// </summary>
        public virtual ICollection<IdentityUserClaim> Claims { get; protected set; }

        ///// <summary>
        ///// Navigation property for this users login accounts.
        ///// </summary>
        //public virtual ICollection<IdentityUserLogin> Logins { get; protected set; }

        /// <summary>
        /// Navigation property for this users tokens.
        /// </summary>
        public virtual ICollection<IdentityUserToken> Tokens { get; protected set; }

        /// <summary>
        /// Navigation property for this organization units.
        /// </summary>
        public virtual ICollection<IdentityUserOrganizationUnit> OrganizationUnits { get; protected set; }

        private AppUser()
        {
        }

        public AppUser(
            Guid id,
            Guid? tenantId,
            string userName,
            string name,
            string surname,
            string email,
            bool emailConfirmed,
            string phoneNumber,
            bool phoneNumberConfirmed,
            string headPortrait) : base(id)
        {
            TenantId = tenantId;
            UserName = userName;
            Name = name;
            Surname = surname;
            Email = email;
            EmailConfirmed = emailConfirmed;
            PhoneNumber = phoneNumber;
            PhoneNumberConfirmed = phoneNumberConfirmed;
            HeadPortrait = headPortrait;
        }

        public void SetHeadPortrait(string headPortrait)
        {
            HeadPortrait = headPortrait;
        }
    }
}