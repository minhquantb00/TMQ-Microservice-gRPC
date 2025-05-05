using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseEvents;
using TMQ.BaseEvents.Extensions;
using TMQ.Common;
using TMQ.Config;

namespace TMQ.EventBus
{
    public class EventProcessor(
    ILogger<EventProcessor> logger,
    IEventStorageRepository eventStorageDao,
    IServiceProvider serviceProvider,
    RabbitMqConnectionPool rabbitMqConnection)
    : IEventProcessor
    {
        private readonly ConcurrentDictionary<Type, HashSet<IMessageHandler>> _handlers = [];
        private readonly ConcurrentDictionary<string, HashSet<string>> _eventHandlerFilterQueues = [];
        private readonly string _exChange = ConfigSettingEnum.RabbitMqExChange.GetConfig();
        private readonly string _rabbitMqExChangeNotify = ConfigSettingEnum.RabbitMqExChangeNotify.GetConfig();

        private readonly string _eventHandlerFilterQueuesConfig =
            ConfigSettingEnum.EventHandlerFilterQueues.GetConfig().ToLower();

        private readonly string[] _routingKeys =
            ConfigSettingEnum.RabbitMqRouting.GetConfig().Split(',', StringSplitOptions.RemoveEmptyEntries);

        readonly string[] _topics =
            ConfigSettingEnum.RabbitMqQueues.GetConfig().Split(',', StringSplitOptions.RemoveEmptyEntries);

        //string exChangeNotify = ConfigSettingEnum.RabbitMqExChangeNotifyListen.GetConfig();
        readonly string[] _exChangesTrigger = ConfigSettingEnum.RabbitMqExChangeTriggerListen.GetConfig()
            .Split(',', StringSplitOptions.RemoveEmptyEntries);

        private readonly string[] _workerGroup =
            ConfigSettingEnum.WorkerGroup.GetConfig().ToLower()
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);


