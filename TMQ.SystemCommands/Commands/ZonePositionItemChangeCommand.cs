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
    public record ZonePositionItemChangeCommand : SystemBaseCommand
    {
        [ProtoMember(2)] public int Position { get; set; }
        [ProtoMember(3)] public ZonePositionType Type { get; set; }
        [ProtoMember(5)] public bool Status { get; set; }
        [ProtoMember(6)] public int Priority { get; set; }
        [ProtoMember(7)] public DateTime? BeginDateTime { get; set; }
        [ProtoMember(8)] public DateTime? EndDateTime { get; set; }

        [ProtoMember(9)] public string Id { get; set; }
    }

    [ProtoContract]
    public record ZonePositionChangeFromNewsCommand : SystemBaseCommand
    {
        /*[ProtoMember(1)] public string ObjectId { get; set; }*/

        // [ProtoMember(2)] public bool IsPR { get; set; }
        [ProtoMember(3)] public ZonePositionItemChangeFromNewsCommand[] Items { get; set; }
        [ProtoMember(4)] public string[]? ZonePositionIdsRemove { get; set; }
    }

    [ProtoContract]
    public record ZonePositionItemChangeFromNewsCommand
    {
        [ProtoMember(1)] public string ZoneId { get; set; }
        [ProtoMember(2)] public int Position { get; set; }
        [ProtoMember(3)] public DateTime? BeginDateTime { get; set; }
        [ProtoMember(4)] public DateTime? EndDateTime { get; set; }
        [ProtoMember(5)] public ZonePositionDisplayTypeEnum DisplayType { get; set; }
        [ProtoMember(6)] public string ZonePositionId { get; set; }
        [ProtoMember(7)] public bool Status { get; set; }
        [ProtoMember(8)] public long LastOrder { get; set; }
        [ProtoMember(9)] public bool IsReadOnly { get; set; }
    }

    [ProtoContract]
    public record ZonePositionRemoveFromNewsCommand : SystemBaseCommand
    {
        /*[ProtoMember(1)] public string ObjectId { get; set; }*/
    }
}
