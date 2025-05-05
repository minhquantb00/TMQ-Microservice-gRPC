using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseDomains;
using TMQ.EnumDefine;

namespace TMQ.AccountDomains.Entities
{
    public class UserDeviceLoginNotify : BaseDomain
    {
        public new string Id { get; set; }
        public string UserDeviceMappingId { get; set; }
        public string SessionId { get; set; }
        public DateTime ExpireDate { get; set; }
        public string ClientId { get; set; }
        public bool UserResponse { get; set; }
        public StatusEnum Status { get; set; }
        public string? SocketId { get; set; }
    }
}
