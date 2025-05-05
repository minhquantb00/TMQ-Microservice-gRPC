using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseDomains;

namespace TMQ.AccountDomains.Entities
{
    [Table("ConfirmEmail_tbl")]
    public class ConfirmEmail : BaseDomain
    {
        public new string Id { get; set; }
        public string ConfirmCode { get; set; }
        public DateTime ExpiredTime { get; set; }
        public string UserId { get; set; }
        public bool IsConfirmed { get; set; }
    }
}
