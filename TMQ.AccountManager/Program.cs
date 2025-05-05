
using AccountRepositorySQLImplement;
using Microsoft.AspNetCore.Identity;
using TMQ.AccountDomains.Entities;
using TMQ.AccountManager.Handlers;
using TMQ.AccountManager.Services;
using TMQ.AccountManager.Shared;
using TMQ.AccountRepository;
using TMQ.AccountRepositorySQLImplement;
using TMQ.BaseApplication;
using TMQ.BaseRepositories;
using TMQ.Common;
using TMQ.Config;
using TMQ.EnumDefine;
using TMQ.EsRepositories;
using TMQ.EventBus;
using TMQ.HttpClientBase;

BaseProgram.Main(args, services =>
{
    services
        .AddTransient<IMessageHandler, UserEventHandler>()
        .AddTransient<UserEventHandler>();

    services.AddTransient<TMQ.AccountOldRepositorySQLImplement.UserRepository>(p =>
    {
        string dbConnectionString = ConfigSettingEnum.DbOldConnectionString.GetConfig();
        return new TMQ.AccountOldRepositorySQLImplement.UserRepository(new DbConnectionFactory(dbConnectionString));
    });
    services.AddTransient<TMQ.AccountRepositorySQLImplement.UserRepository>();
    services.AddTransient<UserRepositoryResolver>(serviceProvider => key =>
    {
        switch (key)
        {
            case AccountTypeEnum.Local:
            case AccountTypeEnum.CustomerApp:
                return serviceProvider.GetRequiredService<TMQ.AccountRepositorySQLImplement.UserRepository>();
            case AccountTypeEnum.OldSystem:
                return serviceProvider.GetRequiredService<TMQ.AccountOldRepositorySQLImplement.UserRepository>();
            default:
                throw new KeyNotFoundException();
        }
    });


    services.AddTransient<IAccountService, AccountService>();
    

    services.AddTransient<IAccountSettingRepository, AccountSettingRepository>();
    services.AddTransient<IUserDeviceMappingRepository, UserDeviceMappingRepository>();

    services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
    services.AddTransient<OtpUtility>();
    services.AddSingleton<IESRepository>(sp =>
    {
        var logger = sp.GetRequiredService<ILogger<ESRepository>>();
        var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
        var httpClient = httpClientFactory.CreateClient(TMQ.HttpClientBase.HttpClientNameEnum.Default.ToString());
        IHttpClient vnnHttpClient = new TMQ.HttpClientBase.HttpClient(httpClient);
        string url = ConfigSettingEnum.EsUrl.GetConfig();
        string environment = ConfigSettingEnum.EnvironmentName.GetConfig();
        return new ESRepository(logger, url, vnnHttpClient, environment);
    });
    if (ConfigSettingEnum.StartWorker.GetConfig().AsInt() == 1)
    {
        //services.AddHostedService<AccountHostedService>();
    }

    return services;
}, endpoints =>
{
    endpoints.MapGrpcService<AccountService>();
});

public delegate IUserRepository UserRepositoryResolver(AccountTypeEnum accountType);
