using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseReadModels;

namespace TMQ.AccountReadModels
{
    [ProtoContract]
    [ProtoInclude(200, typeof(RUser))]
    [ProtoInclude(300, typeof(RUserInfo))]
    [ProtoInclude(400, typeof(RExternalLogin))]
    [ProtoInclude(500, typeof(RAccountSetting))]
    [ProtoInclude(600, typeof(RAuthenticatorSecretKey))]
    [ProtoInclude(700, typeof(RAddress))]
    [ProtoInclude(800, typeof(RConfirmEmail))]
    [ProtoInclude(900, typeof(RFriendShip))]
    [ProtoInclude(1000, typeof(RRefreshToken))]
    [ProtoInclude(1100, typeof(RRole))]
    [ProtoInclude(1200, typeof(RUserActivity))]
    [ProtoInclude(1300, typeof(RUserBlocked))]
    [ProtoInclude(1400, typeof(RUserDevice))]
    [ProtoInclude(1500, typeof(RUserDeviceLoginNotify))]
    [ProtoInclude(1600, typeof(RUserDeviceMapping))]
    [ProtoInclude(1700, typeof(RUserFollower))]
    [ProtoInclude(1800, typeof(RUserRelationship))]
    [ProtoInclude(1900, typeof(RUserRole))]
    public record AccountBaseReadModel : BaseReadModel
    {
        [ProtoMember(1)] public override long NumericalOrder { get; set; }
        [ProtoMember(2)] public override required string Id { get; set; }
        [ProtoMember(3)] public override string? Code { get; set; }
        [ProtoMember(4)] public override required string CreatedUid { get; set; }
        [ProtoMember(5)] public override DateTime CreatedDate { get; set; }
        [ProtoMember(6)] public override DateTime CreatedDateUtc { get; set; }
        [ProtoMember(7)] public override required string UpdatedUid { get; set; }
        [ProtoMember(8)] public override DateTime UpdatedDate { get; set; }
        [ProtoMember(9)] public override DateTime UpdatedDateUtc { get; set; }
        [ProtoMember(10)] public override int Version { get; set; }
        [ProtoMember(11)] public override required string LoginUid { get; set; }
        [ProtoMember(12)] public override int TotalRow { get; set; }
        [ProtoMember(13)] public override required string ShardId { get; set; }
    }
}
