using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.EsRepositories
{
    public interface IESRepository
    {
        Task<string> Add<T>(string indexName, string id, T indexObject, Func<string, string, string, Task> addLog = null,
            Dictionary<string, string> parameters = null);

        Task<string> Add<T>((string, string, T)[] indexEses);
        Task<string> Add<T>((string, string, T)[] indexEses, Dictionary<string, string> parameters = null);
        Task<string> AddWithShard<T>((string, string, T)[] indexEses, string shard);
        Task<string> Remove<T>((string, string, T)[] indexEses);
        Task<string> Remove(string indexName, string script);
        Task<string> RemoveById(string indexName, string id);
        Task<string> UpdateValue(string indexName, string id, string script);
        Task<string> UpdateValueBulk<T>((string, string, T)[] indexEses);
        Task<string> Update(string indexName, string script);
        Task<string> UpdateBulk<T>((string, string, T)[] indexEses);
        Task<string> GetById(string indexName, string id);
        Task<string> GetById(string indexName, string[] ids);

        Task<string> Search(string indexName, string script, bool isAggs = false,
            Func<string, string, string, Task>? addLog = null);

        Task<string> Search(string[] indexNames, string script, bool isAggs = false,
            Func<string, string, string, Task>? addLog = null, bool isValidatedIndex = false);

        Task<string> MultiSearch((string, string)[] indexesAndQueries, bool isAggs = false,
            Func<string, string, string, Task>? addLog = null);

        Task<(bool, string)> CreateIndex(string indexName, string indexFile, int numberOfShards);
        Task<string[]> GetExistsIndexes(string[] indexNames);
    }
}
