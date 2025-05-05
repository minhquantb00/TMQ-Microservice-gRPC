using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseDomains;

namespace TMQ.AccountDomains.Entities
{
    [Table("UserBlocked_tbl")]
    public class UserBlocked : BaseDomain
    {
        public new string Id { get; set; }
        public string UserId { get; set; }
        public string BlockedUserId { get; set; }
        public bool IsBlocked { get; set; }
    }
}
