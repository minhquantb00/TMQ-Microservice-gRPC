using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseEvents;

namespace TMQ.SystemCommands.Events
{
    [ProtoContract]
    public record ZoneChangeEvent : SystemBaseEvent
    {
        [ProtoMember(1)] public string Id { get; set; }
        public override EventTypeEnum EventType => EventTypeEnum.System;
    }
}
