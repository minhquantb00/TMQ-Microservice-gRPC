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
    [Table("ExternalLogin_tbl")]
    public class ExternalLogin : BaseDomain
    {
        public ExternalLogin(RExternalLogin externalLogin) : base(externalLogin) 
        {
            
        }
        public new string Id { get; private set; }
        public ExternalLoginProviderEnum LoginProvider { get; private set; }
        public string? LoginProviderName { get; private set; }
        public string? ProviderKey { get; private set; }
        public string UserId { get; private set; }
        public string? Info { get; private set; }
        public string? Email { get; private set; }
        public string? PhoneNumber { get; private set; }
        public string? FullName { get; private set; }
        public string? Locale { get; private set; }
        public StatusEnum Status { get; private set; }
        public int? BirthYear { get; private set; }
        public int? BirthMonth { get; private set; }
        public int? BirthDay { get; private set; }
        public GenderEnum? Gender { get; private set; }
    }
}
