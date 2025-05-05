using ProtoBuf;
using TMQ.EnumDefine;

namespace TMQ.BaseEvents
{
    [ProtoContract]
    public record CacheChangeEvent : Event
    {
        [ProtoMember(1)] public string? Id { get; set; }
        [ProtoMember(2)] public KeyCacheTypeEnum CacheType { get; set; }
        public override EventTypeEnum EventType => EventTypeEnum.Cache;

        [ProtoMember(3)] public override string EventId { get; set; }
        [ProtoMember(4)] public override int DelayTime { get; set; }
        [ProtoMember(5)] public override int Version { get; set; }
        [ProtoMember(7)] public override bool IsTrigger { get; set; }
        [ProtoMember(8)] public override string ObjectId { get; set; }
        [ProtoMember(9)] public override string ProcessUid { get; set; }
        [ProtoMember(10)] public override DateTime ProcessDate { get; set; }
        [ProtoMember(11)] public override string LoginUid { get; set; }
        [ProtoMember(12)] public string DealerId { get; set; }
        [ProtoMember(13)] public bool ComponentRelatedCacheClear { get; set; }
    }
}
