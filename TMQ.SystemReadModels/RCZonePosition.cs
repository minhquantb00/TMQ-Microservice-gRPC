using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemReadModels
{
    [ProtoContract]
    public class RCZonePosition
    {
        [ProtoMember(1)] public string? ZoneId { get; set; }
        [ProtoMember(2)] public RZonePosition[]? Items { get; set; }
    }
}
