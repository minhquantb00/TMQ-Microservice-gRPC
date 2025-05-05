using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.SystemReadModels
{
    [ProtoContract]
    public record RMenu : SystemBaseReadModel
    {
        [ProtoMember(1)] public string ParentId { get; set; }
        [ProtoMember(2)] public string Name { get; set; }
        [ProtoMember(3)] public MenuTypeEnum Type { get; set; }
        [ProtoMember(4)] public string Url { get; set; }
        [ProtoMember(5)] public string? ActionDefineId { get; set; }
        [ProtoMember(6)] public MenuPosition PositionId { get; set; }
        [ProtoMember(7)] public int Priority { get; set; }
        [ProtoMember(8)] public StatusEnum Status { get; set; }
        [ProtoMember(9)] public string? ObjectId { get; set; }
        [ProtoMember(10)] public string Condition { get; set; }
        [ProtoMember(11)] public string DealerId { get; set; }
        [ProtoMember(12)] public string CssClassIcon { get; set; }
        [ProtoMember(13)] public RMenu[] Children { get; set; }
        [ProtoMember(14)] public bool IsDisplayPermission { get; set; }
        [ProtoMember(15)] public SystemOptionEnum SystemOption { get; set; }
        [ProtoMember(16)] public string PathBase { get; set; }
        [ProtoMember(17)] public string BaseUrl { get; set; }
        [ProtoMember(18)] public DateTime? StartDate { get; set; }
        [ProtoMember(19)] public DateTime? EndDate { get; set; }
        public bool HasChild => Children?.Length > 0;
        public int CountChild => Children?.Length ?? 0;
    }
}
