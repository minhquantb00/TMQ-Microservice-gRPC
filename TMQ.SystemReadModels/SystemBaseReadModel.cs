using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseReadModels;

namespace TMQ.SystemReadModels
{
    [ProtoContract]
    [ProtoInclude(100, typeof(RConfig))]
    [ProtoInclude(200, typeof(RLanguage))]
    [ProtoInclude(300, typeof(RLocaleStringResource))]
    [ProtoInclude(400, typeof(RMenu))]
    public record SystemBaseReadModel : BaseReadModel
    {
        [ProtoMember(1)] public override long NumericalOrder { get; set; }
        [ProtoMember(2)] public override string Id { get; set; }
        [ProtoMember(3)] public override string Code { get; set; }
        [ProtoMember(4)] public override string CreatedUid { get; set; }
        [ProtoMember(5)] public override DateTime CreatedDate { get; set; }
        [ProtoMember(6)] public override DateTime CreatedDateUtc { get; set; }
        [ProtoMember(7)] public override string UpdatedUid { get; set; }
        [ProtoMember(8)] public override DateTime UpdatedDate { get; set; }
        [ProtoMember(9)] public override DateTime UpdatedDateUtc { get; set; }
        [ProtoMember(10)] public override int Version { get; set; }
        [ProtoMember(11)] public override string LoginUid { get; set; }
        [ProtoMember(12)] public override int TotalRow { get; set; }
        [ProtoMember(13)] public string? DealerId { get; set; }
        [ProtoMember(14)] public override string? ShardId { get; set; }
    }
}
