
using TMQ.BaseApplication;
using TMQ.SystemManager.Services;
using TMQ.SystemManager.Shared;
using TMQ.SystemRepository;
using TMQ.SystemRepositorySQLImplement;

BaseProgram.Main(args, servicesCollection =>
{
    servicesCollection.AddTransient<IMenuService, MenuService>();
    servicesCollection.AddTransient<ILanguageService, LanguageService>();
    servicesCollection.AddTransient<ILocaleStringResourceService, LocaleStringResourceService>();
    servicesCollection.AddTransient<IConfigService, ConfigService>();
    servicesCollection.AddTransient<ICommonService, CommonService>();
    servicesCollection.AddTransient<IZoneService, ZoneService>();


    servicesCollection.AddTransient<IMenuRepository, MenuRepository>();
    servicesCollection.AddTransient<ILanguageRepository, LanguageRepository>();
    servicesCollection.AddTransient<ILocaleStringResourceRepository, LocaleStringResourceRepository>();
    servicesCollection.AddTransient<IConfigRepository, ConfigRepository>();
    servicesCollection.AddTransient<ICommonRepository, CommonRepository>();
    servicesCollection.AddTransient<IZoneRepository, ZoneRepository>();
    servicesCollection.AddTransient<IZonePositionHistoryRepository, ZonePositionHistoryRepository>();

    servicesCollection.AddTransient<IShardingRepository, ShardingRepository>();
    servicesCollection.AddTransient<IShardingGroupRepository, ShardingGroupRepository>();

    return servicesCollection;
}, endpoints =>
{
    endpoints.MapGrpcService<MenuService>();
    endpoints.MapGrpcService<LanguageService>();
    endpoints.MapGrpcService<LocaleStringResourceService>();
    endpoints.MapGrpcService<ConfigService>();
    endpoints.MapGrpcService<CommonService>();
    endpoints.MapGrpcService<ZoneService>();
    endpoints.MapGrpcService<ShardService>();
});
