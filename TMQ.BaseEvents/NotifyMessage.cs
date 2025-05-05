using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.BaseEvents
{
    [ProtoContract]
    public record NotifyMessage : Event
    {
        [ProtoMember(1)] public string[] Conditions { get; set; }
        [ProtoMember(3)] public NotifyMessageType Type { get; set; }
        [ProtoMember(4)] public string Title { get; set; }
        [ProtoMember(5)] public string Value { get; set; }
        [ProtoMember(6)] public string Name { get; set; }
        [ProtoMember(7)] public EnumDefine.NotificationPlatformEnum PlatformType { get; set; }
        [ProtoMember(8)] public TMQ.EnumDefine.NotifyActionTypeEnum ActionType { get; set; }
        [ProtoMember(9)] public string TitleSub { get; set; }
        [ProtoMember(10)] public string UserId { get; set; }

        [ProtoMember(11)] public override string EventId { get; set; }
        [ProtoMember(12)] public override int DelayTime { get; set; }
        [ProtoMember(13)] public override int Version { get; set; }
        [ProtoMember(14)] public override string ObjectId { get; set; }
        [ProtoMember(15)] public override string ProcessUid { get; set; }
        [ProtoMember(16)] public override DateTime ProcessDate { get; set; }
        [ProtoMember(17)] public override string LoginUid { get; set; }
        [ProtoMember(18)] public override bool IsTrigger { get; set; }
        public override EventTypeEnum EventType => EventTypeEnum.NotifyMessage;
    }
}
