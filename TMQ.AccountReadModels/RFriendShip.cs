using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.AccountReadModels
{
    [ProtoContract]
    public record RFriendShip : AccountBaseReadModel
    {
        [ProtoMember(1)] public string? UserOneId { get; private set; }
        [ProtoMember(2)] public string? UserTwoId { get; private set; }
        [ProtoMember(3)] public FriendShipStatus FriendShipStatus { get; private set; }
        [ProtoMember(4)] public DateTime AcceptTime { get; private set; }
        [ProtoMember(5)] public DateTime RefuseTime { get; private set; }
        [ProtoMember(6)] public DateTime CancelTime { get; private set; }
    }
}
