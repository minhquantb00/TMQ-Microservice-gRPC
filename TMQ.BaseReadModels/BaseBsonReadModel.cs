using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TMQ.BaseReadModels
{
    public abstract record BaseBsonReadModel
    {
        public abstract long NumericalOrder { get; set; }
        public abstract ObjectId Id { get; set; }
        public abstract string Code { get; set; }
        public abstract string CreatedUid { get; set; }
        public abstract DateTime CreatedDate { get; set; }
        public abstract DateTime CreatedDateUtc { get; set; }
        public abstract string UpdatedUid { get; set; }
        public abstract DateTime UpdatedDate { get; set; }
        public abstract DateTime UpdatedDateUtc { get; set; }
        public abstract int Version { get; set; }
        public abstract string LoginUid { get; set; }
        public abstract int TotalRow { get; set; }
    }
}
