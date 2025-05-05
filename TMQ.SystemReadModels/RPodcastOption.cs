using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemReadModels
{
    [ProtoContract]
    public class RPodcastOption
    {
        [ProtoMember(1)] public string? DefaultBannerLightDesktop { get; set; }
        [ProtoMember(2)] public string? DefaultBannerDarkDesktop { get; set; }
        [ProtoMember(3)] public string? DefaultBannerLightMobile { get; set; }
        [ProtoMember(4)] public string? DefaultBannerDarkMobile { get; set; }
        [ProtoMember(5)] public string? PodcastMainCateId { get; set; }
        [ProtoMember(6)] public string? VoiceCodeDefault { get; set; }
        [ProtoMember(7)] public string? SpeedRateDefault { get; set; }
        [ProtoMember(8)] public string? DelayDefault { get; set; }
    }
}
