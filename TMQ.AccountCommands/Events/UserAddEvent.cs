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
    [ProtoInclude(350, typeof(UserChangeEvent))]
    public record UserAddEvent : AccountBaseEvent
    {
        [ProtoMember(1)] public AccountTypeEnum AccountTypeEnum { get; set; }
    }

    [ProtoContract]
    public record UserChangeEvent : UserAddEvent
    {
    }
}
