


using Elastic.Serilog.Sinks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using ProtoBuf.Grpc.Server;
using Serilog;
using Serilog.Events;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using TMQ.BaseApplication.HosterdServices;
using TMQ.BaseRepositories;
using TMQ.Cache;
using TMQ.Common;
using TMQ.Config;
using TMQ.EventBus;
using TMQ.GrpcServer;

namespace TMQ.BaseApplication
{
    public abstract class BaseProgram
    {
        public static string? HostName;
        public static string? HostIp;
        public static void Main(string[] args, Func<IServiceCollection, IServiceCollection>? registerServiceFunc, Action<WebApplication>? registerRoutingUrl)
        {
            HostName = System.Net.Dns.GetHostName();
            var t = System.Net.Dns.GetHostEntry(HostName);
            foreach(var address in t.AddressList)
            {
                string ip = address.ToString();
                if(address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    HostIp = ip;
                }
            }

            ThreadPool.GetMinThreads(out var minWorker, out var minToc);
            ThreadPool.GetMaxThreads(out var maxWorker, out var maxToc);

            

            
            var builder = WebApplication.CreateBuilder(new WebApplicationOptions
            {
                WebRootPath = HostName,
                Args = args,
            });

            ConfigSetting.Init(builder.Configuration);
            ConfigSetting.Init(builder.Configuration);

            string appName = ConfigSettingEnum.AppName.GetConfig();
            string appVersion = ConfigSettingEnum.AppVersion.GetConfig();

            LogEventLevel logEventLevel = (LogEventLevel)ConfigSettingEnum.LogEventLevel.GetConfig().AsInt();

            string logEsUrl = ConfigSettingEnum.LogToEsUrl.GetConfig();
            string logPath = Path.Combine(Environment.CurrentDirectory, "log");
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", logEventLevel)
            .Enrich.WithProperty("HostName", HostName)
            .Enrich.WithProperty("AppName", appName)
            .Enrich.WithProperty("AppVersion", appVersion)
            .Enrich.FromLogContext()
            .WriteTo.Console(
                outputTemplate:
                "[{Level} {Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] {Message} {Properties}{NewLine}{Exception}",
                restrictedToMinimumLevel: logEventLevel)
            .WriteTo.File(
                $"{logPath}/log-.txt",
                fileSizeLimitBytes: 1_000_000,
                rollOnFileSizeLimit: true,
                shared: false,
                flushToDiskInterval: TimeSpan.FromSeconds(5),
                rollingInterval: RollingInterval.Day,
                restrictedToMinimumLevel: logEventLevel,
                buffered: true
            );
            if (logEsUrl.Length > 0)
            {
                loggerConfiguration =
                    loggerConfiguration.WriteTo.Elasticsearch([new Uri(logEsUrl)],
                        options => { options.MinimumLevel = LogEventLevel.Warning; });
            }

            Serilog.Log.Logger = loggerConfiguration.CreateLogger();
            int httpPort = ConfigSettingEnum.HttpPort.GetConfig().AsInt();
            if (httpPort <= 0)
            {
                httpPort = 30000;
            }
            builder.Host.UseContentRoot(Directory.GetCurrentDirectory());
            builder.Host.UseSerilog();
            int httpType = ConfigSettingEnum.HttpType.GetConfig().AsInt();
            if (httpType != 2)
            {
                httpType = 1;
            }

            builder.WebHost.UseKestrel(options =>
            {
                options.AllowSynchronousIO = true;
                options.Limits.MinRequestBodyDataRate = null;
                options.Limits.MaxRequestBodySize = 50971520000;
                options.Limits.RequestHeadersTimeout = TimeSpan.FromMinutes(60);
                options.Limits.MaxConcurrentConnections = int.MaxValue;
                options.Limits.MaxConcurrentUpgradedConnections = int.MaxValue;
                options.Limits.MaxRequestBufferSize = null;
                options.Limits.MaxResponseBufferSize = null;
                var http2 = options.Limits.Http2;
                http2.InitialConnectionWindowSize = 2 * 1024 * 1024 * 2;
                http2.InitialStreamWindowSize = 1024 * 1024; 
                http2.MaxStreamsPerConnection = int.MaxValue;
                if (httpType == 1)
                {
                    options.ListenAnyIP(httpPort,
                        listenOptions => { listenOptions.Protocols = HttpProtocols.Http1; });
                }
                else if (httpType == 2)
                {
                    options.ListenAnyIP(httpPort,
                        listenOptions => { listenOptions.Protocols = HttpProtocols.Http2; });
                }
            });
            builder.Services.Configure<HostOptions>(options => { options.ShutdownTimeout = TimeSpan.FromMinutes(10); });
            builder.Services.AddLogging(p =>
                p.AddSerilog(Serilog.Log.Logger)
            );

            builder.Services.ConfigCodeFirstGrpc();
            builder.Services.AddHttpContextAccessor();
            //builder.Services.AddTransient<ContextService>();
            //builder.Services.AddTransient<AuthenService>();
            //builder.Services.AddSingleton<ILogAction, LogAction>();
            string dbConnectionString = ConfigSettingEnum.DbConnectionString.GetConfig();
            if (dbConnectionString.Length > 0)
            {
                builder.Services.AddTransient<IDbConnectionFactory>(_ => new DbConnectionFactory(dbConnectionString));
            }

            string rabbitMqHost = ConfigSettingEnum.RabbitMqHost.GetConfig();
            if (rabbitMqHost.Length > 0)
            {
                builder.Services.AddSingleton<RabbitMqConnectionPool>(sp =>
                {
                    ILogger<RabbitMqConnectionPool> logger = sp.GetRequiredService<ILogger<RabbitMqConnectionPool>>();
                    int poolSize = ConfigSettingEnum.RabbitMqPoolSize.GetConfig().AsInt();
                    if (poolSize <= 0)
                    {
                        poolSize = 1;
                    }

                    return new RabbitMqConnectionPool(logger, poolSize);
                });
            }
            var authenticationType = ConfigSettingEnum.AuthenticationType.GetConfig().AsInt();
            if (authenticationType > 0)
            {
                IdentityModelEventSource.ShowPII = true;

                builder.Services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, cfg =>
                    {
                        cfg.RequireHttpsMetadata = false;
                        cfg.SaveToken = true;
                        cfg.TokenValidationParameters = new TokenValidationParameters()
                        {
                            ValidateIssuerSigningKey = true,
                            ValidateIssuer = false,
                            ValidateAudience = false,
                            IssuerSigningKey =
                                new SymmetricSecurityKey(
                                    Encoding.ASCII.GetBytes(ConfigSettingEnum.JwtTokensKey.GetConfig())),
                            ValidateLifetime = true
                        };
                    });
                builder.Services.AddAuthorization();
                // var requireAuthPolicy = new AuthorizationPolicyBuilder()
                //     .RequireAuthenticatedUser()
                //     .Build();
                // builder.Services.AddAuthorizationBuilder()
                //     .SetDefaultPolicy(requireAuthPolicy);

                string dataProtectionRedisKey =
                    $"DataProtectionRedisKey_TYT_SSO_{ConfigSettingEnum.DataProtectionRedisKey.GetConfig()}";
                using var scope = builder.Services.BuildServiceProvider().CreateScope();
                var connectionPersistence = scope.ServiceProvider.GetRequiredService<RedisConnectionPersistence>();
                connectionPersistence.MakeConnection().GetAwaiter().GetResult();
                IDataProtectionBuilder dataProtectionBuilder = builder
                    .Services
                    .AddDataProtection()
                    .SetApplicationName(dataProtectionRedisKey)
                    .PersistKeysToStackExchangeRedis(
                        () => connectionPersistence.GetDatabase(Constant.RedisDbIdDataProtectionKey)!,
                        $"{dataProtectionRedisKey}")
                    .UseCustomCryptographicAlgorithms(
                        new ManagedAuthenticatedEncryptorConfiguration()
                        {
                            EncryptionAlgorithmType = typeof(Aes),
                            EncryptionAlgorithmKeySize = 256,
                            ValidationAlgorithmType = typeof(HMACSHA512)
                        });
                if (ConfigSettingEnum.AccountManagerAutomaticKeyGeneration.GetConfig().AsInt() != 1)
                {
                    dataProtectionBuilder.DisableAutomaticKeyGeneration();
                }
            }

