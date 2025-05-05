using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.SystemReadModels
{
    [ProtoContract]
    public record RZonePositionHistory : SystemBaseReadModel
    {
        [ProtoMember(1)] public int Position { get; set; }
        [ProtoMember(2)] public ZonePositionDisplayTypeEnum DisplayType { get; set; }
        [ProtoMember(3)] public string ZoneId { get; set; }
        [ProtoMember(4)] public RZonePosition? ZonePositionOld { get; set; }
        [ProtoMember(5)] public RZonePosition? ZonePositionNew { get; set; }
        [ProtoMember(6)] public string ChangeId { get; set; }
        [ProtoMember(7)] public string ObjectIdOld { get; set; }
        [ProtoMember(8)] public string ObjectIdNew { get; set; }
        [ProtoMember(9)] public ZonePositionActionTypeEnum ActionType { get; set; }
        [ProtoMember(10)] public bool IsAuto { get; set; }
        [ProtoMember(11)] public ZonePositionType TypeOld { get; set; }
        [ProtoMember(12)] public ZonePositionType TypeNew { get; set; }
        [ProtoMember(13)] public bool? IsPin { get; set; }
    }
}
