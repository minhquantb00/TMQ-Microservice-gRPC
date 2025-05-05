using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.AccountReadModels
{
    [ProtoContract]
    public record RUserActivity : AccountBaseReadModel
    {
        [ProtoMember(1)] public string? UserId { get; set; }
        [ProtoMember(2)] public ActivityType ActivityType { get; set; }
        [ProtoMember(3)] public string? ActivityDetail { get; set; }
        [ProtoMember(4)] public DateTime ActivityTime { get; set; }
        [ProtoMember(5)] public ActivityStatus ActivityStatus { get; set; }
    }
}
