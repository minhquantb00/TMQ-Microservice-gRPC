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
    [Table("Shard_tbl")]
    public class Shard : BaseDomain
    {
        public Shard(RShard sharding) : base(sharding)
        {
            Id = sharding.Id;
            Name = sharding.Name;
            ShardingGroupId = sharding.ShardingGroupId;
            Setting = sharding.Setting == null ? null : new ShardSetting(sharding.Setting);
            Status = sharding.Status;
        }

        public Shard(ShardAddCommand command, string id) : base(command)
        {
            Id = id;
            Name = command.Name;
            ShardingGroupId = command.ShardingGroupId;
            Status = command.Status;
            Setting = new ShardSetting(command);
        }
        public void Change(ShardChangeCommand command)
        {
            Name = command.Name;
            ShardingGroupId = command.ShardingGroupId;
            Status = command.Status;
            Setting = new ShardSetting(command);
            Changed(command);
        }
        public string Id { get; private set; }
        public string? Name { get; private set; }
        public string? ShardingGroupId { get; private set; }
        public ShardSetting? Setting { get; private set; }
        public StatusEnum Status { get; private set; }
        public string SettingJson => Common.Serialize.JsonSerializeObject(Setting);
    }
}
