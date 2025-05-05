using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemCommands.Commands
{
    [ProtoContract]
    public record MenuChangeCommand : MenuAddCommand
    {
        [ProtoMember(2)] public int DisplayOrder { get; set; }
    }

    [ProtoContract]
    public record MenuPermissionChangeCommand : MenuPermissionAddCommand
    {
        [ProtoMember(1)] public string Id { get; set; }
    }

    [ProtoContract]
    public record MenuChangeDisplayPermissionCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public string Id { get; set; }
        [ProtoMember(2)] public bool IsDisplayPermission { get; set; }
    }
}