            string corsConfig = ConfigSettingEnum.CORS.GetConfig().AsEmpty();
            if (corsConfig.Length > 0)
            {
                builder.Services.AddCors(options =>
                {
                    var configs = corsConfig.Split(',');
                    options.AddPolicy(Constant.CorsPolicy,
                        policyBuilder => policyBuilder.WithOrigins(configs).AllowAnyHeader().AllowCredentials()
                    );
                });
            }

            builder.Services.AddControllers(options =>
            {
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
            builder.Services.AddResponseCompression();

            if (ConfigSettingEnum.StartWorker.GetConfig().AsInt() == 1)
            {
                builder.Services.AddTransient<IEventStorageRepository>(_ => new EventStorageRepository(
                    ConfigSettingEnum.EventDatabaseConnectionString.GetConfig(),
                    ConfigSettingEnum.EventDatabaseName.GetConfig(),
                    ConfigSettingEnum.EventCollectionName.GetConfig()));

                string exChange = ConfigSettingEnum.RabbitMqExChange.GetConfig();
                string exChangeNotify = ConfigSettingEnum.RabbitMqExChangeNotifyListen.GetConfig();
                string exChangesTriggerConfig = ConfigSettingEnum.RabbitMqExChangeTriggerListen.GetConfig();
                if (exChange.Length > 0 || exChangeNotify.Length > 0 || exChangesTriggerConfig.Length > 0)
                {
                    builder.Services.AddSingleton<IEventProcessor, EventProcessor>();
                }
            }

            builder.Services.AddHostedService<AppInitHostedService>();
            if (registerServiceFunc != null)
            {
                registerServiceFunc(builder.Services);
            }

            var app = builder.Build();

            if (ConfigSettingEnum.Https.GetConfig().AsInt() == 1)
            {
                app.Use(async (ctx, next) =>
                {
                    ctx.Request.Scheme = "https";
                    if (ctx.Request.Path.HasValue && ctx.Request.Path.Value.Contains("/."))
                    {
                        ctx.Request.Path = "/error/404/";
                    }

                    await next();
                });
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseSerilogRequestLogging();
            app.UseRouting();
            if (corsConfig.Length > 0)
            {
                app.UseCors(Constant.CorsPolicy);
            }

            var forwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedHost | ForwardedHeaders.XForwardedFor |
                                   ForwardedHeaders.XForwardedProto
            };
            forwardedHeaderOptions.KnownProxies.Clear();
            forwardedHeaderOptions.KnownNetworks.Clear();
            app.UseForwardedHeaders(forwardedHeaderOptions);
            app.UseResponseCompression();

            if (authenticationType > 0)
            {
                app.UseAuthentication();
                app.UseAuthorization();
            }
            if (registerRoutingUrl != null)
            {
                registerRoutingUrl(app);
            }

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapCodeFirstGrpcReflectionService();

            app.Run();
        }
    }
}
