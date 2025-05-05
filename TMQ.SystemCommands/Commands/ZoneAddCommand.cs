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
    [ProtoInclude(3050, typeof(ZoneChangeCommand))]
    public record ZoneAddCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public string Name { get; set; }
        [ProtoMember(2)] public string Description { get; set; }
        [ProtoMember(3)] public StatusEnum Status { get; set; }
        [ProtoMember(4)] public int Priority { get; set; }
        [ProtoMember(5)] public string DealerId { get; set; }
        [ProtoMember(6)] public int TotalPosition { get; set; }
        [ProtoMember(7)] public string CategoryId { get; set; }
        [ProtoMember(8)] public string Thumbnail { get; set; }

        [ProtoMember(9)] public string Code { get; set; }

        /*[ProtoMember(10)] public new string ObjectId { get; set; }*/
        [ProtoMember(11)] public ZoneObjectTypeEnum ObjectType { get; set; }
        [ProtoMember(12)] public string MappingKey { get; set; }
        [ProtoMember(13)] public string GroupId { get; set; }
        [ProtoMember(16)] public MobilePageEnum MobilePage { get; set; }
        [ProtoMember(17)] public long AutoSetPositionTime { get; set; }
        [ProtoMember(18)] public int PRExpiredPosition { get; set; }
        [ProtoMember(19)] public ZoneOptionEnum? Options { get; set; }
    }
}
