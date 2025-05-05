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
    [Table("UserInfo_tbl")]
    public class UserInfo : BaseDomain
    {
        public UserInfo(RUserInfo userInfo) : base(userInfo) 
        {

        }
        public string? Identity { get; set; }
        public EncryptTypeEnum EncryptType { get; set; }
        public string? ImageFrontUrl { get; set; }
        public string? ImageBackUrl { get; set; }
        public string[]? JobIds { get; set; }
        public string? MaritalStatusId { get; set; }
        public string? FavoriteDrink { get; set; }
    }
}
