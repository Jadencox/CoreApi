using Core.Utility.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Authorization.Core
{
    public class RoleAction
    {

        /// <summary>
        /// Gets or sets the normalized name of the role that is linked to an authorization action.
        /// </summary>
        public string RoleId { get; internal set; }

        /// <summary>
        /// Gets or sets the normalized name of the authorization action that is linked to a role.
        /// </summary>
        public string AuthorityId { get; internal set; }

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
        /// Initializes a new instance of the <see cref="RoleAction" /> class.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <param name="actionName">Name of the authorization action.</param>
        /// <param name="appId">The application identifier.</param>
        public RoleAction(string roleId, string authorityId)
        {
            Guard.ArgumentNotNullOrWhiteSpace(roleId, "roleId");

            this.RoleId = roleId;
            this.AuthorityId = authorityId;
        }

        internal RoleAction()
        { }
    }
}
