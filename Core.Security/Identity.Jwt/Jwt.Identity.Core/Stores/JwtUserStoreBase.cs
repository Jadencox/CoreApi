using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Identity.Core
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="Tkey"></typeparam>
    public abstract class JwtUserStoreBase<TUser, TRole, Tkey>
        where Tkey : IEquatable<Tkey>
        where TRole : JwtIdentityRole<Tkey>
        where TUser : JwtIdentityUser<Tkey>

    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="name"></param>
        /// <param name="password"></param>
        /// <param name="maxErrorTimes"></param>
        /// <returns></returns>
        public virtual async Task<JwtIdentityResult> LoginWithUserNamePassword(HttpContext httpContext, string name, string password, int maxErrorTimes)
        {
            ThrowIfDisposed();
            if (name == null)
                throw new ArgumentNullException(nameof(name));
            return await Task.FromResult(JwtIdentityResult.Success);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public virtual async Task<TRole[]> GetRoleByUsername(string userName)
        {
            ThrowIfDisposed();
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));
            return await Task.FromResult(new JwtIdentityRole<Tkey>() as TRole[]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task<TUser> CreateUser(TUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return await Task.FromResult(new JwtIdentityUser<Tkey>() as TUser);
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract IQueryable<TUser> Users
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Throws if this class has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        /// <summary>
        /// Dispose the store
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }
}
