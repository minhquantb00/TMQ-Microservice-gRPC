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
    [Table("UserActivity_tbl")]
    public class UserActivity : BaseDomain
    {
        public new string Id { get; set; }
        public string UserId { get; set; }
        public ActivityType ActivityType { get; set; }
        public string ActivityDetail {  get; set; }
        public DateTime ActivityTime { get; set; }
        public ActivityStatus ActivityStatus { get; set; }
    }
}
