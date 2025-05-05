using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.Cache
{
    public class RedisConnectionPool
    {
        private readonly RedisConnection[] _connections;
        private readonly int _poolSize;
        private readonly string _serverIp;
        private readonly string _password;
        private readonly ILogger<RedisConnectionPool> _logger;
        private long _currentConnectionIndex;
        private bool _isInit;
        private readonly int _dbId;

        public RedisConnectionPool(
            string serverIp,
            string password,
            int dbId,
            int poolSize,
            ILogger<RedisConnectionPool> logger)
        {
            _poolSize = poolSize;
            _connections = new RedisConnection[_poolSize];
            _logger = logger;
            _serverIp = serverIp;
            _password = password;
            _dbId = dbId;
        }

        public async Task Init(Func<ILogger<RedisConnection>> getServiceFunc)
        {
            if (_isInit)
            {
                return;
            }

            try
            {
                if (_isInit)
                {
                    return;
                }

                _isInit = true;

                for (int i = 0; i < _poolSize; i++)
                {
                    ILogger<RedisConnection> loggerRedisConnection = getServiceFunc();
                    _connections[i] = new RedisConnection(_serverIp, _password, loggerRedisConnection);
                    await _connections[i].MakeConnection();
                }

                _currentConnectionIndex = 0;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "{Message}", e.Message);
                throw;
            }
        }

        private int GetCurrentConnectionIndex()
        {
            long c = Interlocked.Increment(ref _currentConnectionIndex);
            var index = c % _poolSize;
            return (int)index;
        }

        private RedisConnection GetCurrentConnection()
        {
            int currentConnectionIndex = GetCurrentConnectionIndex();
            return _connections[currentConnectionIndex];
        }

        public IDatabase? GetDatabase()
        {
            RedisConnection connection = GetCurrentConnection();
            return connection.GetDatabase(_dbId);
        }

        public IDatabase? GetDatabase(int dbId)
        {
            RedisConnection connection = GetCurrentConnection();
            return connection.GetDatabase(dbId);
        }

        public IServer? GetServer()
        {
            RedisConnection connection = GetCurrentConnection();
            return connection.GetServer();
        }
    }
}
