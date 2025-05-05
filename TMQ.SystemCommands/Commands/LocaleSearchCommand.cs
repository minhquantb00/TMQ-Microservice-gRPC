using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemCommands.Commands
{
    [ProtoContract]
    public record LocaleSearchCommand : SystemBaseCommand
    {
        [ProtoMember(2)] public string LanguageId { get; set; }

        [ProtoMember(3)] public string ResourceName { get; set; }

        [ProtoMember(4)] public string ResourceValue { get; set; }
        [ProtoMember(5)] public int PageIndex { get; set; }
        [ProtoMember(6)] public int PageSize { get; set; }
        [ProtoMember(7)] public string DealerId { get; set; }

        /*[ProtoMember(101)] public override string ObjectId { get; set; }
        [ProtoMember(102)] public override string ProcessUid { get; set; }
        [ProtoMember(103)] public override DateTime ProcessDate { get; set; }
        [ProtoMember(104)] public override string LoginUid { get; set; }
        [ProtoMember(105)] public override bool IsCache { get; set; }*/
    }
}
