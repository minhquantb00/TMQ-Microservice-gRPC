using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;

namespace TMQ.SystemCommands.Queries
{
    [ProtoContract]
    public record ZoneGetByObjectIdAndObjectTypeQuery : SystemBaseCommand
    {
        /*[ProtoMember(1)] public new string ObjectId { get; set; }*/
        [ProtoMember(2)] public ZoneObjectTypeEnum Type { get; set; }
        [ProtoMember(3)] public string MappingKey { get; set; }
    }
}
