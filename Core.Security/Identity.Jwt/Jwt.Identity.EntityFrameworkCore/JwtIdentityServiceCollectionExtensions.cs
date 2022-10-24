using Jwt.Identity.Core;
using Jwt.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class JwtIdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtIdentity<TUser, TRole, TContext>(this IServiceCollection services)
            where TUser : JwtIdentityUser
            where TRole : JwtIdentityRole
            where TContext : DbContext
        {
            var contextType = typeof(TContext);
            var userType = typeof(TUser);
            var roleType = typeof(TRole);
            var userRoleType = typeof(JwtIdentityUserRole<string>);

            var identityUserType = FindGenericBaseType(userType, typeof(JwtIdentityUser<>));
            var identityRoleType = FindGenericBaseType(roleType, typeof(JwtIdentityRole<>));

            if (identityUserType == null || identityRoleType == null)
            {
                throw new InvalidOperationException("NotIdentityUser");
            }

            var keyType = identityUserType.GenericTypeArguments[0];

            //var identityContext = FindGenericBaseType(contextType, typeof(JwtIdentityDbContext<,,>));

            Type userStoreType = null;

            services.TryAddScoped<IPasswordHasher<TUser>, DoCarePassWordHasher<TUser>>();

            // If its a custom DbContext, we can only add the default POCOs
            userStoreType = typeof(JwtUserStore<,,,,>).MakeGenericType(userType, roleType, userRoleType, keyType, contextType);

            services.TryAddScoped(typeof(JwtUserStoreBase<,,>).MakeGenericType(userType, roleType, keyType), userStoreType);
            return services;

        }

        private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return typeInfo;
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}
