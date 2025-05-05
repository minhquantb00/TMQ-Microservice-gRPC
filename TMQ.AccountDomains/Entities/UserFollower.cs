using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseDomains;

namespace TMQ.AccountDomains.Entities
{
    [Table("UserFollower_tbl")]
    public class UserFollower : BaseDomain
    {
        public new string Id { get; set; }
        public string UserFollowId { get; set; }
        public string UserFollowingId { get; set; }
        public bool IsFollowed { get; set; }
    }
}
