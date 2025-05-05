using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.BaseEvents
{
    public abstract record Event : IEvent
    {
        protected Event()
        {
            EventId = Guid.CreateVersion7().ToString("N");
            DelayTime = 0;
            Version = 0;
            ProcessDate = DateTime.Now;
        }

        public abstract string EventId { get; set; }
        public abstract int DelayTime { get; set; }
        public abstract int Version { get; set; }
        public abstract EventTypeEnum EventType { get; }
        public abstract bool IsTrigger { get; set; }
        public abstract string? ObjectId { get; set; }
        public abstract string? ProcessUid { get; set; }
        public abstract DateTime ProcessDate { get; set; }
        public DateTime ProcessDateUtc => ProcessDate.ToUniversalTime();
        public abstract string? LoginUid { get; set; }
        public SerializeTypeEnum SerializeType { get; set; }
        public string? Publisher { get; set; }

        public enum StatusEnum
        {
            New = 0,
            Success = 1,
            Fail = -1,
            Retry = 2
        }
    }
}
