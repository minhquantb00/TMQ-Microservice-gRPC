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
    public record RShardGroup : SystemBaseReadModel
    {
        [ProtoMember(1)] public new required string Id { get; set; }
        [ProtoMember(2)] public string? Name { get; set; }
        [ProtoMember(3)] public ShardingTypeEnum Type { get; set; }
        [ProtoMember(4)] public int TotalShard { get; set; }
        [ProtoMember(5)] public StatusEnum Status { get; set; }
        [ProtoMember(6)] public List<RShard>? Items { get; set; }
    }
}
