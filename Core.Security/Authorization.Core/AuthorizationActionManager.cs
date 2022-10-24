using Core.Utility.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Core.Authorization.Core
{
    public class AuthorizationActionManager : IDisposable
    {
        private bool _disposed;
        private IAuthorizationActionStore _actionStore;
        //private ICache _cache;

        public static readonly string AuthorityCacheName = "AuthorityCache";
        public static readonly string UserRoleCacheName = "UserRoleCache";
        public static readonly string RoleActionCacheName = "RoleActionCache";


        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationActionManager"/> class.
        /// </summary>
        /// <param name="actionStore">The authorization action store.</param>
        /// <exception cref="ArgumentNullException">The argument <paramref name="actionStore"/> is null.</exception>
        public AuthorizationActionManager(IAuthorizationActionStore actionStore)
        {
            Guard.ArgumentNotNull(actionStore, nameof(actionStore));
            _actionStore = actionStore;
            //_cache = cacheFactory.GetCache(CacheConstants._authorizationFrameworkCacheName, new CacheOptions());
        }

        ///// <summary>
        ///// Initializes a new instance of the <see cref="AuthorizationActionManager"/> class.
        ///// </summary>
        ///// <param name="actionStore">The authorization action store.</param>
        ///// <exception cref="ArgumentNullException">The argument <paramref name="actionStore"/> is null.</exception>
        //public AuthorizationActionManager(IAuthorizationActionStore actionStore, ICacheFactory cacheFactory)
        //{
        //    Guard.ArgumentNotNull(actionStore, nameof(actionStore));
        //    _actionStore = actionStore;
        //    //_cache = cacheFactory.GetCache(CacheConstants._authorizationFrameworkCacheName, new CacheOptions());
        //}

        /// <summary>
        /// Create a new authorization action.
        /// </summary>
        /// <param name="actionName">The name of authorization action to create.</param>
        /// <param name="authorizationCode">The authorization code of authorization action to create..</param>
        /// <returns>The task to create a new authorization action.</returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="actionName"/> is null.</exception>
        /// <exception cref="ArgumentException">The argument <paramref name="actionName"/> is a white space string.</exception>
        public async Task CreateAsync(string actionName, string authorizationCode)
        {
            Guard.ArgumentNotNullOrWhiteSpace(actionName, "actionName");
            //this.ThrowIfDisposed();
            await _actionStore.CreateAsync(new AuthorizationAction(actionName, authorizationCode));
        }


        /// <summary>
        /// Deletes an existing authorization action.
        /// </summary>
        /// <param name="actionName">The name of authorization code to delete.</param>
        /// <returns>The task to delete the specified authorization action.</returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="actionName" /> is null.</exception>
        /// <exception cref="ArgumentException">The argument <paramref name="actionName" /> is a white space string.</exception>
        public async Task DeleteAsync(string authorityId)
        {
            //Guard.ArgumentNotNullOrWhiteSpace(actionName, nameof(actionName));
            //this.ThrowIfDisposed();
            await _actionStore.DeleteAsync(authorityId);
        }

        /// <summary>
        /// Gets all authorization actions.
        /// </summary>
        /// <returns>
        /// The task to get all authorization actions.
        /// </returns>
        //public async Task<IEnumerable<AuthorizationAction>> GetAllAsync(bool forceRefresh = false)
        //{
        //    this.ThrowIfDisposed();

        //    IEnumerable<AuthorizationAction> data;
        //    if (forceRefresh || !_cache.TryGet(AuthorityCacheName, out data))
        //    {
        //        data = await _actionStore.GetAllAsync();
        //        _cache.Set(AuthorityCacheName, data);
        //    }
        //    return data;
        //}

        //public async Task<IEnumerable<UserRole>> GetAllUserRolesAsync(bool forceRefresh = false)
        //{
        //    this.ThrowIfDisposed();

        //    IEnumerable<UserRole> data;
        //    if (forceRefresh || !_cache.TryGet(UserRoleCacheName, out data))
        //    {
        //        data = await _actionStore.GetAllUserRolesAsync();
        //        _cache.Set(UserRoleCacheName, data);
        //    }
        //    return data;
        //}

        //public async Task<IEnumerable<RoleAction>> GetAllRoleActionAsync(bool forceRefresh = false)
        //{
        //    this.ThrowIfDisposed();

        //    IEnumerable<RoleAction> data;
        //    if (forceRefresh || !_cache.TryGet(RoleActionCacheName, out data))
        //    {
        //        data = await _actionStore.GetAllRoleActionAsync();
        //        _cache.Set(RoleActionCacheName, data);
        //    }
        //    return data;
        //}

        /// <summary>
        /// Finds the authorization action based on specified action name.
        /// </summary>
        /// <param name="actionName">The name of authorization action to find.</param>
        /// <returns>The task to find the authorization action based on specified action name.</returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="actionName" /> is null.</exception>
        /// <exception cref="ArgumentException">The argument <paramref name="actionName" /> is a white space string.</exception>
        public async Task<AuthorizationAction> FindAsync(string authorityId)
        {
            //Guard.ArgumentNotNullOrWhiteSpace(actionName, nameof(actionName));
            //this.ThrowIfDisposed();
            return await _actionStore.FindAsync(authorityId);
        }

        /// <summary>
        /// Adds authorization actions to specified role.
        /// </summary>
        /// <param name="actionNames">The names of authorization actions to add.</param>
        /// <param name="roleName">The name of role to which the authorization actions is added.</param>
        /// <returns>The task to add authorization actions to specified role.</returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="actionNames" /> is null.</exception>
        /// <exception cref="ArgumentException">The argument <paramref name="actionNames" /> is an empty array.</exception>
        /// <exception cref="ArgumentNullException">The argument <paramref name="roleName" /> is null.</exception>
        /// <exception cref="ArgumentException">The argument <paramref name="roleName" /> is a white space string.</exception>
        public async Task AddActionsToRoleAsync(string roleId, params string[] authorityIds)
        {
            Guard.ArgumentNotNullOrEmpty(authorityIds, "authorityIds");
            Guard.ArgumentNotNullOrWhiteSpace(roleId, "roleId");
            //this.ThrowIfDisposed();

            await _actionStore.AddActionsToRoleAsync(roleId, authorityIds);
        }

        /// <summary>
        /// Removes authorization actions from specified role.
        /// </summary>
        /// <param name="actionNames">The names of authorization actions to remove.</param>
        /// <param name="roleName">The name of role from which the authorization actions is removed.</param>
        /// <returns>The task to remove authorization actions from specified role.</returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="actionNames" /> is null.</exception>
        /// <exception cref="ArgumentException">The argument <paramref name="actionNames" /> is an empty array.</exception>
        /// <exception cref="ArgumentNullException">The argument <paramref name="roleName" /> is null.</exception>
        /// <exception cref="ArgumentException">The argument <paramref name="roleName" /> is a white space string.</exception>
        public async Task RemoveActionsFromRoleAsync(string roleId, params string[] authorityIds)
        {
            Guard.ArgumentNotNullOrEmpty(authorityIds, "authorityIds");
            Guard.ArgumentNotNullOrWhiteSpace(roleId, "roleId");
            //this.ThrowIfDisposed();

            await _actionStore.RemoveActionsFromRoleAsync(roleId, authorityIds);
        }

        /// <summary>
        /// Determines whether the specfied authorization action in specified role.
        /// </summary>
        /// <param name="actionName">The name of the authorization action.</param>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>The task to dtermine whether the specfied authorization action in specified role.</returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="actionName" /> is null.</exception>
        /// <exception cref="ArgumentException">The argument <paramref name="actionName" /> is a white space string.</exception>
        /// <exception cref="ArgumentNullException">The argument <paramref name="roleName" /> is null.</exception>
        /// <exception cref="ArgumentException">The argument <paramref name="roleName" /> is a white space string.</exception>
        public async Task<bool> IsActionInRoleAsync(string authorityId, string roleId)
        {
            Guard.ArgumentNotNullOrWhiteSpace(roleId, "roleId");
            //this.ThrowIfDisposed();

            return await _actionStore.IsActionInRoleAsync(authorityId, roleId);
        }

        /// <summary>
        /// Gets all authorization actions added in specified roles.
        /// </summary>
        /// <param name="roleNames">The role names.</param>
        /// <returns>The task to get all authorization actions added in specified role.</returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="roleNames" /> is null.</exception>
        /// <exception cref="ArgumentException">The argument <paramref name="roleNames" /> is an empty array.</exception>
        public async Task<IEnumerable<AuthorizationAction>> GetActionsInRolesAsync(params string[] roleIds)
        {
            Guard.ArgumentNotNullOrEmpty(roleIds, "roleIds");
            //this.ThrowIfDisposed();

            return await _actionStore.GetActionsInRoleAsync(roleIds);
        }

        /// <summary>
        /// Gets all authorization actions by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        //public async Task<IEnumerable<AuthorizationAction>> GetAllActionsByUserId(string userId, bool forceRefresh = false)
        //{
        //    this.ThrowIfDisposed();

        //    var authorities = await GetAllAsync(forceRefresh);
        //    var userRoles = await GetAllUserRolesAsync(forceRefresh);
        //    var roleActions = await GetAllRoleActionAsync(forceRefresh);

        //    var query = from action in authorities
        //                join roleAction in roleActions on action.AuthorityId equals roleAction.AuthorityId
        //                join userRole in userRoles on roleAction.RoleId equals userRole.RoleId
        //                where userRole.UserId == userId
        //                select action;
        //    return query;

        //}

        /// <summary>
        /// Gets all role added by specified authorization action.
        /// </summary>
        /// <param name="actionNames">The authorization action names.</param>
        /// <returns>The task to get all roles by specified authorization action.</returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="actionNames"/> is null.</exception>
        /// <exception cref="ArgumentException">The argument <paramref name="actionNames"/> is an empty array.</exception>
        public async Task<IEnumerable<string>> GetRolesByActionAsync(string[] authorityIds)
        {
            Guard.ArgumentNotNullOrEmpty(authorityIds, "authorityIds");
            //this.ThrowIfDisposed();

            return await _actionStore.GetRolesByActionAsync(authorityIds);
        }

        /// <summary>
        /// Returns a value that indicates whether user is accessible by the specified authorization action.(From Claim)
        /// </summary>
        /// <param name="actionName">The authorization action for which to check.</param>
        /// <param name="principal">The <see cref="ClaimsPrincipal" /> to represent user.</param>
        /// <returns>true if user is accessible by the specified authorization action; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">actionName or principal is null.</exception>
        /// <exception cref="ArgumentException">actionName is white space.</exception>
        //public async Task<bool> IsAccessibleForUser(string authorityId, ClaimsPrincipal principal)
        //{
        //    Guard.ArgumentNotNull(principal, "principal");
        //    this.ThrowIfDisposed();

        //    //var action = await _actionStore.FindAsync(authorityId);
        //    //if (action == null)
        //    //{
        //    //    return false;
        //    //}

        //    //return principal.IsAccessible(action.AuthorityId);
        //    return principal.IsAccessible(authorityId);
        //}

        /// <summary>
        /// Returns a value that indicates whether user is accessible by the specified authorization action.(from DB)
        /// </summary>
        /// <param name="actionName">The authorization action for which to check.</param>
        /// <param name="principal">The <see cref="ClaimsPrincipal" /> to represent user.</param>
        /// <returns>true if user is accessible by the specified authorization action; otherwise, false.</returns>
        /// <exception cref="ArgumentNullException">actionName or principal is null.</exception>
        /// <exception cref="ArgumentException">actionName is white space.</exception>
        //public async Task<bool> IsAccessibleForUser(string authorityId, ClaimsPrincipal principal)
        //{
        //    Guard.ArgumentNotNull(principal, "principal");
        //    this.ThrowIfDisposed();

        //    var userId = principal.Claims.FirstOrDefault(x => x.Type == MyClaimTypes.UserId)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //        return false;
        //    var authorities = await GetAllActionsByUserId(userId);
        //    if (authorities.Any(x => x.AuthorityId == authorityId))
        //        return true;
        //    return false;
        //}

        /// <summary>
        /// Releases all resources used by the authorization action manager.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the authorization action manager and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !this._disposed)
            {
                _actionStore?.Dispose();
                _actionStore = null;
            }
            _disposed = true;
        }

        /// <summary>
        /// Throws <see cref="ObjectDisposedException"/> if the current manager has been disposed.
        /// </summary>
        //protected void ThrowIfDisposed()
        //{
        //    if (this._disposed)
        //    {
        //        throw new ObjectDisposedException(Resources.ExcetpionObjectIsDisposed.Fill(typeof(AuthorizationActionManager).Name));
        //    }
        //}
    }
}
