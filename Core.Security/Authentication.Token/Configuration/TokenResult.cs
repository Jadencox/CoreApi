using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Authentication.Token.Configuration
{
    public class TokenResult
    {
        /// <summary>
        /// Token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 持续时间（天）
        /// </summary>
        public int Expiration { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpiryTime { get; set; }
    }
}
