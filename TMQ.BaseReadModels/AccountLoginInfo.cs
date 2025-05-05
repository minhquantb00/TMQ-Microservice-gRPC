using ProtoBuf;
using TMQ.Common;
using TMQ.Config;
using TMQ.EnumDefine;

namespace TMQ.BaseReadModels
{
    [ProtoContract]
    public record AccountLoginInfo
    {
        [ProtoMember(1)] public required string Id { get; set; }
        [ProtoMember(2)] public string? Code { get; set; }
        [ProtoMember(3)] public bool IsAdministrator { get; set; }

        [ProtoMember(4)] public HashSet<string>? Permissions { get; set; }

        // [ProtoMember(5)] public required string Token { get; set; }
        [ProtoMember(6)] public required string RefToken { get; set; }
        [ProtoMember(7)] public bool OtpVerify { get; set; }
        [ProtoMember(8)] public int Version { get; set; }

        [ProtoMember(9)] public required string ClientId { get; set; }

        [ProtoMember(10)] public bool RememberMe { get; set; }
        [ProtoMember(11)] public string? PhoneNumber { get; set; }
        [ProtoMember(12)] public string? Email { get; set; }
        [ProtoMember(13)] public string? FullName { get; set; }
        [ProtoMember(14)] public string? AvatarUrl { get; set; }
        [ProtoMember(15)] public required string CurrentPartnerId { get; set; }

        [ProtoMember(16)] private string _currentDealerId = string.Empty;

        public required string CurrentDealerId
        {
            get => ConfigSettingEnum.IsMasterData.GetConfig().AsInt() == 1
                ? ConfigSettingEnum.DealerTMVId.GetConfig()
                : _currentDealerId;
            set => _currentDealerId = value;
        }

        [ProtoMember(17)] public required string LoginUid { get; set; }
        [ProtoMember(18)] public OtpTypeEnum OtpType { get; set; }
        [ProtoMember(19)] public DateTime InitDate { get; set; }
        [ProtoMember(20)] public required int OTPSendCount { get; set; }
        [ProtoMember(21)] public int MinuteExpire { get; set; }
        [ProtoMember(22)] public LoginTypeEnum LoginType { get; set; }
        [ProtoMember(23)] public string[]? PartnerIds { get; set; }
        [ProtoMember(24)] public string[]? DealerIds { get; set; }
        [ProtoMember(25)] public string[]? GroupAdmins { get; set; }
        [ProtoMember(26)] public int PermissionReloadVersion { get; set; }
        [ProtoMember(27)] public AccountTypeEnum AccountType { get; set; }
        [ProtoMember(28)] public bool TwoFactorEnabled { get; set; }
        [ProtoMember(29)] public required string SessionId { get; set; }

        public string? DisplayName
        {
            get
            {
                string? displayName = FullName;
                if (string.IsNullOrEmpty(displayName))
                {
                    displayName = Email;
                }

                if (string.IsNullOrEmpty(displayName))
                {
                    displayName = PhoneNumber;
                }

                return displayName;
            }
        }

        public string ShortName => CommonUtility.UserGetShortName(FullName);

        [ProtoMember(47)] public string CurrentLanguageId { get; set; }
        public DateTime ExpireDate => InitDate.AddMinutes(MinuteExpire);
    }
}
