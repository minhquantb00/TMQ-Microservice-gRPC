using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemCommands.Queries
{
    [ProtoContract]
    public record GetNextCodeQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string? TypeName { get; set; }
        [ProtoMember(2)] public bool IsDigit { get; set; }
        [ProtoMember(3)] public int Number { get; set; }
        [ProtoMember(4)] public string? Prefix { get; set; }
        [ProtoMember(5)] public int? TotalValue { get; set; }
    }

    [ProtoContract]
    public record GetNextMultipleCodeQuery : SystemBaseCommand
    {
        [ProtoMember(1)] public string? TypeName { get; set; }
        [ProtoMember(2)] public int TotalValue { get; set; }
    }
}
