using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseDomains;
using TMQ.Common;
using TMQ.EnumDefine;
using TMQ.SystemCommands.Commands;
using TMQ.SystemReadModels;

namespace TMQ.SystemDomains
{
    [Table("ConfigDomain_tbl")]
    public class ConfigDomain : BaseDomain
    {
        #region Properties

        public string Key { get; private set; }
        public string Description { get; private set; }
        public string Value { get; private set; }
        public StatusEnum Status { get; private set; }
        public ConfigTypeEnum Type { get; private set; }

        #endregion

        public ConfigDomain(RConfig config) : base(config)
        {
            Key = config.Key;
            Description = config.Description;
            Value = config.Value;
            Status = config.Status;
        }

        public ConfigDomain(ConfigAddCommand command) : base(command)
        {
            Key = command.Key;
            Description = command.Description.AsEmpty();
            Value = command.Value;
            Status = EnumDefine.StatusEnum.Active;
            Type = command.Type.GetValueOrDefault();
        }

        public void Change(string value)
        {
            Value = value;
            UpdatedDate = DateTime.Now;
            UpdatedDateUtc = DateTime.UtcNow;
        }
    }
}
