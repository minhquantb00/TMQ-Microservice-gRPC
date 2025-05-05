using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.AccountCommands.Events
{

    [ProtoContract]
    public record UserDeviceChangeFcmTokenEvent : AccountBaseEvent
    {
        [ProtoMember(1)] public required string UserDeviceId { get; set; }
        [ProtoMember(2)] public string[]? UserDeviceIdsRemove { get; set; }
        [ProtoMember(3)] public LoginTypeEnum LoginType { get; set; }
    }
}
