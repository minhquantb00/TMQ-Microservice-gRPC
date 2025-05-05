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
    public record ZonePositionChangeEvent : SystemBaseEvent
    {
        [ProtoMember(1)] public string? ZoneId { get; set; }
        public override EventTypeEnum EventType => EventTypeEnum.ZonePosition;
        [ProtoMember(2)] public ZonePositionItemChangeEvent[]? Items { get; set; }
    }

    [ProtoContract]
    public record ZonePositionPinChangeEvent : SystemBaseEvent
    {
        [ProtoMember(1)] public string? ZoneId { get; set; }
        public override EventTypeEnum EventType => EventTypeEnum.ZonePosition;
    }
}
