using Core.Authorization.Core;
using Core.Utility.Common;
using Jwt.Identity.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt.Identity.EntityFrameworkCore
{
    public class JwtUserStore<TUser, TRole, TUserRole, TKey, TContext> : JwtUserStoreBase<TUser, TRole, TKey>
        where TUser : JwtIdentityUser<TKey>
        where TRole : JwtIdentityRole<TKey>
        where TUserRole : JwtIdentityUserRole<TKey>
        where TKey : IEquatable<TKey>
        where TContext : DbContext
    {

        public TContext Context { get; private set; }

        /// <summary>
        /// The <see cref="IPasswordHasher{TUser}"/> used to hash passwords.
        /// </summary>
        public IPasswordHasher<TUser> PasswordHasher { get; set; }


        public JwtUserStore(TContext context, IPasswordHasher<TUser> passwordHasher)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            PasswordHasher = passwordHasher;
        }

        private DbSet<TUser> UsersSet { get { return Context.Set<TUser>(); } }

        private DbSet<TRole> RolesSet { get { return Context.Set<TRole>(); } }

        private DbSet<TUserRole> UserRoleSet { get { return Context.Set<TUserRole>(); } }

        public override IQueryable<TUser> Users => UsersSet;

        public IQueryable<TRole> Roles => RolesSet;

        public IQueryable<TUserRole> UserRole => UserRoleSet;

        public override async Task<JwtIdentityResult> LoginWithUserNamePassword(HttpContext httpContext, string name, string password,int maxErrorTimes)
        {
            var loginCertification = httpContext.Session.GetObjectFromJson<LoginCertification>("LoginCertification") ?? new LoginCertification(0);
            if (maxErrorTimes > 0 && loginCertification.ErrorTimes >= maxErrorTimes && (DateTimeHandler.CurrentTime - loginCertification.LastErrorTime).TotalMinutes <= 10)
            {
                return JwtIdentityResult.Failed(new JwtIdentityError() { Code = "", Description = "密码输入错误次数过多，请稍后重试!" });
            }
            ThrowIfDisposed();
            Guard.ArgumentNotNullOrEmpty(name, "name");
            Guard.ArgumentNotNullOrEmpty(password, "password");
            var hashPassword = PasswordHasher.HashPassword(password);

            var isValid = await Users.AnyAsync(i => i.UserName == name && i.PasswordHash == hashPassword);
            if (isValid)
            {
                var user = await Users.FirstOrDefaultAsync(i => i.UserName == name && i.PasswordHash == hashPassword);

                user.LastLoginTime = user.LastUpdatedTime = DateTimeHandler.CurrentTime;
                user.VersionNo++;
                this.Context.Set<TUser>().Attach(user);
                Context.Entry(user).Property(x => x.LastLoginTime).IsModified = true;
                Context.Entry(user).Property(x => x.VersionNo).IsModified = true;
                Context.Entry(user).Property(x => x.LastUpdatedTime).IsModified = true;
                this.Context.SaveChanges();

                try
                {
                    //var userAudit = new SysUserAudit
                    //{
                    //    AuditId = Guid.NewGuid().ToString("N"),
                    //    AuditOperateAction = 4,
                    //    AuditOperateTime = user.LastLoginTime.Value,
                    //    UserId = user.Id.ToString(),
                    //    LoginName = user.UserName,
                    //    UserName = user.NormalizedUserName,
                    //    LoginPwd = user.PasswordHash,
                    //    LastLoginTime = user.LastLoginTime.Value,
                    //    VersionNo = user.VersionNo,
                    //    ModifyTime = user.LastUpdatedTime,
                    //    Modifier = user.LastUpdatedBy,
                    //    Creator = user.CreatedBy,
                    //    CreateTime = user.CreatedTime,
                    //    TransactionId = user.TransactionId
                    //};
                    //var userAudtitEntry = this.Context.Set<SysUserAudit>().Attach(userAudit);
                    //userAudtitEntry.State = EntityState.Added;
                    //this.Context.SaveChanges();
                    loginCertification.ErrorTimes = 0;
                    loginCertification.LastErrorTime = DateTimeHandler.MinTime;
                    httpContext.Session.SetObjectAsJson("LoginCertification", loginCertification);
                }
                catch
                {

                }

                return JwtIdentityResult.Success;
            }
            loginCertification.ErrorTimes = ++loginCertification.ErrorTimes;
            loginCertification.LastErrorTime = DateTimeHandler.CurrentTime;
            httpContext.Session.SetObjectAsJson("LoginCertification", loginCertification);
            return JwtIdentityResult.Failed(new JwtIdentityError() { Code = "", Description = "用户名或密码不正确" });
        }

        public override async Task<TRole[]> GetRoleByUsername(string userName)
        {
            ThrowIfDisposed();
            Guard.ArgumentNotNullOrEmpty(userName, "userName");
            var roles = (from o in Roles
                         join userRole in UserRole on o.Id equals userRole.RoleId
                         join user in Users on userRole.UserId equals user.Id
                         where user.UserName == userName
                         select o).ToArrayAsync();

            return await roles;

        }

        public override async Task<TUser> CreateUser(TUser user)
        {
            ThrowIfDisposed();
            Guard.ArgumentNotNull(user, "userName");
            Context.Add(user);
            Context.SaveChanges();
            return await Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);
        }

    }
}
