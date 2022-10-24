using System;
using System.Collections.Generic;
using System.Text;

namespace Jwt.Identity.Core
{
    public class JwtIdentityError
    {

        /// <summary>
        /// Gets or sets the code for this error.
        /// </summary>
        /// <value>
        /// The code for this error.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets the description for this error.
        /// </summary>
        /// <value>
        /// The description for this error.
        /// </value>
        public string Description { get; set; }
        
    }
}
