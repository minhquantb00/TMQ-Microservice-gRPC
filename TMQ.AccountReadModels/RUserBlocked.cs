using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.AccountReadModels
{
    [ProtoContract]
    public record RUserBlocked : AccountBaseReadModel
    {
        [ProtoMember(1)] public string? UserId { get; set; }
        [ProtoMember(2)] public string? BlockedUserId { get; set; }
        [ProtoMember(3)] public bool IsBlocked { get; set; }
    }
}
