using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.EsRepositories
{
    public class EsSearchResultAgg<T>
    {
        public ObjectSearchResult<T>? hits { get; set; }
        public RESCountGets? aggregations { get; set; }
    }

    public class EsSearchAggsResult<T>
    {
        public ObjectSearchResult hits { get; set; }
        //public aggregations<T> aggregations { get; set; }
        public T aggregations { get; set; }
    }

    public class RESCountGets
    {
        public CountObject? groupBy { get; set; }
        public class CountObject
        {
            public int doc_count_error_upper_bound { get; set; }
            public int sum_other_doc_count { get; set; }
            public Bucket[] buckets { get; set; }
        }
        public class Bucket
        {
            public int key { get; set; }
            public int doc_count { get; set; }
        }
    }

}
