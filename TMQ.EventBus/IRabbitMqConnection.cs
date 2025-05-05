using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseEvents;

namespace TMQ.EventBus
{
    public interface IRabbitMqConnection
    {
        Task Notify(string exchange, IEvent message);
        Task<(string, string)[]> Send((string, string) exchange, IEvent[] messages);
        Task<string> Send((string, string) topic, byte[] message);
        Task Send((string, string) topic, byte[][] messages);
        Task NotifyTrigger(string exchange, IEvent[] messages);
        Task NotifyTrigger(string exchange, EventBusMessage message);
        Task SubscribeAsync(string[] topics, Func<EventBusMessage, Task> processFunc);
        Task SubscribeNotifyAsync(string[] exchangesTrigger, Func<EventBusMessage, Task> processFunc);
        Task SubscribeExchangeAsync(string[] exchangesTrigger, Func<EventBusMessage, Task> processFunc);
        Task RegisterExchangeAndQueue(string exchange, string[] routingKeys, string[] queues);
        Task RegisterExchangeTrigger(string[] exchanges);
        Task<(IChannel? Channel, string? ConsumerTag)> SubscribeAsync(string queue, Func<byte[], Task> processFunc,
            int? rabbitMqPrefetchCount);
        Task RegisterExchange((string ExChange, string Type)[] exchanges);
        Task RegisterExchange(string exChange, string type);
        Task RegisterQueue(string exchange, (string Queue, string Routing)[] queues);
        string GetHosts();
        string Hosts { get; }
    }
}
