using Core.Utility.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jwt.Identity.Core
{
    public class JwtIdentityRole : JwtIdentityRole<string>
    {
        public JwtIdentityRole()
        {
            Id = StringHandler.AutoId();
        }

        public JwtIdentityRole(string roleName) : this()
        {
            Name = roleName;
        }
    }


    public class JwtIdentityRole<TKey> where TKey : IEquatable<TKey>
    {
        public JwtIdentityRole() { }

        public JwtIdentityRole(string roleName) : this()
        {
            Name = roleName;
        }

        /// <summary>
        /// Gets or sets the primary key for this role.
        /// </summary>
        public virtual TKey Id { get; set; }

        /// <summary>
        /// Gets or sets the name for this role.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the DomainId for this role.
        /// </summary>
        public virtual string DomainId { get; set; }

        /// <summary>
        /// Gets or sets the Description for this role.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the Description for this role.
        /// </summary>
        public virtual int Status { get; set; }

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

        /// <summary>
        /// Returns the name of the role.
        /// </summary>
        /// <returns>The name of the role.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}
