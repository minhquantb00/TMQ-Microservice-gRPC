using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseDomains;

namespace TMQ.AccountDomains.Entities
{
    [Table("Role_tbl")]
    public class Role : BaseDomain
    {
        public new string Id { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
    }
}
