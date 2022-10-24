using Core.Security.Authentication.Token.Configuration;
using Jwt.Identity.Core;

namespace Core.Authentication.Token
{
    public interface IJwtIdentityTokenManager<TUser, TRole>
        where TUser : JwtIdentityUser<string>, new()
        where TRole : JwtIdentityRole<string>, new()
    {
        TokenResult GenerateToken(string username);
    }
}