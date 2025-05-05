using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.AccountReadModels;
using TMQ.BaseDomains;
using TMQ.EnumDefine;

namespace TMQ.AccountDomains.Entities
{
    [Table("AccountSetting_tbl")]
    public class AccountSetting : BaseDomain
    {
        public AccountSetting(RAccountSetting accountSetting) : base(accountSetting) 
        {

        }
        public new string Id { get; set; }
        public string? UserId { get;  set; }
        public AccountSettingEnum Type { get; set; }
        public string? Description { get; set; }
        public string? Value { get; set; }
        public StatusEnum Status { get; set; }
    }
}
