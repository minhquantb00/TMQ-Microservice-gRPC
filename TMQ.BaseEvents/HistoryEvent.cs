using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.BaseEvents
{
    [ProtoContract]
    public record HistoryEvent : Event
    {
        [ProtoMember(1)] public override string EventId { get; set; }
        [ProtoMember(2)] public override int DelayTime { get; set; }
        [ProtoMember(3)] public override int Version { get; set; }
        public override EventTypeEnum EventType => EventTypeEnum.History;
        [ProtoMember(5)] public override bool IsTrigger { get; set; }
        [ProtoMember(6)] public override string ObjectId { get; set; }
        [ProtoMember(7)] public override string ProcessUid { get; set; }
        [ProtoMember(8)] public override DateTime ProcessDate { get; set; }
        [ProtoMember(9)] public override string LoginUid { get; set; }

        [ProtoMember(10)] public string? Domain { get; set; }
        [ProtoMember(11)] public string? Command { get; set; }
        [ProtoMember(12)] public string? Items { get; set; }
        [ProtoMember(13)] public string? DomainType { get; set; }
        [ProtoMember(14)] public string? CommandType { get; set; }
        [ProtoMember(15)] public bool IsAddNew { get; set; }
        [ProtoMember(16)] public required string Type { get; set; }
        [ProtoMember(17)] public HistoryDomainChangeEvent[]? DomainsChange { get; set; }
        [ProtoMember(18)] public string? DealerId { get; set; }
        [ProtoMember(19)] public string? OldStatus { get; set; }
        [ProtoMember(20)] public string? CommandData { get; set; }
        [ProtoMember(21)] public string? NewsCreatedUid { get; set; }
    }

    [ProtoContract]
    public record HistoryDomainChangeEvent
    {
        [ProtoMember(1)] public int Priority { get; set; }
        [ProtoMember(2)] public required string Domain { get; set; }
        [ProtoMember(3)] public bool IsSystem { get; set; }
    }
}
