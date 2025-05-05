using TMQ.Cache;
using TMQ.EventBus;

namespace TMQ.BaseApplication.HosterdServices
{
    public class AppInitHostedService(ILogger<AppInitHostedService> logger, IServiceProvider services)
    : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("App Init Hosted Service is starting");
            using var scope = services.CreateScope();
            var connectionPersistence = scope.ServiceProvider.GetService<RedisConnectionPersistence>();
            if (connectionPersistence != null)
            {
                await connectionPersistence.MakeConnection();
            }

            var redisConnectionPool = scope.ServiceProvider.GetService<RedisConnectionPool>();
            if (redisConnectionPool != null)
            {
                await redisConnectionPool.Init(() =>
                    scope.ServiceProvider.GetRequiredService<ILogger<RedisConnection>>());
            }

            var rabbitMqConnectionPool = scope.ServiceProvider.GetService<RabbitMqConnectionPool>();
            rabbitMqConnectionPool?.Init(scope.ServiceProvider);

            var scopedProcessingService = scope.ServiceProvider.GetService(typeof(IEventProcessor));
            if (scopedProcessingService != null)
            {
                IEventProcessor eventProcessor = (IEventProcessor)scopedProcessingService;
                eventProcessor.Register();
                await eventProcessor.Start();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("App Init Hosted Service is stopping");
            return Task.CompletedTask;
        }
    }
}
