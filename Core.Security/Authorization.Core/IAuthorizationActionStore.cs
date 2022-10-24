using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Authorization.Core
{
    public interface IAuthorizationActionStore : IDisposable
    {
        /// <summary>
        /// Creates a new authorization action.
        /// </summary>
        /// <param name="action">The authorization action to create.</param>
        /// <returns>The task to create a new authorization action.</returns>
        Task CreateAsync(AuthorizationAction action);

        /// <summary>
        /// Deletes an existing authorization action.
        /// </summary>
        /// <param name="actionName">The name of authorization action to delete.</param>
        /// <returns>The task to delete the specified athorization action.</returns>
        Task DeleteAsync(string authorityId);

        /// <summary>
        /// Find the authorization action based on specified action name.
        /// </summary>
        /// <param name="actionName">The name of authorization action to find.</param>
        /// <returns>The task to find the specified authorization action.</returns>
        Task<AuthorizationAction> FindAsync(string authorityId);

        /// <summary>
        /// Gets all authorization actions.
        /// </summary>
        /// <returns>The task to get all authorization actions.</returns>
        Task<IEnumerable<AuthorizationAction>> GetAllAsync();

        Task<IEnumerable<UserRole>> GetAllUserRolesAsync();

        Task<IEnumerable<RoleAction>> GetAllRoleActionAsync();

        /// <summary>
        /// Adds authorization actions to specified role.
        /// </summary>
        /// <param name="actionNames">The names of authorization actions to add.</param>
        /// <param name="roleName">The name of role to which the authorization actions is added.</param>
        /// <returns>The task to add authorization actions to specified role.</returns>
        Task AddActionsToRoleAsync(string roleId, string[] authorityIds);

        /// <summary>
        /// Removes authorization actions from specified role.
        /// </summary>
        /// <param name="actionNames">The names of authorization actions to remove.</param>
        /// <param name="roleName">The name of role from which the authorization actions is removed.</param>
        /// <returns>The task to remove authorization actions from specified role.</returns>
        Task RemoveActionsFromRoleAsync(string roleId, string[] authorityIds);


        /// <summary>
        /// Determines whether the specfied authorization action in specified role.
        /// </summary>
        /// <param name="actionName">The name of the authorization action.</param>
        /// <param name="roleName">The name of the role.</param>
        /// <returns>The task to dtermine whether the specfied authorization action in specified role.</returns>
        Task<bool> IsActionInRoleAsync(string authorityId, string roleId);


        /// <summary>
        /// Gets all authorization actions added in specified roles.
        /// </summary>
        /// <param name="roleNames">The role names.</param>
        /// <returns>The task to get all authorization actions added in specified role.</returns>
        Task<IEnumerable<AuthorizationAction>> GetActionsInRoleAsync(string[] roleIds);

        /// <summary>
        /// Gets all role added by specified authorization action.
        /// </summary>
        /// <param name="actionNames">The authorization action names.</param>
        /// <returns>The task to get all roles by specified authorization action.</returns>
        Task<IEnumerable<string>> GetRolesByActionAsync(string[] authorityIds);

        /// <summary>
        /// Gets all authorization actions by user id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<AuthorizationAction>> GetAllActionsByUserId(string userId);
    }
}
