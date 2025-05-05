using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.SystemCommands.Queries
{
    [ProtoContract]
    public record MenuSearchQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public MenuPosition Position { get; set; }
        [ProtoMember(2)] public string ObjectId { get; set; }
    }

    [ProtoContract]
    public record MenusGetByWebsiteIdQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string DealerId { get; set; }
    }

    [ProtoContract]
    public record MenusGetByDisplayPermissionQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public MenuPosition? Position { get; set; }
        [ProtoMember(2)] public string ObjectId { get; set; }
        [ProtoMember(3)] public bool IsDisplayPermission { get; set; }
    }

    [ProtoContract]
    public record MenuGetByIdQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public required string Id { get; set; }
    }
}