        public void Register()
        {
            if (_eventHandlerFilterQueuesConfig.Length > 0)
            {
                var configs = _eventHandlerFilterQueuesConfig.Split(",",
                    StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (configs.Length > 0)
                {
                    foreach (var config in configs)
                    {
                        var configsByQueue = config.Split("|",
                            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        if (configsByQueue.Length > 1)
                        {
                            if (!_eventHandlerFilterQueues.ContainsKey(configsByQueue[0]))
                            {
                                _eventHandlerFilterQueues.TryAdd(configsByQueue[0], []);
                            }

                            for (int i = 1; i < configsByQueue.Length; i++)
                            {
                                _eventHandlerFilterQueues[configsByQueue[0]].Add(configsByQueue[i]);
                            }
                        }
                    }
                }
            }

            var genericHandler = typeof(IEventHandler<>);
            var services = serviceProvider.GetServices<IMessageHandler>();
            foreach (var service in services)
            {
                if (_workerGroup.Length > 0)
                {
                    if (!_workerGroup.Contains(service.WorkerGroup.ToLower()))
                    {
                        continue;
                    }
                }

                //if (service.GetType().BaseType == typeof(BaseEventHandler))
                //{
                //    continue;
                //}
                var supportedCommandTypes = service.GetType()
                    .GetInterfaces()
                    .Where(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == genericHandler)
                    .Select(iface => iface.GetGenericArguments()[0])
                    .ToArray();
                if (supportedCommandTypes.Length > 0)
                {
                    foreach (var commandType in supportedCommandTypes)
                    {
                        if (!_handlers.ContainsKey(commandType))
                        {
                            bool result = _handlers.TryAdd(commandType, new HashSet<IMessageHandler>());
                        }

                        bool result1 = _handlers[commandType].Add(service);
                    }
                }
                else
                {
                    var baseTypes = service.GetType().GetInterfaces();
                    foreach (var baseType in baseTypes)
                    {
                        if (baseType == typeof(IMessageHandler))
                        {
                            if (!_handlers.ContainsKey(typeof(string)))
                            {
                                bool result = _handlers.TryAdd(typeof(string), new HashSet<IMessageHandler>());
                            }

                            bool result1 = _handlers[typeof(string)].Add(service);
                        }
                    }
                }
            }
        }

        public async Task Start()
        {
            await rabbitMqConnection.RegisterExchange(_exChange, ExchangeType.Topic);
            await rabbitMqConnection.RegisterExchange(_rabbitMqExChangeNotify, ExchangeType.Fanout);
            (string Queue, string Routing)[] queues = new (string Queue, string Routing)[_topics.Length];
            int i = 0;
            foreach (var topic in _topics)
            {
                queues[i] = (topic, _routingKeys[i]);
                i++;
            }

            await rabbitMqConnection.RegisterQueue(_exChange, queues);
            if (_exChangesTrigger.Length > 0)
            {
                (string ExChange, string Type)[] exchanges = new (string ExChange, string Type)[_exChangesTrigger.Length];
                i = 0;
                foreach (var exChangeTrigger in _exChangesTrigger)
                {
                    exchanges[i] = (exChangeTrigger, ExchangeType.Fanout);
                    i++;
                }

                await rabbitMqConnection.RegisterExchange(exchanges);
            }

            await rabbitMqConnection.SubscribeQueueAsync(_topics, ProcessMessage);
            await rabbitMqConnection.SubscribeExchangeAsync(_exChangesTrigger, ProcessMessage);
        }

        private async Task ProcessMessage(EventBusMessage message)
        {
            var messageProcess = message;
            var processDate = Extension.GetCurrentDate();
            messageProcess.ProcessDate = processDate;
            messageProcess.SendTime = messageProcess.ProcessDate?.Subtract(messageProcess.CreatedDate)
                .TotalMilliseconds.DoubleAsLong();
            try
            {
                logger.LogInformation("ProcessMessage: {MessageProcessBodyType}", messageProcess.BodyType);
                EventStatusEnum status = EventStatusEnum.New;
                string error = string.Empty;
                var result = new Dictionary<string, string>();
                try
                {
                    result = Handle(messageProcess);
                    status = EventStatusEnum.Success;
                }
                catch (Exception e)
                {
                    status = EventStatusEnum.Fail;
                    error = $"Exception:{e.Message}:{e.StackTrace}";
                    logger.LogError(e, "{Error}", error);
                }
                finally
                {
                    if (!messageProcess.BodyType?.StartsWith("TYT.BaseApplication.Models.LogActionModel") == true)
                    {
                        try
                        {
                            messageProcess.FinishDate = Extension.GetCurrentDate();
                            messageProcess.ExecuteTime = messageProcess.FinishDate?.Subtract(processDate)
                                .TotalMilliseconds.DoubleAsLong();
                            messageProcess.Status = status;
                            messageProcess.Error = $"{error} - {result.Values.ToArray().AsArrayJoin()}";
                            messageProcess.Consumer = $"{Environment.MachineName} - {result.Keys.ToArray().AsArrayJoin()}";
                            await eventStorageDao.Add(messageProcess, status, error);
                        }
                        catch (Exception e)
                        {
                            logger.LogError(e, "ProcessMessage finally {Message}", e.Message);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                logger.LogError(e, "ProcessMessage Exception {Message}", e.Message);
                throw;
            }
        }

        public Dictionary<string, string> Handle(EventBusMessage message)
        {
            var result = new Dictionary<string, string>();
            var commandType = Type.GetType(message.BodyType.AsEmpty());
            if (commandType != null && _handlers.TryGetValue(commandType, out var handlers))
            {
                using var scope = serviceProvider.CreateScope();
                if (handlers.Count <= 0) return result;
                List<Task> tasks = [];
                foreach (var handler in handlers)
                {
                    var handlerType = handler.GetType();
                    var consumer = handlerType.FullName;
                    string handlerName = handlerType.Name.ToLower();
                    if (_eventHandlerFilterQueues.TryGetValue(handlerName, out var queues) && queues.Count > 0)
                    {
                        if (!queues.Contains(message.TopicName.ToLower()))
                        {
                            continue;
                        }
                    }

                    var error = string.Empty;
                    try
                    {
                        var service = scope.ServiceProvider.GetRequiredService(handlerType);
                        if (service == null)
                        {
                            throw new Exception("Handler not register");
                        }

                        logger.LogInformation("Handle BodyType:{MessageBodyType}", message.BodyType);
                        var task = ((dynamic)service).Handle(
                            (dynamic)message.EventBusMessageToObj()!, message.TopicName);
                        tasks.Add(task);
                    }
                    catch (Exception e)
                    {
                        error = $"Exception:{e.Message}:{e.StackTrace}";
                        logger.LogError(e, "{Message}", e.Message);
                    }

                    result.Add(consumer.AsEmpty(), error);
                }

                if (tasks.Count > 0)
                {
                    Task.WaitAll(tasks.ToArray());
                }
            }
            else
            {
                var logMessage = $"No Handler for {message.BodyType}";
                logger.LogInformation("{LogMessage}", logMessage);
                result.Add("No Handler", logMessage);
            }

            return result;
        }
    }
}
