using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.AccountReadModels
{
    [ProtoContract]
    public record RRefreshToken : AccountBaseReadModel
    {
        [ProtoMember(1)] public string? Token { get; set; }
        [ProtoMember(2)] public DateTime ExpiredTime { get; set; }
        [ProtoMember(3)] public string? UserId { get; set; }
    }
}
