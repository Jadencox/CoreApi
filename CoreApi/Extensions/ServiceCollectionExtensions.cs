using Citms.Common.Mapping;
using Core.Admin.DataAccess;
using Core.Common.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CoreApi.Extensions
{
    /// <summary>
    /// ServiceCollection extend methods
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
        public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration Configuration)
        {
            //Guard.ArgumentNotNull(services, "services");
            //Guard.ArgumentNotNull(Configuration, "Configuration");
            //EFCoreLocator.RegisterProviderRelate("Oracle.EntityFrameworkCore", EFCoreExtend.SqlGenerator.DBType.Oracle);


            services.AddScoped(x => new DbConnectSource(GetConnectString(Configuration)));

            //Add module DbContext
            services.AddDoCareDbContext<UserManageDbContext>(Configuration);

            //services.AddDoCareDbContext<WorkStationDbContext>(Configuration);


            return services;
        }

        private static string GetConnectString(IConfiguration Configuration)
        {
            var connectString = Configuration.GetConnectionString("DefaultConnection");
            //if (!connectString.StartsWith("Data Source"))
            //{
            //    connectString = EncyptHandler.DESDecrypt(connectString);
            //}

            return connectString;
        }

        /// <summary>
        /// Oracle 11g Settings
        /// </summary>
        /// <typeparam name="TContext"></typeparam>
        /// <param name="services"></param>
        /// <param name="Configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddDoCareDbContext<TContext>(this IServiceCollection services, IConfiguration Configuration) where TContext : DbContext
        {
            services.AddDbContext<TContext>((service, options) => options.UseOracle11(Configuration, service));

            var fields = typeof(TContext).GetRuntimeFields();
            foreach (var field in fields)
            {
                var fieldType = field.FieldType;
                if (fieldType.IsGenericType)
                {
                    var genericTypes = fieldType.GenericTypeArguments;
                    foreach (var genericType in genericTypes)
                    {
                        MappingResolver.CreateEntityMap(genericType);
                    }
                }
            }

            return services;
        }

        /// <summary>
        /// Oracle 11g Settings
        /// </summary>
        /// <param name="options"></param>
        /// <param name="configuration"></param>
        /// <param name="service"></param>
        /// <returns></returns>
        public static DbContextOptionsBuilder UseOracle11(this DbContextOptionsBuilder options, IConfiguration configuration, IServiceProvider service = null)
        {
            if (service != null)
            {
                var dbConnectSource = service.GetRequiredService<DbConnectSource>();
                var dbConnection = dbConnectSource.SqlConnection;
                return options.UseOracle(dbConnection, b => b.UseOracleSQLCompatibility("11"));
            }

            return options.UseOracle(GetConnectString(configuration), b => b.UseOracleSQLCompatibility("11"));
            //return options.UseOracle(Configuration.GetConnectionString("DefaultConnection"));
        }

        ///// <summary>
        ///// Build an <see cref="ExceptionManager"/> and add it in specified service collection.
        ///// </summary>
        ///// <param name="services">The service collection.</param>
        ///// <param name="configuration">The delegate to build the <see cref="ExceptionManager"/>.</param>
        ///// <returns>The current service collection.</returns>
        ///// <exception cref="ArgumentNullException">The specified argument <paramref name="services"/> is null.</exception>
        ///// <exception cref="ArgumentNullException">The specified argument <paramref name="configuration"/> is null.</exception>
        //public static IServiceCollection AddDictTable(this IServiceCollection services, IConfiguration configuration)
        //{
        //    Guard.ArgumentNotNull(services, "services");
        //    Guard.ArgumentNotNull(configuration, "Configuration");

        //    services.AddDbContext<DictDbContext>((service, dbOption) => dbOption.UseOracle11(configuration, service));

        //    services.AddDictCodeTable(option => option
        //        .AddDicProvider<BedRec, BedRecProvider>()
        //        .AddDicProvider<DataArchiveRule, DataArchiveRuleProvider>()
        //        .AddDicProvider<DataArchiveConfig, DataArchiveConfigProvider>()
        //        .AddDicProvider<DataArchiveRuleMapping, DataArchiveRuleMappingProvider>()
        //        .AddDicProvider<DataArchiveTemplate, DataArchiveTemplateProvider>()
        //        .AddDicProvider<DataMapping, DataMappingProvider>()
        //        .AddDicProvider<DeptDict, DeptDictProvider>()
        //        .AddDicProvider<OrderDict, OrderDictProvider>()
        //        .AddDicProvider<MonitorDict, MonitorDictProvider>()
        //        .AddDicProvider<NursingScheduleDict, NursingScheduleDictProvider>()
        //        .AddDicProvider<BodyPartDict, BodyPartDictProvider>()
        //        .AddDicProvider<CathDict, CathDictProvider>()
        //        .AddDicProvider<CathPropertyConfig, CathPropertyConfigProvider>()
        //        .AddDicProvider<CathPropertyDict, CathPropertyDictProvider>()
        //        .AddDicProvider<DocumentDict, DocumentDictProvider>()
        //        .AddDicProvider<DrugTagEntity, DrugTagProvider>()
        //        .AddDicProvider<GeneralConfig, GeneralConfigProvider>()
        //        .AddDicProvider<GeneralDict, GeneralDictProvider>()
        //        .AddDicProvider<MonitorModule, MonitorModuleProvider>()
        //        .AddDicProvider<NurseTemplate, NurseTemplateProvider>()
        //        .AddDicProvider<OrderCClassDict, OrderCClassDictProvider>()
        //        .AddDicProvider<OrderCClassMapping, OrderCClassMappingProvider>()
        //        .AddDicProvider<OrderFreqDict, OrderFreqDictProvider>()
        //        .AddDicProvider<OrderFreqMapping, OrderFreqMappingProvider>()
        //        .AddDicProvider<OrderTemplate, OrderTemplateProvider>()
        //        .AddDicProvider<PharmacyCategoryMapping, PharmacyCategoryMappingProvider>()
        //        .AddDicProvider<PharmacyNutriDict, PharmacyNutriDictProvider>()
        //        .AddDicProvider<PharmacyRegularConfig, PharmacyRegularConfigProvider>()
        //        .AddDicProvider<ScoreDetailConfig, ScoreDetailConfigProvider>()
        //        .AddDicProvider<ScoreDict, ScoreDictProvider>()
        //        .AddDicProvider<SignMapping, SignMappingProvider>()
        //        .AddDicProvider<SignDict, SignDictProvider>()
        //        .AddDicProvider<SignMapping, SignMappingProvider>()
        //        .AddDicProvider<SysConfig, SystemConfigProvider>()
        //        .AddDicProvider<SysUser, SysUserProvider>()
        //        .AddDicProvider<ScoreGradeDict, ScoreGradeDictProvider>()
        //        .AddDicProvider<NursingPlanTemp, NursingPlanTempProvider>()
        //        .AddDicProvider<NursingPlanTempItem, NursingPlanTempItemProvider>()
        //        .AddDicProvider<AuthorityExtension, AuthorityExtensionProvider>()
        //        .AddDicProvider<SysDept, SysDeptProvider>()
        //        .AddDicProvider<ChecklistDict, ChecklistDictProvider>()
        //        .AddDicProvider<ChecklistTemplate, ChecklistTemplateProvider>()
        //        .AddDicProvider<PatOverviewDict, PatOverviewDictProvider>()
        //        .AddDicProvider<NotifyScreenConfigEntity, NotifyScreenConfigProvider>()
        //        .AddDicProvider<AdvancedQueryConfig, AdvancedQueryConfigProvider>()
        //        .AddDicProvider<OrderTransDict, OrderTransDictProvider>()
        //    );

        //    return services;
        //}

        ///// <summary>
        ///// 根据MapTo属性自动注入容器
        ///// </summary>
        ///// <param name="services"></param>
        ///// <param name="projectName">项目名称</param>
        ///// <param name="configure"></param>
        ///// <returns></returns>
        //public static IServiceCollection AddServices(this IServiceCollection services, string projectName, Action<AssemblyOption> configure = null)
        //{
        //    var location = Assembly.GetEntryAssembly().Location;
        //    var baseDirectory = Path.GetDirectoryName(location);
        //    var serviceProvider = services.BuildServiceProvider();

        //    AssemblyOption assemblyOption = new AssemblyOption();
        //    if (configure != null)
        //    {
        //        configure(assemblyOption);
        //    }
        //    else
        //    {
        //        assemblyOption = serviceProvider.GetRequiredService<IOptions<AssemblyOption>>().Value;
        //    }

        //    var assemblies = new List<Assembly>();
        //    var dllList = new List<string>();
        //    assemblyOption.Services?.RemoveEmptySplit(",").ToList().ForEach(service =>
        //    {
        //        if (string.IsNullOrEmpty(service))
        //        {
        //            return;
        //        }

        //        var dll = Directory.GetFiles(baseDirectory, $"{service}.dll", SearchOption.TopDirectoryOnly)[0];
        //        dllList.Add(dll);
        //    });
        //    assemblyOption.DataAccess?.RemoveEmptySplit(",").ToList().ForEach(dataAccess =>
        //    {
        //        if (string.IsNullOrEmpty(dataAccess))
        //        {
        //            return;
        //        }

        //        var dll = Directory.GetFiles(baseDirectory, $"{dataAccess}.dll", SearchOption.TopDirectoryOnly)[0];
        //        dllList.Add(dll);
        //    });
        //    if (!string.IsNullOrWhiteSpace(projectName))
        //    {
        //        var fileList = Directory.GetFiles(baseDirectory, $"{projectName}.dll", SearchOption.TopDirectoryOnly);
        //        if (fileList.Length <= 0)
        //        {
        //            throw new Exception("未创建对应的服务层");
        //        }

        //        dllList.Add(fileList[0]);
        //    }

        //    assemblyOption.RemoteServices?.RemoveEmptySplit(",").ToList().ForEach(remoteService =>
        //    {
        //        if (string.IsNullOrEmpty(remoteService))
        //        {
        //            return;
        //        }

        //        var dll = Directory.GetFiles(baseDirectory, $"{remoteService}.dll", SearchOption.TopDirectoryOnly)[0];
        //        dllList.Add(dll);
        //    });

        //    foreach (var fileFullPath in dllList)
        //    {
        //        var assemblyName = AssemblyLoadContext.GetAssemblyName(fileFullPath);
        //        var assembly = Assembly.Load(assemblyName);
        //        assemblies.Add(assembly);
        //    }

        //    var factory = new MultipleServiceFactory();

        //    var registerServices = new Dictionary<Type, Type>();

        //    foreach (var assembly in assemblies)
        //    {
        //        var types = assembly.GetExportedTypes();
        //        foreach (var type in types)
        //        {
        //            //DI for business service
        //            if (CompareType(type, typeof(ServiceBase)))
        //            {
        //                var mapToAttribute = type.GetTypeInfo().CustomAttributes.FirstOrDefault(p => p.AttributeType == typeof(MapToAttribute));
        //                if (mapToAttribute != null)
        //                {
        //                    var a = mapToAttribute.ConstructorArguments.FirstOrDefault();
        //                    if (a != null)
        //                    {
        //                        var b = mapToAttribute.ConstructorArguments.Count > 1 ? mapToAttribute.ConstructorArguments[1].Value?.ToString() : string.Empty;
        //                        if (!string.IsNullOrEmpty(b))
        //                        {
        //                            factory.AddService((Type)a.Value, type, b, services);
        //                        }
        //                        else
        //                        {
        //                            AddServiceToContainer(registerServices, (Type)a.Value, type, projectName, mapToAttribute);
        //                        }
        //                    }
        //                }
        //            }
        //            //DI for DbContext
        //            else if (CompareType(type, typeof(RepositoryBase)))
        //            {
        //                //services.AddScoped(type, type);
        //                services.AddScoped(type, provider =>
        //                {
        //                    //var service = provider.GetService(type);
        //                    var service = ActivatorUtilities.CreateInstance(provider, type);
        //                    ((RepositoryBase)service).Provider = provider;
        //                    return service;

        //                });
        //            }
        //            //DI for remote service
        //            else if (CompareType(type, typeof(IRemoteInterface)))
        //            {
        //                services.AddScoped(type, service =>
        //                {
        //                    var proxyFactory = service.GetRequiredService<IProxyFactory>();
        //                    return proxyFactory.CreateProxy(type, null);
        //                });
        //            }

        //        }
        //    }

        //    foreach (var registerServiceItem in registerServices)
        //    {
        //        services.AddScoped(registerServiceItem.Key, registerServiceItem.Value);
        //    }

        //    services.AddSingleton(factory);

        //    return services;
        //}

        //private static void AddServiceToContainer(Dictionary<Type, Type> container, Type interfaceType, Type serviceType, string hospitalCode, CustomAttributeData customAttributeData)
        //{
        //    if (!container.ContainsKey(interfaceType))
        //    {
        //        container.Add(interfaceType, serviceType);
        //        return;
        //    }

        //    var hospitalKey = customAttributeData.ConstructorArguments.Count >= 3 ? customAttributeData.ConstructorArguments[2].Value?.ToString() : string.Empty;
        //    if (string.IsNullOrEmpty(hospitalKey) || hospitalKey != hospitalCode)
        //    {
        //        return;
        //    }

        //    container[interfaceType] = serviceType;
        //}

        //private static bool CompareType(Type sourceType, Type baseType)
        //{
        //    if (sourceType.GetTypeInfo().BaseType == baseType)
        //    {
        //        return true;
        //    }

        //    if (sourceType.GetTypeInfo().ImplementedInterfaces.Contains(baseType))
        //    {
        //        return true;
        //    }

        //    if (sourceType.GetTypeInfo().BaseType == typeof(object))
        //    {
        //        return false;
        //    }

        //    if (sourceType.GetTypeInfo().BaseType != null)
        //    {
        //        return CompareType(sourceType.GetTypeInfo().BaseType, baseType);
        //    }

        //    return false;
        //}

        ///// <summary>
        ///// 注入带有键名的Service到容器(可以注册多个service 到 同一个interface)
        ///// </summary>
        ///// <param name="services"></param>
        ///// <param name="option"></param>
        ///// <returns></returns>
        //public static IServiceCollection AddMultipleServices(this IServiceCollection services, Action<MultipleServiceFactory> option)
        //{
        //    var factory = new MultipleServiceFactory();
        //    option?.Invoke(factory);
        //    services.AddSingleton(factory);
        //    return services;
        //}


        ///// <summary>
        ///// 注册DoCare 缓存管理类
        ///// </summary>
        ///// <param name="services"></param>
        ///// <returns></returns>
        //public static IServiceCollection AddDoCareCacheManagement(this IServiceCollection services)
        //{
        //    Guard.ArgumentNotNull(services, "services");
        //    return services.AddSingleton<IDbContextCacheManagement, DoCareCacheManagement>();
        //}
    }
}
