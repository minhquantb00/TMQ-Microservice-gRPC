using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.BaseEvents
{
    public interface IEvent
    {
        string EventId { get; set; }
        int Version { get; set; }
        EventTypeEnum EventType { get; }
        public bool IsTrigger { get; set; }
        public SerializeTypeEnum SerializeType { get; set; }
        public string? Publisher { get; set; }
    }
}
