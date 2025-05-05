using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseEvents;
using TMQ.Common;
using TMQ.SystemCommands.Events;

namespace TMQ.SystemCommands
{
    [ProtoContract]
    [ProtoInclude(400, typeof(MenuChangeEvent))]
    public record SystemBaseEvent : Event
    {
        public SystemBaseEvent()
        {
            EventId = CommonUtility.GenerateGuid();
            DelayTime = 0;
            Version = 0;
            ProcessDate = Extension.GetCurrentDate();
        }

        public SystemBaseEvent(bool isTrigger)
        {
            EventId = CommonUtility.GenerateGuid();
            DelayTime = 0;
            Version = 0;
            ProcessDate = Extension.GetCurrentDate();
            IsTrigger = isTrigger;
        }

        [ProtoMember(1)] public override string EventId { get; set; }
        [ProtoMember(2)] public override int DelayTime { get; set; }
        [ProtoMember(3)] public override int Version { get; set; }
        public override EventTypeEnum EventType => EventTypeEnum.System;

        [ProtoMember(5)] public override string ObjectId { get; set; }
        [ProtoMember(6)] public override string ProcessUid { get; set; }
        [ProtoMember(7)] public override DateTime ProcessDate { get; set; }
        [ProtoMember(8)] public override string LoginUid { get; set; }
        [ProtoMember(9)] public override bool IsTrigger { get; set; }
    }
}
