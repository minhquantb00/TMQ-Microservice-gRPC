using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemCommands.Commands
{
    [ProtoContract]
    [ProtoInclude(200, typeof(MenuAddCommand))]
    [ProtoInclude(201, typeof(MenuPermissionAddCommand))]
    public record MenuDeleteCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public string Id { get; set; }
    }
}
