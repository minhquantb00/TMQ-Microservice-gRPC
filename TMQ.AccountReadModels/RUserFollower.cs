using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.AccountReadModels
{
    [ProtoContract]
    public record RUserFollower : AccountBaseReadModel
    {
        [ProtoMember(1)] public string? UserFollowId { get; set; }
        [ProtoMember(2)] public string? UserFollowingId { get; set; }
        [ProtoMember(3)] public bool IsFollowed { get; set; }
    }
}
