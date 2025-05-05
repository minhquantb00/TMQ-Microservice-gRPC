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
    [ProtoInclude(1850, typeof(ShardChangeCommand))]
    public record ShardAddCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public string? Name { get; set; }
        [ProtoMember(2)] public string? ShardingGroupId { get; set; }
        [ProtoMember(3)] public StatusEnum Status { get; set; }

        [ProtoMember(4)] public string? ESUrl { get; set; }
        [ProtoMember(5)] public string? ESIndexLastName { get; set; }
        [ProtoMember(6)] public string? DatabaseIp { get; set; }
        [ProtoMember(7)] public string? DatabaseName { get; set; }
        [ProtoMember(8)] public string? DatabaseUid { get; set; }
        [ProtoMember(9)] public string? DatabasePwd { get; set; }
    }
}
