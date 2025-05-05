using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.SystemCommands.Queries
{
    [ProtoContract]
    public record LanguageGetByTypeQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public LanguageTypeEnum Type { get; set; }
    }

    [ProtoContract]
    public record LanguageGetsQuery : SystemBaseCommand
    {
    }
}
