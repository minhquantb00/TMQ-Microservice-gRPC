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
    public record RUserDeviceLoginNotify : AccountBaseReadModel
    {
        [ProtoMember(1)] public string? UserDeviceMappingId { get; set; }
        [ProtoMember(2)] public string? SessionId { get; set; }
        [ProtoMember(3)] public DateTime ExpireDate { get; set; }
        [ProtoMember(4)] public string? ClientId { get; set; }
        [ProtoMember(5)] public bool UserResponse { get; set; }
        [ProtoMember(6)] public StatusEnum Status { get; set; }
        [ProtoMember(7)] public string? SocketId { get; set; }
    }
}
