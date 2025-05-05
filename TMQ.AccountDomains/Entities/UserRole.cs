using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseDomains;

namespace TMQ.AccountDomains.Entities
{
    public class UserRole : BaseDomain
    {
        public new string Id { get; set; }
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}
