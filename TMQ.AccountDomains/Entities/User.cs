using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TMQ.AccountCommands.Commands;
using TMQ.AccountCommands;
using TMQ.AccountCommands.Events;
using TMQ.AccountReadModels;
using TMQ.BaseCommands;
using TMQ.BaseDomains;
using TMQ.Common;
using TMQ.EnumDefine;

namespace TMQ.AccountDomains.Entities
{
    [Table("User_tbl")]
    public class User : BaseDomain
    {
        public User(RUser user) : base(user) 
        {
            Id = user.Id;
            Email = user.Email;
            FullName = user.FullName;
            Password = user.Password;
            DateOfBirth = user.DateOfBirth;
            PhoneNumber = user.PhoneNumber;
            IsPhoneNumberConfirmed = user.IsPhoneNumberConfirmed;
            CurrentLocation = user.CurrentLocation;
            CurrentResident = user.CurrentResident;
            SecurityStamp = user.SecurityStamp;
            IsTwoFactorEnabled = user.IsTwoFactorEnabled;
            IsEmailConfirmed = user.IsEmailConfirmed;
            PasswordSalt = user.PasswordSalt;
            Story = user.Story;
            NumberOfFollowers = user.NumberOfFollowers;
            NumberOfFriends = user.NumberOfFriends;
            RegistrationDate = user.RegistrationDate;
            Gender = user.Gender;
            WebsiteName = user.WebsiteName;
            AccountType = user.AccountType;
            UpdateTime = user.UpdateTime;
            PrivacySettings = user.PrivacySettings;
            Thumbnail = user.Thumbnail;
            NumberOfPagesLiked = user.NumberOfPagesLiked;
            Relationship = user.Relationship;
            IsLocked = user.IsLocked;
            LastLoginTime = user.LastLoginTime;
            UnLockedTime = user.UnLockedTime;
            Status = user.Status;
            ExternalLogins = user.ExternalLogins?.Select(p => new ExternalLogin(p)).ToList();
            AccountSettings = user.AccountSettings?.Select(p => new AccountSetting(p)).ToList();
            AuthenticationSecretKeys =
                user.AuthenticatorSecretKeysObject?.Select(p => new AuthenticationSecretKey(p)).ToList();
            if (user.RUserInfo != null)
            {
                UserInfo = new UserInfo(user.RUserInfo);
            }

            Addresses = user.Addresses?.Select(p => new Address(p)).ToList();
        }

        public void SetPassword(SetPasswordCommand command)
        {
            Password = EncryptionExtensions.Encryption(Id, command.Password, out string salt);
            PasswordSalt = salt;
            Changed(command);
        }

        public void Change(AccountChangeCommand command)
        {
            Code = command.UserName;
            Email = command.Email.AsEmpty();
            FullName = command.FullName;
            PhoneNumber = command.PhoneNumber.AsEmpty();
            IsPhoneNumberConfirmed = !string.IsNullOrEmpty(PhoneNumber) && command.IsPhoneNumberConfirmed;
            SecurityStamp = null;
            IsEmailConfirmed = !string.IsNullOrEmpty(Email) && command.IsEmailConfirmed;
            AccessFailedCount = 0;
            DateOfBirth = command.DateOfBirth;
            Gender = command.Gender;
            Status = command.Status;
            AccountType = command.AccountType;
            Changed(command);
        }

        public new string Id { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string FullName { get; private set; }
        public DateTime DateOfBirth { get; private set; }
        public string PhoneNumber { get; private set; }
        public bool IsPhoneNumberConfirmed { get; private set; }
        public string? CurrentResident { get; private set; }
        public string? SecurityStamp { get; private set; }
        public bool IsTwoFactorEnabled { get; private set; }
        public bool IsEmailConfirmed { get;  private set; }
        public string? PasswordSalt { get; private set; }
        public int AccessFailedCount { get; private set; }
        public string Story {  get; private set; }
        public int NumberOfFollowers { get; private set; }
        public int NumberOfFriends { get; private set; }
        public DateTime RegistrationDate { get; private set; }
        public GenderEnum Gender { get; private set; }
        public string CurrentLocation { get; private set; }
        public string WebsiteName { get; private set; }
        public AccountTypeEnum AccountType { get; private set; }
        public DateTime? UpdateTime { get; private set; }
        public string PrivacySettings {  get; private set; }
        public string Thumbnail {  get; private set; }
        public int NumberOfPagesLiked { get; private set; }
        public RelationshipEnum Relationship { get; private set; }
        public string Email { get; private set; }
        public bool IsLocked { get; private set; }
        public DateTime LastLoginTime { get; private set; }
        public DateTime UnLockedTime {  get; private set; }
        public AccountStatusEnum Status { get; private set; }
        public virtual ICollection<ExternalLogin>? ExternalLogins { get; private set; }
        public virtual ICollection<AccountSetting>? AccountSettings { get; private set; }
        public virtual ICollection<AuthenticationSecretKey>? AuthenticationSecretKeys { get; private set; }
        public virtual ICollection<Address>? Addresses { get; private set; }
        public virtual UserInfo? UserInfo { get; private set; }
        public string AuthenticatorSecretKeys => Common.Serialize.JsonSerializeObject(AuthenticationSecretKeys);
        public string? RUserInfoJson => Common.Serialize.JsonSerializeObject(UserInfo);

        public User(AccountAddCommand command, string id) : base(command)
        {
            Id = id;
            Code = command.UserName;
            Email = command.Email.AsEmpty();
            FullName = command.FullName;
            if (string.IsNullOrEmpty(command.Password))
            {
                command.Password = CommonUtility.GenerateGuid();
            }

            Password = EncryptionExtensions.Encryption(Id, command.Password, out string salt);
            PasswordSalt = salt;
            PhoneNumber = command.PhoneNumber.AsEmpty();
            IsPhoneNumberConfirmed = !string.IsNullOrEmpty(PhoneNumber) && command.IsPhoneNumberConfirmed;
            SecurityStamp = null;
            IsEmailConfirmed = !string.IsNullOrEmpty(Email) && command.IsEmailConfirmed;
            AccessFailedCount = 0;
            DateOfBirth = command.DateOfBirth;
            Gender = command.Gender;
            Status = command.Status;
            AccountType = command.AccountType;
        }

        public bool ComparePassword(string loginPassword)
        {
            string passwordHash = EncryptionExtensions.Encryption(Id, loginPassword, PasswordSalt.AsEmpty());
            return Password?.Equals(passwordHash) == true;
        }

        public UserAddEvent ToAddEvent()
        {
            return new UserAddEvent()
            {
                ObjectId = Id,
                AccountTypeEnum = AccountType
            };
        }

        public UserChangeEvent ToChangeEvent()
        {
            return new UserChangeEvent()
            {
                ObjectId = Id,
                AccountTypeEnum = AccountType
            };
        }
    }
}
