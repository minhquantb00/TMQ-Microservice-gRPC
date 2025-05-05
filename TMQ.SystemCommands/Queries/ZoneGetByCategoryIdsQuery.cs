using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemCommands.Queries
{
    [ProtoContract]
    public record ZoneGetByCategoryIdsQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string[]? CategoryIds { get; set; }
    }
}
