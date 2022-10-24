using Core.Caching.Redis;
using Core.Utility.Common;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRedisCache(this IServiceCollection services)
        {
            Guard.ArgumentNotNull(services, "services");
            //Guard.ArgumentNotNull(Configuration, "Configuration");
            //EFCoreLocator.RegisterProviderRelate("Oracle.EntityFrameworkCore", EFCoreExtend.SqlGenerator.DBType.Oracle);


            services.AddSingleton<IRedisCache, RedisCache>();
            return services;
        }
    }
}
