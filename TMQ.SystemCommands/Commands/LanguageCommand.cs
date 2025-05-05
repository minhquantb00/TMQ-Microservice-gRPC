using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.SystemCommands.Commands
{
    [ProtoContract]
    [ProtoInclude(200, typeof(LanguageChangeCommand))]
    public record LanguageAddCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public string Id { get; set; }

        [ProtoMember(2)] public string Name { get; set; }
        [ProtoMember(3)] public string Culture { get; set; }
        [ProtoMember(4)] public string UniqueSeoCode { get; set; }
        [ProtoMember(5)] public string FlagImageFileName { get; set; }
        [ProtoMember(6)] public byte Rtl { get; set; }
        [ProtoMember(7)] public byte LimitedToStores { get; set; }
        [ProtoMember(8)] public string DefaultCurrencyId { get; set; }
        [ProtoMember(10)] public int DisplayOrder { get; set; }

        [ProtoMember(11)] public StatusEnum Status { get; set; }
        [ProtoMember(12)] public LanguageTypeEnum Type { get; set; }
        [ProtoMember(13)] public string NumberFormat { get; set; }
        [ProtoMember(14)] public string DateFormat { get; set; }
    }

    [ProtoContract]
    public record LanguageChangeCommand : LanguageAddCommand
    {
    }
}
