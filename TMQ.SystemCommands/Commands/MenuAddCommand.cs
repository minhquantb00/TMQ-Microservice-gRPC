using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.SystemCommands.Commands
{
    [ProtoContract]
    [ProtoInclude(300, typeof(MenuChangeCommand))]
    public record MenuAddCommand : MenuDeleteCommand
    {
        [ProtoMember(2)] public string ParentId { get; set; }
        [ProtoMember(3)] public string Name { get; set; }
        [ProtoMember(4)] public MenuTypeEnum Type { get; set; }
        [ProtoMember(5)] public string Url { get; set; }
        [ProtoMember(6)] public string ActionDefineId { get; set; }
        [ProtoMember(7)] public MenuPosition PositionId { get; set; }
        [ProtoMember(8)] public int Priority { get; set; }
        [ProtoMember(9)] public StatusEnum Status { get; set; }
        [ProtoMember(10)] public string DealerId { get; set; }
        [ProtoMember(11)] public string CssClassIcon { get; set; }
        [ProtoMember(12)] public bool IsDisplayPermission { get; set; }
        [ProtoMember(13)] public SystemOptionEnum SystemOption { get; set; }
        [ProtoMember(14)] public string PathBase { get; set; }
        [ProtoMember(15)] public string BaseUrl { get; set; }
        [ProtoMember(16)] public DateTime? StartDate { get; set; }
        [ProtoMember(17)] public DateTime? EndDate { get; set; }
    }

    [ProtoContract]
    [ProtoInclude(100, typeof(MenuPermissionChangeCommand))]
    public record MenuPermissionAddCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public string Id { get; set; }
        [ProtoMember(2)] public string ParentId { get; set; }
        [ProtoMember(3)] public string Name { get; set; }
        [ProtoMember(4)] public MenuTypeEnum Type { get; set; }
        [ProtoMember(5)] public string Url { get; set; }
        [ProtoMember(7)] public MenuPosition PositionId { get; set; }
        [ProtoMember(8)] public int Priority { get; set; }
        [ProtoMember(9)] public StatusEnum Status { get; set; }
        [ProtoMember(10)] public string DealerId { get; set; }
        [ProtoMember(11)] public bool IsDisplayPermission { get; set; }
        [ProtoMember(12)] public DateTime? StartDate { get; set; }
        [ProtoMember(13)] public DateTime? EndDate { get; set; }
    }

    [ProtoContract]
    public record MenuAddToBookMarkCommand : SystemBaseCommand
    {
        [ProtoMember(1)] public string Url { get; set; }
        [ProtoMember(2)] public string Name { get; set; }
        [ProtoMember(3)] public string Id { get; set; }
        [ProtoMember(4)] public string DealerId { get; set; }
    }
}
