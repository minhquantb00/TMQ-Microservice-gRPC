
using TMQ.BaseApplication;
using TMQ.Config;
using TMQ.GoogleService;
using TMQ.HttpClientBase;

BaseProgram.Main(args, services =>
{
    services.AddTransient<FileAppService>();

    services.AddTransient<IGoogleReCaptChaService>(p =>
    {
        var httpClientResolver = p.GetRequiredService<HttpClientResolver>();
        var httpClient = httpClientResolver(HttpClientNameEnum.GoogleAPI.ToString());
        return new GoogleReCaptChaService(ConfigSettingEnum.GoogleRecaptchaDomain.GetConfig(), httpClient);
    });
    return services;
}, endpoints =>
{
    endpoints.UseStaticFiles();
});