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
    public record RLanguage : SystemBaseReadModel
    {
        [ProtoMember(1)] public new string Id { get; set; }
        [ProtoMember(2)] public string Name { get; set; }
        [ProtoMember(3)] public string Culture { get; set; }
        [ProtoMember(4)] public string UniqueSeoCode { get; set; }
        [ProtoMember(5)] public string FlagImageFileName { get; set; }
        [ProtoMember(6)] public byte Rtl { get; set; }
        [ProtoMember(7)] public byte LimitedToStores { get; set; }
        [ProtoMember(8)] public string DefaultCurrencyId { get; set; }
        [ProtoMember(9)] public StatusEnum Status { get; set; }
        [ProtoMember(10)] public int DisplayOrder { get; set; }
        [ProtoMember(12)] public LanguageTypeEnum Type { get; set; }

        private string _dateFormat;

        [ProtoMember(14)]
        public string DateFormat
        {
            get => _dateFormat;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    _dateFormat = UniqueSeoCode.AsEmpty() == "vn" ? "dd/MM/yyyy" : "MM/dd/yyyy";
                }
                else
                {
                    _dateFormat = value;
                }
            }
        }

        [ProtoMember(15)] public string NumberFormat { get; set; }
    }
}
