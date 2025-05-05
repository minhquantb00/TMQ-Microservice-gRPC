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
    public record ZoneGetByIdQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string? Id { get; set; }
    }

    [ProtoContract]
    public record ZoneGetByIdsQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string[]? Ids { get; set; }
    }

    [ProtoContract]
    public record ZoneSearchQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string? Keyword { get; set; }
        [ProtoMember(2)] public string? DealerId { get; set; }
        [ProtoMember(3)] public StatusEnum Status { get; set; }
        [ProtoMember(4)] public int PageIndex { get; set; }
        [ProtoMember(5)] public int PageSize { get; set; }
    }

    [ProtoContract]
    public record ZoneAutoCompleteQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string Keyword { get; set; }
        [ProtoMember(2)] public string DealerId { get; set; }
        [ProtoMember(3)] public StatusEnum Status { get; set; }
    }

    [ProtoContract]
    public record ZonePositionGetByZoneIdQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string? ZoneId { get; set; }
        [ProtoMember(2)] public int TotalPosition { get; set; }
        [ProtoMember(3)] public ZonePositionDisplayTypeEnum? ZonePositionDisplayType { get; set; }
    }

    [ProtoContract]
    public record ZonePositionGetByIdsQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string[]? Ids { get; set; }
    }

    [ProtoContract]
    public record ZoneGetByMappingKeyQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string? MappingKey { get; set; }
    }

    // [ProtoContract]
    // public class ZonePositionGetByZoneIdAndObjectIdsQuery : SystemBaseCommand
    // {
    //     [ProtoMember(1)] public KeyValuePair<string, string>[] ZoneIdAndObjectIds { get; set; }
    // }

    [ProtoContract]
    public record ZoneGetsByWebsiteIdQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string? DealerId { get; set; }
    }

    [ProtoContract]
    public record ZonePositionGetByObjectIdQuery : SystemBaseCommand
    {
        /*[ProtoMember(1)] public string? ObjectId { get; set; }*/

        [ProtoMember(2)] public ZonePositionType Type { get; set; }
    }
}
