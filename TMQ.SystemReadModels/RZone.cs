using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.Common;
using TMQ.EnumDefine;

namespace TMQ.SystemReadModels
{
    [ProtoContract]
    public record RZone : SystemBaseReadModel
    {
        [ProtoMember(1)] public string? Name { get; set; }
        [ProtoMember(2)] public string Description { get; set; }
        [ProtoMember(3)] public StatusEnum Status { get; set; }
        [ProtoMember(4)] public int Priority { get; set; }
        [ProtoMember(5)] public string DealerId { get; set; }
        [ProtoMember(6)] public int TotalPosition { get; set; }
        [ProtoMember(7)] public string LockUserId { get; set; }
        [ProtoMember(13)] public string LockSocketId { get; set; }
        [ProtoMember(8)] public string? CategoryId { get; set; }
        [ProtoMember(9)] public string Thumbnail { get; set; }
        [ProtoMember(10)] public string ObjectId { get; set; }
        [ProtoMember(11)] public ZoneObjectTypeEnum ObjectType { get; set; }
        [ProtoMember(12)] public string MappingKey { get; set; }
        [ProtoMember(14)] public string? GroupId { get; set; }
        [ProtoMember(15)] public long LastOrder { get; set; }
        [ProtoMember(16)] public MobilePageEnum MobilePage { get; set; }
        [ProtoMember(17)] public long AutoSetPositionTime { get; set; }
        [ProtoMember(18)] public int PRExpiredPosition { get; set; }
        [ProtoMember(19)] public ZoneOptionEnum Options { get; set; }
        private string _name;

        public string NameNonUnicode
        {
            get
            {
                if (string.IsNullOrEmpty(_name))
                {
                    _name = UnicodeUtility.UnicodeToKoDau(Name).AsEmpty().ToLower();
                }

                return _name;
            }
        }
    }
}
