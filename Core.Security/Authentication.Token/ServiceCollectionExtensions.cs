using Core.Utility.Common;
using Core.Authentication.Token;
using Core.Authorization.Core;
using Core.Utility.Common;
using Jwt.Identity.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core.Security.Authentication.Token.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Build an <see cref="ExceptionManager"/> and add it in specified service collection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Build an <see cref="ExceptionManager"/> and add it in specified service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="Configuration">The delegate to build the <see cref="ExceptionManager"/>.</param>
        /// <returns>The current service collection.</returns>
        /// <exception cref="ArgumentNullException">The specified argument <paramref name="services"/> is null.</exception>
        /// <exception cref="ArgumentNullException">The specified argument <paramref name="Configuration"/> is null.</exception>
        public static IServiceCollection AddAuthenticationJwt(this IServiceCollection services)
        {
            Guard.ArgumentNotNull(services, "services");

            services.TryAddScoped<IJwtIdentityTokenManager<JwtIdentityUser, JwtIdentityRole>, JwtIdentityTokenManager<JwtIdentityUser, JwtIdentityRole>>();

            var jwtProviderOption = services.BuildServiceProvider().GetRequiredService<IOptions<JwtProviderOption>>();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = jwtProviderOption.Value.DefaultAuthenticateScheme;
                option.DefaultChallengeScheme = jwtProviderOption.Value.DefaultChallengeScheme;

            }).AddJwtBearer("JwtBearer", jwtBearerOption =>
            {
                jwtBearerOption.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtProviderOption.Value.JwtKey)),

                    ValidateIssuer = true,
                    ValidIssuer = jwtProviderOption.Value.Issuer,

                    ValidateAudience = true,
                    ValidAudience = jwtProviderOption.Value.Audience,

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(jwtProviderOption.Value.ClockSkew)
                };
                jwtBearerOption.Events = new AspNetCore.Authentication.JwtBearer.JwtBearerEvents
                {
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        var payload = JsonConvert.SerializeObject(new { RetCode = 405, RetMsg = "未登录", RetObj = "" });
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = 405;
                        context.Response.WriteAsync(payload);
                        return Task.FromResult(0);
                    }
                };
            });
            return services;
        }

        public static IServiceCollection AddAuthorizationAction(this IServiceCollection services, Action<AuthorizationActionBuilder> buildAction = null)
        {
            Guard.ArgumentNotNull(services, "services");
            services.TryAddScoped<AuthorizationActionManager>();
            //services.TryAddScoped<AuthorizationActionManager, AuthorizationActionManager>();
            AuthorizationActionBuilder builder = new AuthorizationActionBuilder(services);
            buildAction?.Invoke(builder);
            return services;
        }
    }
}
