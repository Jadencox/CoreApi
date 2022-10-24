using Core.Authorization.Core;
using Jwt.Identity.Core;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Numerics;
using Core.Utility.Common;
using Core.Security.Authentication.Token.Configuration;

namespace Core.Authentication.Token
{
    public class JwtIdentityTokenManager<TUser, TRole> : IJwtIdentityTokenManager<TUser, TRole>
        where TUser : JwtIdentityUser<string>, new()
        where TRole : JwtIdentityRole<string>, new()
    {
        private JwtUserStoreBase<TUser, TRole, string> _userStore;
        //private AuthorizationActionManager _actionManager;
        private JwtProviderOption _JwtProviderOption;

        public JwtIdentityTokenManager(JwtUserStoreBase<TUser, TRole, string> userStore,
            //AuthorizationActionManager actionManager,
             IOptions<JwtProviderOption> options)
        {
            _userStore = userStore;
            //_actionManager = actionManager;
            _JwtProviderOption = options.Value;
        }

        public TokenResult GenerateToken(string username)
        {
            return GetToken(username);
        }

        /// <summary>
        /// Use generate Jwt token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userStore"></param>
        /// <param name="actionManager"></param>
        /// <returns></returns>
        private TokenResult GetToken(string username)
        {
            //var roles = _userStore.GetRoleByUsername(username).Result;
            //var claims = AddAuthorizationCodeMaskClaim(_actionManager, roles);

            //Extend private claims username
            //claims.Add(new Claim(ClaimTypes.Name, username));

            var user = _userStore.Users.FirstOrDefault(x => x.UserName == username);
            //claims.Add(new Claim(MyClaimTypes.UserId, user.Id));

            var currentTime = DateTimeHandler.CurrentTime;
            var expiryTime = currentTime.AddDays(_JwtProviderOption.Expiration);

            //Extend private claims Roles
            //AddRolesMaskClaim(claims, roles);

            var token = new JwtSecurityToken(
                issuer: _JwtProviderOption.Issuer,
                audience: _JwtProviderOption.Audience,
                //claims: claims.ToArray(),
                notBefore: currentTime,
                expires: expiryTime,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_JwtProviderOption.JwtKey)), SecurityAlgorithms.HmacSha256)
                );
            string jwtToken = new JwtSecurityTokenHandler().WriteToken(token);



            return new TokenResult { Token = jwtToken,  ExpiryTime = expiryTime, Expiration = _JwtProviderOption.Expiration };
        }

        /// <summary>
        /// Adds the authorization code mask claim.
        /// </summary>
        /// <param name="actionManager">The action manager</param>
        /// <param name="roles">Roles</param>
        /// <returns>The task to add the authorization code mask claim.</returns>
        private List<Claim> AddAuthorizationCodeMaskClaim(AuthorizationActionManager actionManager, TRole[] roles)
        {
            var claims = new List<Claim>();
            //if (!roles.Any()) { return claims; }
            //var codes = BigInteger.Zero;

            //var actions = actionManager.GetActionsInRolesAsync(roles.Select(o => o.Id).ToArray()).Result;

            //foreach (var action in actions)
            //{
            //    var c = BigInteger.Pow(2, (int)action.AuthorityId);
            //    codes = codes | c;
            //}
            //claims.Add(new Claim(AuthorizationAction.ClaimType, codes.ToString()));
            return claims;
        }

        /// <summary>
        /// Adds the role mask claim.
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="roles"></param>
        private void AddRolesMaskClaim(List<Claim> claims, TRole[] roles)
        {
            if (!roles.Any())
                return;
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Id));
            }
        }

    }
}
