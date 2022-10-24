using Core.Utility.Common;
using System;

namespace Jwt.Identity.Core
{
    public class JwtIdentityUser : JwtIdentityUser<string>
    {
        public JwtIdentityUser()
        {
            Id = StringHandler.AutoId();
        }

        public JwtIdentityUser(string userName) : this()
        {
            UserName = userName;
        }
     
    }

    public class JwtIdentityUser<TKey> where TKey : IEquatable<TKey>
    {
        public JwtIdentityUser() { }

        public JwtIdentityUser(string userName) : this()
        {
            UserName = userName;
        }

        public virtual TKey Id { get; set; }

        public virtual string UserName { get; set; }

        public virtual string PasswordHash { get; set; }

        public virtual string NormalizedUserName { get; set; }

        public virtual DateTime? LastLoginTime { get; set; }

        

        /// <summary>
        /// 
        /// </summary>
        public string TransactionId { get; set; }

        /// <summary>
        /// 版本号
        /// </summary>
        public int VersionNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string LastUpdatedBy { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastUpdatedTime { get; set; }

        public override string ToString()
          => UserName;

    }
}
