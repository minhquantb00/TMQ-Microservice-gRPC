using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseCommands;
using TMQ.BaseEvents;

namespace TMQ.BaseDomains
{
    public class HistoryDomain : BaseDomain
    {
        public HistoryDomain(string type,
            string objectId,
            string? dealerId,
            string? oldStatus,
            BaseDomain domain,
            BaseCommand command,
            bool isAddNew,
            string? newsCreatedUid = null) :
            base(command)
        {
            Type = type;
            Id = objectId;
            DomainType = domain.GetType().AssemblyQualifiedName;
            CommandType = command.GetType().AssemblyQualifiedName;
            Domain = Common.Serialize.JsonSerializeObjectJsonIgnore<HistoryJsonIgnoreAttribute>(domain);
            Command = Common.Serialize.JsonSerializeObjectJsonIgnore<HistoryJsonIgnoreAttribute>(command);
            IsAddNew = isAddNew;
            Version = domain.Version;
            DealerId = dealerId;
            OldStatus = oldStatus;
            NewsCreatedUid = newsCreatedUid;
        }

        public HistoryDomain(string type,
            string objectId,
            string? dealerId,
            string? oldStatus,
            BaseDomain domain,
            Event @event,
            bool isAddNew) :
            base(@event)
        {
            Type = type;
            Id = objectId;
            DomainType = domain.GetType().AssemblyQualifiedName;
            CommandType = @event.GetType().AssemblyQualifiedName;
            Domain = Common.Serialize.JsonSerializeObjectJsonIgnore<HistoryJsonIgnoreAttribute>(domain);
            Command = Common.Serialize.JsonSerializeObjectJsonIgnore<HistoryJsonIgnoreAttribute>(@event);
            IsAddNew = isAddNew;
            Version = domain.Version;
            DealerId = dealerId;
            OldStatus = oldStatus;
        }

        public HistoryDomain(string type, string objectId, string? dealerId, string? oldStatus, BaseDomain domain,
            BaseCommand command,
            object? commandData,
            bool isAddNew, string? newsCreatedUid) :
            this(type, objectId, dealerId, oldStatus, domain, command, isAddNew, newsCreatedUid)
        {
            CommandData = Common.Serialize.JsonSerializeObjectJsonIgnore<HistoryJsonIgnoreAttribute>(commandData);
        }

        public new string Id { get; private set; }
        public string Type { get; private set; }
        public string? DomainType { get; private set; }
        public string? CommandType { get; private set; }
        public string Domain { get; private set; }
        public string Command { get; private set; }
        public List<HistoryDomainChange> DomainsChange { get; } = [];
        public List<HistoryItemDomain> Items { get; } = [];
        public bool IsAddNew { get; private set; }
        public string? DealerId { get; private set; }
        public string? OldStatus { get; private set; }
        public string? CommandData { get; private set; }

        public string? NewsCreatedUid { get; private set; }

        public void Add(BaseDomain domain, bool isSystem)
        {
            string domainJson = Common.Serialize.JsonSerializeObjectJsonIgnore<HistoryJsonIgnoreAttribute>(domain);
            DomainsChange.Add(new HistoryDomainChange(DomainsChange.Count, domainJson, isSystem));
        }
    }
}
