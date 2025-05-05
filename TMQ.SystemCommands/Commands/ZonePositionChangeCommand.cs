using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.SystemCommands.Commands
{
    [ProtoContract]
    public record ZonePositionChangeCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public string? ZoneId { get; set; }
        [ProtoMember(2)] public ZonePositionItemChangeCommand[]? Items { get; set; }
        [ProtoMember(3)] public bool ByPassCheckLock { get; set; }
        [ProtoMember(4)] public ZonePositionDisplayTypeEnum DisplayType { get; set; }
        [ProtoMember(5)] public long LastOrder { get; set; }
        [ProtoMember(6)] public bool? IsSave { get; set; }
    }

    [ProtoContract]
    public record ZonePositionRemoveCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public required string ZoneId { get; set; }
        [ProtoMember(2)] public required string[] ZonePositionIds { get; set; }
    }
}
