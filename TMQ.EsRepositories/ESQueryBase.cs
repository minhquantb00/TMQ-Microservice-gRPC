using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TMQ.EsRepositories
{
    public abstract class ESQueryBase
    {
        public ESQueryBase()
        {
            track_total_hits = true;
            from = 0;
            size = 1;
            AggSize = 100000;
        }

        public int? from { get; set; }
        public int? size { get; set; }
        public bool track_total_hits { get; private set; }

        [Newtonsoft.Json.JsonIgnore] public const string DateFormat = "MM/dd/yyyy";

        public abstract string ToQuery();
        [Newtonsoft.Json.JsonIgnore] public int AggSize { get; private set; }
    }

    public abstract class ESQueryBase<T, S> : ESQueryBase
    {
        public ESQueryBase() : base()
        {
            query = new Query()
            {
                _bool = new Bool()
            };
            sort = new List<S>();
        }

        public Query query { get; private set; }
        public IList<S> sort { get; private set; }

        public class Query
        {
            [JsonProperty("bool")] public Bool _bool { get; set; }
        }

        public class Bool
        {
            public IList<Filter> filter { get; set; }
        }

        public class Filter
        {
            public T term { get; set; }
        }

        public void AddFilter(Filter filter)
        {
            if (query._bool.filter == null)
            {
                query = new Query()
                {
                    _bool = new Bool()
                    {
                        filter = new List<Filter>()
                    }
                };
            }

            query._bool.filter.Add(filter);
        }
    }
    public class EsSearchResult<T>
    {
        public ObjectSearchResult<T> hits { get; set; }
        public T _source { get; set; }
    }
    public class ObjectSearchData<T>
    {
        public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }
        public double? _score { get; set; }
        public T _source { get; set; }
        public string _routing { get; set; }
    }

    public class ObjectSearchResult
    {
        public total total { get; set; }
        public decimal? max_score { get; set; }
    }

    public class ObjectSearchResult<T>
    {
        public total total { get; set; }
        public ObjectSearchData<T>[] hits { get; set; }
        public decimal? max_score { get; set; }
    }

    public class total
    {
        public int value { get; set; }
        public string relation { get; set; }
    }
}
