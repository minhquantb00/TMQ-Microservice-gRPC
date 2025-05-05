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
    [ProtoInclude(1750, typeof(ShardGroupChangeCommand))]
    public record ShardGroupAddCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public string? Name { get; set; }
        [ProtoMember(2)] public ShardingTypeEnum Type { get; set; }
        [ProtoMember(3)] public int TotalShard { get; set; }
        [ProtoMember(4)] public StatusEnum Status { get; set; }
    }
}
