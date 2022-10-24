using System;
using System.Collections.Generic;
using System.Text;

namespace Jwt.Identity.Core
{

    public class JwtIdentityUserRole : JwtIdentityUserRole<string>
    {

    }

    public class JwtIdentityUserRole<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the primary key of the user that is linked to a role.
        /// </summary>
        public virtual TKey UserId { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the role that is linked to the user.
        /// </summary>
        public virtual TKey RoleId { get; set; }

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
    }
}
