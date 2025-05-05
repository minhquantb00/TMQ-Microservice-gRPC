using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseReadModels;
using TMQ.BaseRepositories;
using TMQ.SystemDomains;
using TMQ.SystemReadModels;

namespace TMQ.SystemRepository
{
    public interface IShardingRepository : ISqlDbBaseRepository<Shard>
    {
        Task<RShard?> GetById(string id);
        Task<RShard[]> GetByIds(string[] ids);
        Task<RShard[]> Gets(string shardingGroupId, string? keyword, RefSqlPaging paging);
    }
}
