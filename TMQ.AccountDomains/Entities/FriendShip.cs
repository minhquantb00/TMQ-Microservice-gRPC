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
    [Table("FriendShip_tbl")]
    public class FriendShip : BaseDomain
    {
        public new string Id { get; private set; }
        public string UserOneId { get; private set; }
        public string UserTwoId { get; private set; }
        public FriendShipStatus FriendShipStatus { get; private set; }
        public DateTime AcceptTime { get; private set; }
        public DateTime RefuseTime { get; private set; }
        public DateTime CancelTime { get; private set; }
    }
}
