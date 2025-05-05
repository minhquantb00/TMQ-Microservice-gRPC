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
    public record RUserDeviceMapping : AccountBaseReadModel
    {
        [ProtoMember(1)] public string? Token { get; set; }
        [ProtoMember(2)] public string? DeviceLoginInfo { get; set; }
        [ProtoMember(3)] public string? IP { get; set; }
        [ProtoMember(4)] public string? ParentId { get; set; }
        [ProtoMember(5)] public DateTime ExpireDate { get; set; }
        [ProtoMember(6)] public required string ClientId { get; set; }
        [ProtoMember(7)] public bool RememberMe { get; set; }
        [ProtoMember(8)] public StatusEnum Status { get; set; }
        [ProtoMember(9)] public LoginTypeEnum LoginType { get; set; }
        [ProtoMember(10)] public string? FCMToken { get; set; }
    }
}
