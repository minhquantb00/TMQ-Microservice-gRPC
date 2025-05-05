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
    public record ROtpLimitConfigQuery : SystemBaseCommand
    {
        /*[ProtoMember(101)] public override string ObjectId { get; set; }
        [ProtoMember(102)] public override string ProcessUid { get; set; }
        [ProtoMember(103)] public override DateTime ProcessDate { get; set; }
        [ProtoMember(104)] public override string LoginUid { get; set; }
        [ProtoMember(5)] public override bool IsCache { get; set; }*/
    }

    [ProtoContract]
    public record ConfigGetByTypeQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public ConfigTypeEnum Type { get; set; }
    }

    [ProtoContract]
    public record GetConfigDefaultArticleCategoriesByProductTypeQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string DealerId { get; set; }
        [ProtoMember(2)] public ProductTypeEnum ProductType { get; set; }
    }

    [ProtoContract]
    public record RIpLimitConfigQuery : SystemBaseCommand
    {

    }
    public record GetConfigDefaultArticleCategoriesByVendorTypeQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string DealerId { get; set; }
        [ProtoMember(2)] public VendorTypeEnum VendorType { get; set; }
    }

    [ProtoContract]
    public record ApproveArticleEmailByWebsiteQuery : SystemBaseCommand
    {
    }
}
