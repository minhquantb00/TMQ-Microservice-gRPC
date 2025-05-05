using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseCommands;
using TMQ.BaseEvents;
using TMQ.BaseReadModels;
using TMQ.Common;

namespace TMQ.BaseDomains
{
    public abstract class BaseDomain
    {
        public BaseDomain()
        {
            Code = CommonUtility.GenerateGuid();
            CreatedUid = string.Empty;
            CreatedDate = Extension.GetCurrentDate();
            CreatedDateUtc = Extension.GetCurrentDateUtc();
            UpdatedUid = CreatedUid;
            UpdatedDate = CreatedDate;
            UpdatedDateUtc = CreatedDateUtc;
            Version = 0;
            LoginUid = CreatedUid;
        }

        public BaseDomain(BaseReadModel model)
        {
            NumericalOrder = model.NumericalOrder;
            Code = model.Code;
            CreatedDate = model.CreatedDate;
            CreatedDateUtc = model.CreatedDateUtc;
            CreatedUid = model.CreatedUid;
            UpdatedDate = model.UpdatedDate;
            UpdatedDateUtc = model.UpdatedDateUtc;
            UpdatedUid = model.UpdatedUid;
            Version = model.Version;
            LoginUid = model.LoginUid;
        }

        public BaseDomain(BaseCommand message)
        {
            CreatedDate = message.ProcessDate;
            CreatedDateUtc = message.ProcessDateUtc;
            CreatedUid = message.ProcessUid.AsEmpty();
            UpdatedDate = message.ProcessDate;
            UpdatedDateUtc = message.ProcessDateUtc;
            UpdatedUid = message.ProcessUid.AsEmpty();
            LoginUid = message.LoginUid.AsEmpty();
            Version = 0;
        }

        public BaseDomain(BaseDomain message)
        {
            CreatedDate = message.CreatedDate;
            CreatedDateUtc = message.CreatedDateUtc;
            CreatedUid = message.CreatedUid.AsEmpty();
            UpdatedDate = message.UpdatedDate;
            UpdatedDateUtc = message.UpdatedDateUtc;
            UpdatedUid = message.UpdatedUid.AsEmpty();
            LoginUid = message.LoginUid.AsEmpty();
            Version = 0;
        }

        public BaseDomain(Event @event)
        {
            CreatedDate = @event.ProcessDate;
            CreatedDateUtc = @event.ProcessDateUtc;
            CreatedUid = @event.ProcessUid.AsEmpty();
            UpdatedDate = @event.ProcessDate;
            UpdatedDateUtc = @event.ProcessDateUtc;
            UpdatedUid = @event.ProcessUid.AsEmpty();
            LoginUid = @event.LoginUid.AsEmpty();
            Version = 0;
        }

        public void Changed(BaseCommand message)
        {
            UpdatedDate = message.ProcessDate;
            UpdatedDateUtc = message.ProcessDateUtc;
            UpdatedUid = message.ProcessUid.AsEmpty();
            LoginUid = message.LoginUid.AsEmpty();
            Version++;
        }

        public void Changed(BaseDomain message)
        {
            UpdatedDate = message.UpdatedDate;
            UpdatedDateUtc = message.UpdatedDateUtc;
            UpdatedUid = message.UpdatedUid.AsEmpty();
            LoginUid = message.LoginUid.AsEmpty();
        }

        public void Changed(Event @event)
        {
            UpdatedDate = @event.ProcessDate;
            UpdatedDateUtc = @event.ProcessDateUtc;
            UpdatedUid = @event.ProcessUid.AsEmpty();
            LoginUid = @event.LoginUid.AsEmpty();
        }

        protected void HistoryInit(string type, string? objectId, string? dealerId, string? oldStatus, BaseCommand message,
            bool isAddNew)
        {
            History ??= new HistoryDomain(type, objectId.AsEmpty(), dealerId, oldStatus, this, message, isAddNew);
        }

        protected void HistoryInit(string type, string? objectId, string? dealerId, string? oldStatus, BaseCommand message,
            object? commandData, bool isAddNew, string? newsCreatedUid = null)
        {
            History ??= new HistoryDomain(type, objectId.AsEmpty(), dealerId, oldStatus, this, message, commandData,
                isAddNew, newsCreatedUid);
        }

        protected void HistoryInit(string type, string? objectId, string? dealerId, string? oldStatus, BaseCommand message,
            bool isAddNew, string? newsCreatedUid)
        {
            History ??= new HistoryDomain(type, objectId.AsEmpty(), dealerId, oldStatus, this, message, isAddNew,
                newsCreatedUid);
        }

        protected void HistoryInit(string type, string? objectId, string? dealerId, string? oldStatus, Event message,
            bool isAddNew)
        {
            History ??= new HistoryDomain(type, objectId.AsEmpty(), dealerId, oldStatus, this, message, isAddNew);
        }

        protected void HistoryAdd(bool isSystem = false)
        {
            History?.Add(this, isSystem);
        }

        protected void HistoryAdd(BaseDomain domain, bool isSystem)
        {
            History?.Add(domain, isSystem);
        }

        public long NumericalOrder { get; protected set; }
        public string? Id => Code;
        public string? Code { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
        public DateTime CreatedDateUtc { get; protected set; }
        public string? CreatedUid { get; protected set; }
        public DateTime UpdatedDate { get; protected set; }
        public DateTime UpdatedDateUtc { get; protected set; }
        public string? UpdatedUid { get; protected set; }
        public string? LoginUid { get; protected set; }
        public int Version { get; protected set; }
        [HistoryJsonIgnore] private HistoryDomain? History { get; set; }

        public HistoryDomain? HistoryGet()
        {
            return History;
        }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public sealed class HistoryJsonIgnoreAttribute : Attribute
    {
    }
}
