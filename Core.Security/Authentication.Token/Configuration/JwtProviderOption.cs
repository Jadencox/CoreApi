using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Security.Authentication.Token.Configuration
{
    public class JwtProviderOption
    {
        public string Path { get; set; }

        public string Issuer { get; set; }

        public string DefaultAuthenticateScheme { get; set; }

        public string DefaultChallengeScheme { get; set; }

        public string Audience { get; set; }

        public int Expiration { get; set; }

        public int ClockSkew { get; set; }

        public string JwtKey { get; set; }

        public int MaxErrorTimes { get; set; }
    }
}
