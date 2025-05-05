using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.AccountReadModels
{
    [ProtoContract]
    public record RRole : AccountBaseReadModel
    {
        [ProtoMember(1)] public string? RoleName { get; set; }
        [ProtoMember(2)] public string? RoleCode { get; set; }
    }
}
