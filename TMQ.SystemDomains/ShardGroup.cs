using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseDomains;
using TMQ.EnumDefine;
using TMQ.SystemCommands.Commands;
using TMQ.SystemReadModels;

namespace TMQ.SystemDomains
{
    [Table("ShardGroup_tbl")]
    public class ShardGroup : BaseDomain
    {
        public ShardGroup(RShardGroup shardingGroup) : base(shardingGroup)
        {
            Id = shardingGroup.Id;
            Name = shardingGroup.Name;
            Type = shardingGroup.Type;
            TotalShard = shardingGroup.TotalShard;
            Status = shardingGroup.Status;
            Items = shardingGroup.Items?.Select(p => new Shard(p)).ToList();
        }

        public ShardGroup(ShardGroupAddCommand command, string id) : base(command)
        {
            Id = id;
            Name = command.Name;
            Type = command.Type;
            TotalShard = command.TotalShard;
            Status = command.Status;
            Items = [];
        }

        public void Change(ShardGroupChangeCommand command)
        {
            Name = command.Name;
            Type = command.Type;
            TotalShard = command.TotalShard;
            Status = command.Status;
            Changed(command);
        }

        public new string Id { get; private set; }
        public string? Name { get; private set; }
        public ShardingTypeEnum Type { get; private set; }
        public int TotalShard { get; private set; }
        public StatusEnum Status { get; private set; }
        public List<Shard>? Items { get; private set; }
    }
}
