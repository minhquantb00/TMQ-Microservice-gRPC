using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.AccountCommands
{
    [ProtoContract]
    public record ExternalLoginUserRemoveCommand : AccountBaseCommand
    {
        [ProtoMember(1)] public new required string ObjectId { get; set; }
    }
}
