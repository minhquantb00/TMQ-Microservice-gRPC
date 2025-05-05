using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.AccountReadModels
{
    [ProtoContract]
    public record RAuthenticatorSecretKey : AccountBaseReadModel
    {
        [ProtoMember(1)] public OtpTypeEnum Type { get; set; }
        [ProtoMember(2)] public required string Key { get; set; }
        [ProtoMember(3)] public required string Info { get; set; }
        [ProtoMember(4)] public bool IsDefault { get; set; }
        [ProtoMember(5)] public StatusEnum Status { get; set; }
    }
}
