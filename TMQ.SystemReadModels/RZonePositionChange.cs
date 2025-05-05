using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemReadModels
{
    [ProtoContract]
    public class RZonePositionChange
    {
        [ProtoMember(1)] public string ObjectId { get; set; }
        [ProtoMember(2)] public string Id { get; set; }
    }
}
