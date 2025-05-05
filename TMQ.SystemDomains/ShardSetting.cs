using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseDomains;
using TMQ.SystemCommands.Commands;
using TMQ.SystemReadModels;

namespace TMQ.SystemDomains
{
    [Table("ShardSetting_tbl")]
    public class ShardSetting : BaseDomain
    {
        public ShardSetting(RShardingSetting shardingSetting) : base(shardingSetting)
        {
            ESUrl = shardingSetting.ESUrl;
            ESIndexLastName = shardingSetting.ESIndexLastName;
            DatabaseIp = shardingSetting.DatabaseIp;
            DatabaseName = shardingSetting.DatabaseName;
            DatabaseUid = shardingSetting.DatabaseUid;
            DatabasePwd = shardingSetting.DatabasePwd;
        }

        public ShardSetting(ShardAddCommand command) : base(command)
        {
            ESUrl = command.ESUrl;
            ESIndexLastName = command.ESIndexLastName;
            DatabaseIp = command.DatabaseIp;
            DatabaseName = command.DatabaseName;
            DatabaseUid = command.DatabaseUid;
            DatabasePwd = command.DatabasePwd;
        }

        public string? ESUrl { get; private set; }
        public string? ESIndexLastName { get; private set; }
        public string? DatabaseIp { get; private set; }
        public string? DatabaseName { get; private set; }
        public string? DatabaseUid { get; private set; }
        public string? DatabasePwd { get; private set; }
    }
}
