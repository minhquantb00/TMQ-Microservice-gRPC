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
    public record RUser : AccountBaseReadModel
    {
        [ProtoMember(1)] public string UserName { get; private set; }
        [ProtoMember(2)]  public string Password { get; private set; }
        [ProtoMember(3)]  public string FullName { get; private set; }
        [ProtoMember(4)]  public DateTime DateOfBirth { get; private set; }
        [ProtoMember(5)]  public string PhoneNumber { get; private set; }
        [ProtoMember(6)]  public bool IsPhoneNumberConfirmed { get; private set; }
        [ProtoMember(7)]  public string? CurrentResident { get; private set; }
        [ProtoMember(8)]  public string? SecurityStamp { get; private set; }
        [ProtoMember(9)]  public bool IsTwoFactorEnabled { get; private set; }
        [ProtoMember(10)]  public bool IsEmailConfirmed { get; private set; }
        [ProtoMember(11)]  public string? PasswordSalt { get; private set; }
        [ProtoMember(12)]  public int AccessFailedCount { get; private set; }
        [ProtoMember(13)]  public string Story { get; private set; }
        [ProtoMember(14)]  public int NumberOfFollowers { get; private set; }
        [ProtoMember(15)]  public int NumberOfFriends { get; private set; }
        [ProtoMember(16)]  public DateTime RegistrationDate { get; private set; }
        [ProtoMember(17)]  public GenderEnum Gender { get; private set; }
        [ProtoMember(18)]  public string CurrentLocation { get; private set; }
        [ProtoMember(19)]  public string WebsiteName { get; private set; }
        [ProtoMember(20)]  public AccountTypeEnum AccountType { get; private set; }
        [ProtoMember(21)]  public DateTime? UpdateTime { get; private set; }
        [ProtoMember(22)]  public string PrivacySettings { get; private set; }
        [ProtoMember(23)]  public string Thumbnail { get; private set; }
        [ProtoMember(24)]  public int NumberOfPagesLiked { get; private set; }
        [ProtoMember(25)]  public RelationshipEnum Relationship { get; private set; }
        [ProtoMember(26)]  public string Email { get; private set; }
        [ProtoMember(27)]  public bool IsLocked { get; private set; }
        [ProtoMember(28)]  public DateTime LastLoginTime { get; private set; }
        [ProtoMember(29)]  public DateTime UnLockedTime { get; private set; }
        [ProtoMember(30)]  public AccountStatusEnum Status { get; private set; }
        [ProtoMember(31)]  public string AuthenticatorSecretKeys {  get; private set; }
        [ProtoMember(32)]  public string? RUserInfoJson {  get; private set; }
        [ProtoMember(33)] public RExternalLogin[]? ExternalLogins { get; set; }
        [ProtoMember(34)] public RAccountSetting[]? AccountSettings { get; set; }

        public RAuthenticatorSecretKey[]? AuthenticatorSecretKeysObject =>
        Common.Serialize.JsonDeserializeObject<RAuthenticatorSecretKey[]>(AuthenticatorSecretKeys);

        public RUserInfo? RUserInfo =>
            Common.Serialize.JsonDeserializeObject<RUserInfo>(RUserInfoJson);

        public RAddress[]? Addresses { get; set; }
    }

    [ProtoContract]
    public record RUserInfo : AccountBaseReadModel
    {
        [ProtoMember(1)] public string? Identity { get; set; }
        [ProtoMember(2)] public string[]? JobIds { get; set; }
        [ProtoMember(3)] public string? MaritalStatusId { get; set; }
        [ProtoMember(4)] public string? FavoriteDrink { get; set; }
        [ProtoMember(5)] public EncryptTypeEnum EncryptType { get; set; }
        [ProtoMember(6)] public string? ImageFrontUrl { get; set; }
        [ProtoMember(7)] public string? ImageBackUrl { get; set; }
    }
}
