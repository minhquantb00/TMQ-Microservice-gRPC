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
    [Table("Locale_tbl")]
    public class Locale : BaseDomain
    {
        public Locale(RLocaleStringResource locale) : base(locale)
        {
            Id = locale.Id;
            LanguageId = locale.LanguageId;
            ResourceName = locale.ResourceName;
            ResourceValue = locale.ResourceValue;
            Status = locale.Status;
            DisplayOrder = locale.DisplayOrder;
        }

        public Locale(LocaleInsertCommand command) : base(command)
        {
            Id = command.Id.Trim();
            LanguageId = command.LanguageId.Trim();
            ResourceName = command.ResourceName.Trim();
            ResourceValue = command.ResourceValue.Trim();
            Status = command.Status;
        }

        public void Change(LocaleUpdateCommand command)
        {
            LanguageId = command.LanguageId.Trim();
            ResourceName = command.ResourceName.Trim();
            ResourceValue = command.ResourceValue.Trim();
            Status = command.Status;
            DisplayOrder = command.DisplayOrder;
            Changed(command);
        }

        public void Change(LocaleInsertCommand command)
        {
            LanguageId = command.LanguageId.Trim();
            ResourceName = command.ResourceName.Trim();
            ResourceValue = command.ResourceValue.Trim();
            Status = command.Status;
            DisplayOrder = command.DisplayOrder;
            Changed(command);
        }

        #region properties

        public new string Id { get; set; }
        public string LanguageId { get; set; }
        public string ResourceName { get; set; }
        public string ResourceValue { get; set; }
        public StatusEnum Status { get; set; }
        public int DisplayOrder { get; set; }

        #endregion
    }
}
