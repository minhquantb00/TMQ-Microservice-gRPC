using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.AccountCommands.Events;
using TMQ.BaseEvents;

namespace TMQ.AccountCommands
{
    [ProtoContract]
    [ProtoInclude(200, typeof(UserDeviceChangeFcmTokenEvent))]
    [ProtoInclude(300, typeof(UserAddEvent))]
    public record AccountBaseEvent : Event
    {
        [ProtoMember(1)] public sealed override string EventId { get; set; } = Guid.CreateVersion7().ToString("N");
        [ProtoMember(2)] public override int Version { get; set; }
        [ProtoMember(3)] public override bool IsTrigger { get; set; }
        [ProtoMember(4)] public override string? ObjectId { get; set; }
        [ProtoMember(5)] public override string? ProcessUid { get; set; }
        [ProtoMember(6)] public override DateTime ProcessDate { get; set; }
        [ProtoMember(7)] public override string? LoginUid { get; set; }
        [ProtoMember(8)] public override int DelayTime { get; set; }
        public override EventTypeEnum EventType => EventTypeEnum.Account;
    }
}
