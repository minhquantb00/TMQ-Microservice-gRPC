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
    public record RShard : SystemBaseReadModel
    {
        [ProtoMember(1)] public new required string Id { get; set; }
        [ProtoMember(2)] public string? Name { get; set; }
        [ProtoMember(3)] public string? ShardingGroupId { get; set; }
        [ProtoMember(4)] public string? SettingJson { get; set; }
        public RShardingSetting? Setting => Common.Serialize.JsonDeserializeObject<RShardingSetting>(SettingJson);
        [ProtoMember(5)] public StatusEnum Status { get; set; }
    }
}
