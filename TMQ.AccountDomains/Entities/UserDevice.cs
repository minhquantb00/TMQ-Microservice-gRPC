using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseDomains;
using TMQ.EnumDefine;

namespace TMQ.AccountDomains.Entities
{
    [Table("UserDevice_tbl")]
    public class UserDevice : BaseDomain
    {
        public new string Id { get; private set; }
        public string UserDeviceMappingId { get; private set; }
        public string? Token { get; private set; }
        public string? DeviceLoginInfo { get; private set; }
        public string? IP { get; private set; }
        public string? ParentId { get; private set; }
        public DateTime ExpireDate { get; private set; }
        public string ClientId { get; private set; }
        public bool RememberMe { get; private set; }
        public bool Trusted { get; private set; }
        public StatusEnum Status { get; private set; }
        public LoginTypeEnum LoginType { get; private set; }
        public string? FCMToken { get; private set; }
        public DateTime? OtpVerifyTime { get; private set; }
        public DateTime? OtpCMSVerifyTime { get; private set; }
    }
}
