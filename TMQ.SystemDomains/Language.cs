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
    [Table("Language_tbl")]
    public class Language : BaseDomain
    {
        public Language(RLanguage language) : base(language)
        {
            Name = language.Name;
            Culture = language.Culture.AsEmpty();
            UniqueSeoCode = language.UniqueSeoCode.AsEmpty();
            FlagImageFileName = language.FlagImageFileName.AsEmpty();
            Rtl = language.Rtl;
            LimitedToStores = language.LimitedToStores;
            DefaultCurrencyId = language.DefaultCurrencyId.AsEmpty();
            Status = language.Status;
            DisplayOrder = language.DisplayOrder;
            Type = language.Type;
            DateFormat = language.DateFormat;
            NumberFormat = language.NumberFormat;
        }

        public Language(LanguageAddCommand command) : base(command)
        {
            SetData(command);
        }

        public void SetData(LanguageAddCommand command)
        {
            Code = command.Id;
            Name = command.Name;
            Culture = command.Culture.AsEmpty();
            UniqueSeoCode = command.UniqueSeoCode.AsEmpty();
            FlagImageFileName = command.FlagImageFileName.AsEmpty();
            Rtl = command.Rtl;
            LimitedToStores = command.LimitedToStores;
            DefaultCurrencyId = command.DefaultCurrencyId.AsEmpty();
            Status = command.Status;
            DisplayOrder = command.DisplayOrder;
            Type = command.Type;
            NumberFormat = command.NumberFormat;
            DateFormat = command.DateFormat;
        }

        public void Change(LanguageChangeCommand command)
        {
            SetData(command);
            Changed(command);
        }

        #region properties

        public string Name { get; set; }
        public string Culture { get; set; }
        public string UniqueSeoCode { get; set; }
        public string FlagImageFileName { get; set; }
        public byte Rtl { get; set; }
        public byte LimitedToStores { get; set; }
        public string DefaultCurrencyId { get; set; }
        public StatusEnum Status { get; set; }
        public int DisplayOrder { get; set; }
        public LanguageTypeEnum Type { get; set; }

        public string DateFormat { get; set; }
        public string NumberFormat { get; set; }

        #endregion
    }
}
