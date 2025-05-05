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
    [Table("UserRelationship_tbl")]
    public class UserRelationship : BaseDomain
    {
        public new string Id { get; set; }
        public string UserId { get; set; }
        public string RelatedUserId { get; set; }
        public RelationshipEnum Relationship { get; set; }
        public bool IsSetRelationship { get; set; }
    }
}
