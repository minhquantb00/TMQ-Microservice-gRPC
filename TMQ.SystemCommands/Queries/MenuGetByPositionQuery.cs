using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.SystemCommands.Queries
{
    [ProtoContract]
    public record MenuGetByPositionQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public MenuPosition Position { get; set; }
    }

    [ProtoContract]
    public record MenuGetByPositionsQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public MenuPosition[]? Positions { get; set; }
    }
}
