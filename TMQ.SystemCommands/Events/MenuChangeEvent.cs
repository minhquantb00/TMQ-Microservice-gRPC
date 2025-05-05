using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseEvents;
using TMQ.EnumDefine;

namespace TMQ.SystemCommands.Events
{
    [ProtoContract]
    public record MenuChangeEvent : SystemBaseEvent
    {
        [ProtoMember(1)] public string? Id { get; set; }
        public override EventTypeEnum EventType => EventTypeEnum.System;
        public override bool IsTrigger => false;
        [ProtoMember(2)] public MenuPosition PositionId { get; set; }
    }
}
