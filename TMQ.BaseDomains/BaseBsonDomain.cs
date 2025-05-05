using MongoDB.Bson;
using TMQ.BaseCommands;
using TMQ.BaseEvents;
using TMQ.BaseReadModels;
using TMQ.Common;

namespace TMQ.BaseDomains
{
    public abstract class BaseBsonDomain
    {
        public BaseBsonDomain()
        {
            Id = ObjectId.GenerateNewId();
            Code = Id.ToString();
            CreatedUid = string.Empty;
            CreatedDate = Extension.GetCurrentDate();
            CreatedDateUtc = Extension.GetCurrentDateUtc();
            UpdatedUid = CreatedUid;
            UpdatedDate = CreatedDate;
            UpdatedDateUtc = CreatedDateUtc;
            Version = 0;
        }

        public BaseBsonDomain(BaseBsonReadModel model)
        {
            if (model == null)
            {
                return;
            }

            NumericalOrder = model.NumericalOrder;
            Id = model.Id;
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

        public BaseBsonDomain(BaseCommand message)
        {
            if (message == null)
            {
                return;
            }

            Id = ObjectId.GenerateNewId();
            Code = Id.ToString();
            CreatedDate = message.ProcessDate;
            CreatedDateUtc = message.ProcessDateUtc;
            CreatedUid = message.ProcessUid.AsEmpty();
            UpdatedDate = message.ProcessDate;
            UpdatedDateUtc = message.ProcessDateUtc;
            UpdatedUid = message.ProcessUid.AsEmpty();
            LoginUid = message.LoginUid.AsEmpty();
            Version = 0;
        }

        public BaseBsonDomain(BaseBsonDomain message)
        {
            if (message == null)
            {
                return;
            }

            Id = message.Id;
            Code = message.Code;
            CreatedDate = message.CreatedDate;
            CreatedDateUtc = message.CreatedDateUtc;
            CreatedUid = message.CreatedUid.AsEmpty();
            UpdatedDate = message.UpdatedDate;
            UpdatedDateUtc = message.UpdatedDateUtc;
            UpdatedUid = message.UpdatedUid.AsEmpty();
            LoginUid = message.LoginUid.AsEmpty();
            Version = 0;
        }

        public BaseBsonDomain(Event @event)
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
        }

        public void Changed(BaseDomain message)
        {
            UpdatedDate = message.UpdatedDate;
            UpdatedDateUtc = message.UpdatedDateUtc;
            UpdatedUid = message.UpdatedUid.AsEmpty();
            LoginUid = message.LoginUid.AsEmpty();
        }

        public long NumericalOrder { get; set; }

        public ObjectId Id { get; set; }

        public string Code { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime CreatedDateUtc { get; set; }

        public string CreatedUid { get; set; }

        public DateTime UpdatedDate { get; set; }

        public DateTime UpdatedDateUtc { get; set; }

        public string UpdatedUid { get; set; }
        public string LoginUid { get; set; }
        public int Version { get; set; }
    }
}
