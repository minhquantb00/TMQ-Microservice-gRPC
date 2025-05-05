using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.AccountReadModels
{
    [ProtoContract]
    public record RUserRelationship : AccountBaseReadModel
    {
        [ProtoMember(1)] public string? UserId { get; set; }
        [ProtoMember(2)] public string? RelatedUserId { get; set; }
        [ProtoMember(3)] public RelationshipEnum Relationship { get; set; }
        [ProtoMember(4)] public bool IsSetRelationship { get; set; }
    }
}
