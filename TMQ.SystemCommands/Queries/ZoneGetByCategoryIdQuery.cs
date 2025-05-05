using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemCommands.Queries
{
    [ProtoContract]
    public record ZoneGetByCategoryIdQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string CategoryId { get; set; }
        [ProtoMember(2)] public bool AllowNull { get; set; }
        [ProtoMember(3)] public new string? DealerId { get; set; }
    }
}
