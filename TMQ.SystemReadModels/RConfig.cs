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
    public record RConfig : SystemBaseReadModel
    {
        [ProtoMember(1)] public string Key { get; set; }
        [ProtoMember(2)] public string Description { get; set; }
        [ProtoMember(3)] public string Value { get; set; }
        [ProtoMember(4)] public StatusEnum Status { get; set; }
        [ProtoMember(5)] public ConfigTypeEnum Type { get; set; }
    }
}
