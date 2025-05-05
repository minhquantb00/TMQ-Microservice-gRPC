using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.AccountCommands.Commands
{
    [ProtoContract]
    public record SetPasswordCommand : AccountBaseCommand
    {
        [ProtoMember(1)] public required string Password { get; set; }
        [ProtoMember(2)] public required AccountTypeEnum Type { get; set; }
    }

    [ProtoContract]
    public record AccountSyncCommand : AccountBaseCommand
    {
        [ProtoMember(1)] public string[]? Ids { get; set; }
    }
}
