using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemCommands.Queries
{
    [ProtoContract]
    public record LocaleStringResourceGetByKeysQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string[] Keys { get; set; }
        [ProtoMember(2)] public string LanguageId { get; set; }
    }
}
