using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.SystemCommands.Events
{
    [ProtoContract]
    public class ZonePositionItemChangeEvent
    {
        [ProtoMember(1)] public ZonePositionType Type { get; set; }
        [ProtoMember(2)] public string ObjectId { get; set; }
        [ProtoMember(3)] public string ZoneId { get; set; }
        [ProtoMember(4)] public ActiveStatusEnum Status { get; set; }
        [ProtoMember(5)] public int Position { get; set; }
        [ProtoMember(6)] public int Priority { get; set; }
        [ProtoMember(7)] public DateTime? BeginDate { get; set; }
        [ProtoMember(8)] public DateTime? EndDate { get; set; }
        // [ProtoMember(9)] public bool IsPin { get; set; }
        [ProtoMember(10)] public long Order { get; set; }
        [ProtoMember(11)] public string Id { get; set; }
    }
}
