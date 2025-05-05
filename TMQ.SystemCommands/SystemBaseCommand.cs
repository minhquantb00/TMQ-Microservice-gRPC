using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseCommands;
using TMQ.SystemCommands.Commands;
using TMQ.SystemCommands.Queries;

namespace TMQ.SystemCommands
{
    [ProtoContract]
    [ProtoInclude(100, typeof(MenuGetByPositionQuery))]
    [ProtoInclude(200, typeof(LanguageGetsQuery))]
    [ProtoInclude(300, typeof(LanguageAddCommand))]
    [ProtoInclude(400, typeof(LanguageGetByTypeQuery))]
    [ProtoInclude(500, typeof(LocaleStringResourceGetByLanguageIdQuery))]
    [ProtoInclude(600, typeof(MenuAddToBookMarkCommand))]
    [ProtoInclude(700, typeof(MenuChangeDisplayPermissionCommand))]
    [ProtoInclude(800, typeof(MenuPermissionAddCommand))]
    [ProtoInclude(900, typeof(MenusGetByDisplayPermissionQuery))]
    [ProtoInclude(1000, typeof(MenuGetByIdQuery))]
    //[ProtoInclude(1100, typeof(ConfigGetByTypeQuery))]
    [ProtoInclude(1200, typeof(LocaleSearchCommand))]
    [ProtoInclude(1300, typeof(LocaleDeleteCommand))]
    [ProtoInclude(1400, typeof(MenuDeleteCommand))]
    [ProtoInclude(1500, typeof(ShardingGroupGetsQuery))]
    [ProtoInclude(1600, typeof(ShardingGroupGetByIdQuery))]
    [ProtoInclude(1700, typeof(ShardGroupAddCommand))]
    [ProtoInclude(1800, typeof(ShardAddCommand))]
    [ProtoInclude(1900, typeof(ShardingGetsQuery))]
    [ProtoInclude(2000, typeof(ShardingGetByIdQuery))]
    [ProtoInclude(2100, typeof(ROtpLimitConfigQuery))]
    [ProtoInclude(2200, typeof(RIpLimitConfigQuery))]
    public record SystemBaseCommand : BaseCommand
    {
        [ProtoMember(1)] public sealed override string ObjectId { get; set; }
        [ProtoMember(2)] public sealed override string ProcessUid { get; set; }
        [ProtoMember(3)] public sealed override DateTime ProcessDate { get; set; }
        [ProtoMember(4)] public sealed override string LoginUid { get; set; }
        [ProtoMember(6)] public string? DealerId { get; set; }
        [ProtoMember(5)] public bool IsCache { get; set; }
    }
}
