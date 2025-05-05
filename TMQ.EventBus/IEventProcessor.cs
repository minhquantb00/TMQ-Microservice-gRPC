using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseEvents;

namespace TMQ.EventBus
{
    public interface IEventProcessor
    {
        void Register();
        Dictionary<string, string> Handle(EventBusMessage payload);
        Task Start();
    }
}
