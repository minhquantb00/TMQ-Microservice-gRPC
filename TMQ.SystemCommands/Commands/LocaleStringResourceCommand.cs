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
    public record LocaleUpdateCommand : LocaleInsertCommand
    {
    }

    [ProtoContract]
    [ProtoInclude(300, typeof(LocaleUpdateCommand))]
    public record LocaleInsertCommand : LocaleDeleteCommand
    {
        [ProtoMember(2)] public string LanguageId { get; set; }

        [ProtoMember(3)] public string ResourceName { get; set; }

        [ProtoMember(4)] public string ResourceValue { get; set; }

        [ProtoMember(5)] public StatusEnum Status { get; set; }
        [ProtoMember(6)] public int DisplayOrder { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(200, typeof(LocaleInsertCommand))]
    public record LocaleDeleteCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public string Id { get; set; }

        /*[ProtoMember(101)] public override string ObjectId { get; set; }
        [ProtoMember(102)] public override string ProcessUid { get; set; }
        [ProtoMember(103)] public override DateTime ProcessDate { get; set; }
        [ProtoMember(104)] public override string LoginUid { get; set; }
        [ProtoMember(5)] public override bool IsCache { get; set; }*/
    }
}
