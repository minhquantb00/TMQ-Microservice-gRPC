using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.SystemCommands.Queries
{
    [ProtoContract]
    public record ShardingGroupGetsQuery : SystemBaseCommand
    {
        public string? Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }

    [ProtoContract]
    public record ShardingGroupGetByIdQuery : SystemBaseCommand
    {
    }

    [ProtoContract]
    public record ShardingGetsQuery : SystemBaseCommand
    {
        public string? Keyword { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public required string ShardingGroupId { get; set; }
    }

    [ProtoContract]
    public record ShardingGetByIdQuery : SystemBaseCommand
    {
    }
}
