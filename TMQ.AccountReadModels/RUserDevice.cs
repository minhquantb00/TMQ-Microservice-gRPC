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
    public record RUserDevice : AccountBaseReadModel
    {
        [ProtoMember(1)] public string UserDeviceMappingId { get; private set; }
        [ProtoMember(2)] public string? Token { get; private set; }
        [ProtoMember(3)] public string? DeviceLoginInfo { get; private set; }
        [ProtoMember(4)] public string? IP { get; private set; }
        [ProtoMember(5)] public string? ParentId { get; private set; }
        [ProtoMember(6)] public DateTime ExpireDate { get; private set; }
        [ProtoMember(7)] public string ClientId { get; private set; }
        [ProtoMember(8)] public bool RememberMe { get; private set; }
        [ProtoMember(9)] public bool Trusted { get; private set; }
        [ProtoMember(10)] public StatusEnum Status { get; private set; }
        [ProtoMember(11)] public LoginTypeEnum LoginType { get; private set; }
        [ProtoMember(12)] public string? FCMToken { get; private set; }
        [ProtoMember(13)] public DateTime? OtpVerifyTime { get; private set; }
        [ProtoMember(14)] public DateTime? OtpCMSVerifyTime { get; private set; }
    }
}
