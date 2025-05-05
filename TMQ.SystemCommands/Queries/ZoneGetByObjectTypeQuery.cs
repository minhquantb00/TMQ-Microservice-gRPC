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
    public record ZoneGetByObjectTypeQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public ZoneObjectTypeEnum Type { get; set; }
    }
}
