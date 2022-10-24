using Core.Utility.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Authorization.Core
{
    public class AuthorizationAction
    {
        /// <summary>
        /// The claim type
        /// </summary>
        //public const string ClaimType = "http://www.Authorization.Core/claims/authorization_action_mask";

        /// <summary>
        /// Gets the DOMAIN_ID.
        /// </summary>
        /// <value>
        /// The application identifier.
        /// </value>
        public virtual string DomainId { get; set; }

        /// <summary>
        /// Gets or sets the AUTHORITY_ID for this authorization action.
        /// </summary>
        public virtual string AuthorityId { get; internal set; }

        /// <summary>
        /// Gets or sets the name for this authorization action.
        /// </summary>
        public virtual string Name { get; internal set; }

        /// <summary>
        /// Gets the Status for this authorization action.
        /// </summary>
        public virtual int Status { get; internal set; }

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
        /// Navigation property for the roles this authorization action belongs to.
        /// </summary>
        public virtual ICollection<RoleAction> Roles { get; } = new List<RoleAction>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationAction" /> class.
        /// </summary>
        /// <param name="name">The name of the authorization action.</param>
        /// <param name="authorizationCode">The authorization code.</param>
        /// <param name="appId">The application identifier.</param>
        /// <exception cref="ArgumentNullException">The argument <paramref name="name" /> is null.</exception>
        public AuthorizationAction(string name, string authorityId, string domainId = Constants.DefaultApplicationId)
        {
            Guard.ArgumentNotNullOrWhiteSpace(name, "name");
            Guard.ArgumentNotNullOrWhiteSpace(domainId, "domainId");

            this.Name = name;
            this.AuthorityId = authorityId;
            this.DomainId = domainId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationAction" /> class.
        /// </summary>
        internal AuthorizationAction()
        {
            this.DomainId = Constants.DefaultApplicationId;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }
    }
}
