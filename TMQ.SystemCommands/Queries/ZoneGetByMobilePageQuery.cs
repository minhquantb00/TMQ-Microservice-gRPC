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
    public record ZoneGetByMobilePageQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public MobilePageEnum MobilePage { get; set; }
    }
}
