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
    public record RLocaleStringResource : SystemBaseReadModel
    {
        [ProtoMember(1)] public string LanguageId { get; set; }

        [ProtoMember(2)] public string ResourceName { get; set; }

        [ProtoMember(3)] public string ResourceValue { get; set; }

        [ProtoMember(4)] public StatusEnum Status { get; set; }

        [ProtoMember(5)] public int DisplayOrder { get; set; }
    }
}
