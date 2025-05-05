using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.Common;
using TMQ.Config;

namespace TMQ.EventBus
{
    public class RabbitMqConnection(
    IRabbitMqPersistentConnection persistentConnection,
    ILogger<RabbitMqConnection> logger)
    : RabbitMqBaseConnection(persistentConnection, logger, RabbitMqPrefetchCount), IRabbitMqConnection
    {
        private static readonly int RabbitMqPrefetchCount =
            ConfigSettingEnum.RabbitMqPrefetchCount.GetConfig().AsInt(10);
    }
}
