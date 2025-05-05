using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemCommands.Commands
{
    [ProtoContract]
    public record ShardGroupChangeCommand : ShardGroupAddCommand
    {
        [ProtoMember(1)] public new required string ObjectId { get; set; }
    }
}
