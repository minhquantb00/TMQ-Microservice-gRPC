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
    public record ConfigAddCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public string Key { get; set; }
        [ProtoMember(2)] public string Description { get; set; }
        [ProtoMember(3)] public string Value { get; set; }
        [ProtoMember(4)] public StatusEnum Status { get; set; }
        /*[ProtoMember(5)] public override bool IsCache { get; set; }*/
        [ProtoMember(6)] public ConfigTypeEnum? Type { get; set; }
    }

    [ProtoContract]
    public record WikiProfileLastViewChangeCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public DateTime LastChange { get; set; }
    }
}
