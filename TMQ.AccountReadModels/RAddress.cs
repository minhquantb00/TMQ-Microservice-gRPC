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
    public record RAddress : AccountBaseReadModel
    {
        [ProtoMember(1)] public StatusEnum Status { get; set; }
        [ProtoMember(2)] public required string UserId { get; set; }
        [ProtoMember(3)] public string? CountryId { get; set; }
        [ProtoMember(4)] public int? ProvinceId { get; set; }
        [ProtoMember(5)] public int? DistrictId { get; set; }
        [ProtoMember(6)] public int? WardId { get; set; }
        [ProtoMember(7)] public int? StreetId { get; set; }
        [ProtoMember(8)] public string? Detail { get; set; }
        [ProtoMember(9)] public string? Description { get; set; }
        [ProtoMember(10)] public bool IsDefault { get; set; }
        [ProtoMember(11)] public AddressTypeEnum Type { get; set; }
    }
}
