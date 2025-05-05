using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;

namespace TMQ.Cache
{
    public class RedisConnection(string serverIp, string passWord, ILogger<RedisConnection> logger)
    {
        private const int IoTimeOut = 5000;
        private const int SyncTimeout = 5000;
        private ConnectionMultiplexer? _connection;
        private readonly SemaphoreSlim _semaphore = new(1, 1);

        private async Task<ConnectionMultiplexer?> CreateConnection(string ip)
        {
            var config = ConfigurationOptions.Parse(ip);
            config.KeepAlive = 180;
            config.SyncTimeout = SyncTimeout;
            config.AbortOnConnectFail = false;
            config.AllowAdmin = true;
            config.ConnectTimeout = IoTimeOut;
            config.SocketManager = new SocketManager(GetType().Name);
            config.ConnectRetry = 1000;
            config.Password = passWord;
            config.ReconnectRetryPolicy = new LinearRetry(5000);
            //retry#    retry to re-connect after time in milliseconds
            var connection = await ConnectionMultiplexer.ConnectAsync(config);
            connection.ConnectionFailed += (_, e) =>
            {
                logger.LogError("Connection to Redis failed:{Ip} {ExceptionMessage}", ip, e.Exception?.Message);
            };

            if (!connection.IsConnected)
            {
                logger.LogError("Did not connect to Redis:{Ip}", ip);
            }

            return connection;
        }

        public async Task MakeConnection()
        {
            try
            {
                await _semaphore.WaitAsync();
                var connections = await CreateConnection(serverIp);
                _connection = connections;
            }
            catch (Exception e)
            {
                logger.LogError("{Message}", e.Message);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public IDatabase? GetDatabase(int dbId = 1)
        {
            for (int i = 0; i < 10; i++)
            {
                var connection = _connection;
                if (connection is { IsConnected: true })
                {
                    var db = connection.GetDatabase(dbId);
                    return db;
                }

                logger.LogError("can not connect to redis connection {I}: {ServerIp}", i, serverIp);
            }

            logger.LogError("can not connect to redis connection");
            return null;
        }

        public IServer? GetServer()
        {
            string host = serverIp.Split(",", StringSplitOptions.RemoveEmptyEntries)[0];
            var connection = _connection;
            if (connection is { IsConnected: true })
            {
                return connection.GetServer(host);
            }

            logger.LogError("can not connect to redis connection");
            return null;
        }
    }

    public class RedisConnectionPersistence(string serverIp, string passWord, ILogger<RedisConnection> logger)
        : RedisConnection(serverIp, passWord, logger)
    {

    }
}
