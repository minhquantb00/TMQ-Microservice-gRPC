using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.AccountCommands.Commands
{
    [ProtoContract]
    public record AccountAddCommand : AccountBaseCommand
    {
        [ProtoMember(1)] public string UserName { get;  set; }
        [ProtoMember(2)] public string Password { get;  set; }
        [ProtoMember(3)] public string FullName { get;  set; }
        [ProtoMember(4)] public DateTime DateOfBirth { get;  set; }
        [ProtoMember(5)] public string PhoneNumber { get;  set; }
        [ProtoMember(6)] public bool IsPhoneNumberConfirmed { get;  set; }
        [ProtoMember(7)] public string? CurrentResident { get;  set; }
        [ProtoMember(8)] public string? SecurityStamp { get;  set; }
        [ProtoMember(9)] bool IsTwoFactorEnabled { get;  set; }
        [ProtoMember(10)] public bool IsEmailConfirmed { get;  set; }
        [ProtoMember(11)] public string? PasswordSalt { get;  set; }
        [ProtoMember(12)] public int AccessFailedCount { get;  set; }
        [ProtoMember(13)] public string Story { get;  set; }
        [ProtoMember(14)] public int NumberOfFollowers { get;  set; }
        [ProtoMember(15)] public int NumberOfFriends { get;  set; }
        [ProtoMember(16)] public DateTime RegistrationDate { get;  set; }
        [ProtoMember(17)] public GenderEnum Gender { get;  set; }
        [ProtoMember(18)] public string CurrentLocation { get;  set; }
        [ProtoMember(19)] public string WebsiteName { get;  set; }
        [ProtoMember(20)] public AccountTypeEnum AccountType { get;  set; }
        [ProtoMember(21)] public DateTime? UpdateTime { get;  set; }
        [ProtoMember(22)] public string PrivacySettings { get;  set; }
        [ProtoMember(23)] public string Thumbnail { get;  set; }
        [ProtoMember(24)] public int NumberOfPagesLiked { get;  set; }
        [ProtoMember(25)] public RelationshipEnum Relationship { get;  set; }
        [ProtoMember(26)] public string Email { get;  set; }
        [ProtoMember(27)] public bool IsLocked { get;  set; }
        [ProtoMember(28)] public DateTime LastLoginTime { get;  set; }
        [ProtoMember(29)] public DateTime UnLockedTime { get;  set; }
        [ProtoMember(30)] public AccountStatusEnum Status { get;  set; }
    }

    [ProtoContract]
    public record AccountChangeCommand : AccountBaseCommand
    {
        [ProtoMember(1)] public string UserName { get; set; }
        [ProtoMember(2)] public string Password { get; set; }
        [ProtoMember(3)] public string FullName { get; set; }
        [ProtoMember(4)] public DateTime DateOfBirth { get; set; }
        [ProtoMember(5)] public string PhoneNumber { get; set; }
        [ProtoMember(6)] public bool IsPhoneNumberConfirmed { get; set; }
        [ProtoMember(7)] public string? CurrentResident { get; set; }
        [ProtoMember(8)] public string? SecurityStamp { get; set; }
        [ProtoMember(9)] bool IsTwoFactorEnabled { get; set; }
        [ProtoMember(10)] public bool IsEmailConfirmed { get; set; }
        [ProtoMember(11)] public string? PasswordSalt { get; set; }
        [ProtoMember(12)] public int AccessFailedCount { get; set; }
        [ProtoMember(13)] public string Story { get; set; }
        [ProtoMember(14)] public int NumberOfFollowers { get; set; }
        [ProtoMember(15)] public int NumberOfFriends { get; set; }
        [ProtoMember(16)] public DateTime RegistrationDate { get; set; }
        [ProtoMember(17)] public GenderEnum Gender { get; set; }
        [ProtoMember(18)] public string CurrentLocation { get; set; }
        [ProtoMember(19)] public string WebsiteName { get; set; }
        [ProtoMember(20)] public AccountTypeEnum AccountType { get; set; }
        [ProtoMember(21)] public DateTime? UpdateTime { get; set; }
        [ProtoMember(22)] public string PrivacySettings { get; set; }
        [ProtoMember(23)] public string Thumbnail { get; set; }
        [ProtoMember(24)] public int NumberOfPagesLiked { get; set; }
        [ProtoMember(25)] public RelationshipEnum Relationship { get; set; }
        [ProtoMember(26)] public string Email { get; set; }
        [ProtoMember(27)] public bool IsLocked { get; set; }
        [ProtoMember(28)] public DateTime LastLoginTime { get; set; }
        [ProtoMember(29)] public DateTime UnLockedTime { get; set; }
        [ProtoMember(30)] public AccountStatusEnum Status { get; set; }
    }
}
