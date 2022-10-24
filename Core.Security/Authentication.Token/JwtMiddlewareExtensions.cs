using Core.Security.Authentication.Token;
using Core.Utility.Common;
using Jwt.Identity.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Authentication.Token
{
    public static class JwtMiddlewareExtensions
    {
        public static IApplicationBuilder UseMiddlewareJwt<TUser, TRole>(this IApplicationBuilder builder) 
            where TUser : JwtIdentityUser<string>, new()
            where TRole : JwtIdentityRole<string>, new()
        {
            Guard.ArgumentNotNull(builder, "builder");
            return builder.UseMiddleware<JwtMiddleware<TUser, TRole>>();
        }
    }
}
