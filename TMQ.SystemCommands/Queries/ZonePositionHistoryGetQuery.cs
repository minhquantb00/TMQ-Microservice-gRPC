using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemCommands.Queries
{
    [ProtoContract]
    public record ZonePositionHistoryGetQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string ZoneId { get; set; }
        [ProtoMember(2)] public int PageIndex { get; set; }
        [ProtoMember(3)] public int PageSize { get; set; }
        [ProtoMember(4)] public bool IsGetDetail { get; set; }
        [ProtoMember(5)] public string ChangeId { get; set; }
    }
}
