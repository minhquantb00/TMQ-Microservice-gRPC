using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemCommands.Commands
{
    [ProtoContract]
    public record ZonePositionSyncToESCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public string[]? ZoneIds { get; set; }
        [ProtoMember(2)] public string? DealerId { get; set; }
    }
}
