using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemReadModels
{

    [ProtoContract]
    public record RShardingSetting : SystemBaseReadModel
    {
        [ProtoMember(1)] public string? ESUrl { get; set; }
        [ProtoMember(2)] public string? ESIndexLastName { get; set; }
        [ProtoMember(3)] public string? DatabaseIp { get; set; }
        [ProtoMember(4)] public string? DatabaseName { get; set; }
        [ProtoMember(5)] public string? DatabaseUid { get; set; }
        [ProtoMember(6)] public string? DatabasePwd { get; set; }
    }
}
