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
    [Table("AuthenticationSecretKey_tbl")]
    public class AuthenticationSecretKey : BaseDomain
    {
        public AuthenticationSecretKey(RAuthenticatorSecretKey authenticatorSecretKey) : base(authenticatorSecretKey) 
        {
            
        }
        public new string? Id { get; set; }
        public OtpTypeEnum Type { get; set; }
        public string? Key { get; set; }
        public string? Info { get; set; }
        public bool IsDefault { get; set; }
        public StatusEnum Status { get; set; }
    }
}
