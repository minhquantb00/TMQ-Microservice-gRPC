using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseEvents;
using TMQ.Config;

namespace TMQ.EventBus
{
    public class RabbitMqConnectionPool(ILogger<RabbitMqConnectionPool> logger, int poolSize)
    {
        private IRabbitMqConnection[]? _rabbitMqConnections;
        private long _currentConnectionIndex;
        private static readonly Lock MakeConnectionLock = new();
        private bool _isInit;

        public void Init(IServiceProvider serviceProvider)
        {
            if (_isInit)
            {
                return;
            }

            try
            {
                lock (MakeConnectionLock)
                {
                    if (_isInit)
                    {
                        return;
                    }

                    _isInit = true;
                    List<IRabbitMqConnection> rabbitMqConnections = [];
                    var rabbitMqHosts = ConfigSettingEnum.RabbitMqHost.GetConfig().Split('|',
                        StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    foreach (var rabbitMqHost in rabbitMqHosts)
                    {
                        var rabbitMqHostIps = rabbitMqHost.Split(',',
                            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < poolSize; i++)
                        {
                            var index = i % poolSize;
                            var rabbitMqIndex = index % rabbitMqHostIps.Length;
                            string rabbitMqHostIpFirst = rabbitMqHostIps[rabbitMqIndex];
                            List<string> rabbitMqHostIpsConnect = [rabbitMqHostIpFirst];
                            foreach (var rabbitMqHostIp in rabbitMqHostIps)
                            {
                                if (!rabbitMqHostIpsConnect.Contains(rabbitMqHostIp))
                                {
                                    rabbitMqHostIpsConnect.Add(rabbitMqHostIp);
                                }
                            }

                            var virtualHost = ConfigSettingEnum.VirtualHost.GetConfig();
                            ConnectionFactory factory;
                            if (string.IsNullOrEmpty(virtualHost))
                            {
                                factory = new ConnectionFactory()
                                {
                                    HostName = "",
                                    UserName = ConfigSettingEnum.RabbitMqUserName.GetConfig(),
                                    Password = ConfigSettingEnum.RabbitMqPassword.GetConfig(),
                                };
                            }
                            else
                            {
                                factory = new ConnectionFactory()
                                {
                                    HostName = "",
                                    UserName = ConfigSettingEnum.RabbitMqUserName.GetConfig(),
                                    Password = ConfigSettingEnum.RabbitMqPassword.GetConfig(),
                                    VirtualHost = virtualHost
                                };
                            }

                            ILogger<DefaultRabbitMqPersistentConnection> logger1 =
                                serviceProvider.GetRequiredService<ILogger<DefaultRabbitMqPersistentConnection>>();
                            IRabbitMqPersistentConnection rabbitMqPersistentConnection =
                                new DefaultRabbitMqPersistentConnection(factory, logger1, rabbitMqHostIpsConnect.ToArray());
                            ILogger<RabbitMqConnection> loggerRabbitMqConnection =
                                serviceProvider.GetRequiredService<ILogger<RabbitMqConnection>>();

                            var rabbitMqConnection =
                                new RabbitMqConnection(rabbitMqPersistentConnection, loggerRabbitMqConnection);
                            rabbitMqConnections.Add(rabbitMqConnection);
                        }
                    }

                    _rabbitMqConnections = rabbitMqConnections.ToArray();
                    _currentConnectionIndex = 0;
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "{Message}", e.Message);
                throw;
            }
        }

        private int GetCurrentConnectionIndex()
        {
            long c = Interlocked.Increment(ref _currentConnectionIndex);
            var index = c % poolSize;
            return (int)index;
        }

        private IRabbitMqConnection GetCurrentConnection()
        {
            int currentConnectionIndex = GetCurrentConnectionIndex();
            return _rabbitMqConnections![currentConnectionIndex];
        }

        public async Task RegisterExchange((string ExChange, string Type)[] exchanges)
        {
            if (exchanges is not { Length: > 0 })
            {
                return;
            }

            if (_rabbitMqConnections is not { Length: > 0 })
            {
                return;
            }

            foreach (var rabbitMqConnection in _rabbitMqConnections)
            {
                await rabbitMqConnection.RegisterExchange(exchanges);
            }
        }

        public async Task RegisterExchange(string exChange, string type)
        {
            if (exChange is not { Length: > 0 })
            {
                return;
            }

            if (_rabbitMqConnections is not { Length: > 0 })
            {
                return;
            }

            foreach (var rabbitMqConnection in _rabbitMqConnections)
            {
                logger.LogInformation("RegisterExchange {RabbitMqConnection} {ExChange} {Type}",
                    rabbitMqConnection.GetHosts(), exChange, type);
                await rabbitMqConnection.RegisterExchange(exChange, type);
            }
        }

        public async Task RegisterQueue(string exchange, (string Queue, string Routing)[] queues)
        {
            if (queues is not { Length: > 0 })
            {
                return;
            }

            if (_rabbitMqConnections is not { Length: > 0 })
            {
                return;
            }

            foreach (var rabbitMqConnection in _rabbitMqConnections)
            {
                await rabbitMqConnection.RegisterQueue(exchange, queues);
            }
        }

        public async Task SubscribeQueueAsync(string[] queues, Func<EventBusMessage, Task> processFunc)
        {
            if (queues is not { Length: > 0 })
            {
                return;
            }

            if (_rabbitMqConnections is not { Length: > 0 })
            {
                return;
            }

            foreach (var rabbitMqConnection in _rabbitMqConnections)
            {
                await rabbitMqConnection.SubscribeAsync(queues, processFunc);
            }
        }

        public async Task SubscribeExchangeAsync(string[] exchangesTrigger, Func<EventBusMessage, Task> processFunc)
        {
            if (exchangesTrigger is not { Length: > 0 })
            {
                return;
            }

            if (_rabbitMqConnections is not { Length: > 0 })
            {
                return;
            }

            foreach (var rabbitMqConnection in _rabbitMqConnections)
            {
                await rabbitMqConnection.SubscribeExchangeAsync(exchangesTrigger, processFunc);
            }
        }

        public async Task<(string, string)[]> Send((string, string) exchange, IEvent[] messages)
        {
            var connection = GetCurrentConnection();
            return await connection.Send(exchange, messages);
        }

        public async Task Notify(string exchange, IEvent message)
        {
            var connection = GetCurrentConnection();
            await connection.Notify(exchange, message);
        }

        public async Task NotifyTrigger(string exchange, IEvent[] messages)
        {
            var connection = GetCurrentConnection();
            await connection.NotifyTrigger(exchange, messages);
        }

        public async Task NotifyTrigger(string exchange, EventBusMessage message)
        {
            var connection = GetCurrentConnection();
            await connection.NotifyTrigger(exchange, message);
        }
    }
}
