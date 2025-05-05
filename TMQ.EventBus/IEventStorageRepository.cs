using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseEvents;

namespace TMQ.EventBus
{
    public interface IEventStorageRepository
    {
        Task Add(EventBusMessage message, EventStatusEnum status, string exception);
    }
}
