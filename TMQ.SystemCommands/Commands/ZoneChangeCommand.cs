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
    public record ZoneChangeCommand : ZoneAddCommand
    {
        [ProtoMember(1)] public string? Id { get; set; }
    }

    [ProtoContract]
    public record ZoneChangeStatusCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public string? Id { get; set; }
        [ProtoMember(2)] public StatusEnum Status { get; set; }
    }
}
