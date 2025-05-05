using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseEvents;

namespace TMQ.EventBus
{
    public interface IEventHandler<in TI> : IMessageHandler where TI : IEvent
    {
        Task Handle(TI message, string topic);
    }
}
