using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.BaseReadModels
{
    public abstract class BaseReadElasticsearchModel
    {
        public BaseReadElasticsearchModel()
        {
        }

        public BaseReadElasticsearchModel(BaseReadModel model)
        {
            NumericalOrder = model.NumericalOrder;
            Id = model.Id;
            Code = model.Code;
            CreatedUid = model.CreatedUid;
            CreatedDate = model.CreatedDate;
            CreatedDateUtc = model.CreatedDateUtc;
            UpdatedUid = model.UpdatedUid;
            UpdatedDate = model.UpdatedDate;
            UpdatedDateUtc = model.UpdatedDateUtc;
            Version = model.Version;
            LoginUid = model.LoginUid;
        }

        public abstract long NumericalOrder { get; set; }
        public abstract string Id { get; set; }
        public abstract string Code { get; set; }
        public abstract string CreatedUid { get; set; }
        public abstract DateTime CreatedDate { get; set; }
        public abstract DateTime CreatedDateUtc { get; set; }
        public abstract string UpdatedUid { get; set; }
        public abstract DateTime UpdatedDate { get; set; }
        public abstract DateTime UpdatedDateUtc { get; set; }
        public abstract int Version { get; set; }
        public abstract string LoginUid { get; set; }
        public long CreatedDateValue => CreatedDate > DateTime.MinValue ? CreatedDate.ToFileTime() : 0;
        public long UpdatedDateValue => UpdatedDate > DateTime.MinValue ? UpdatedDate.ToFileTime() : 0;
    }
}
