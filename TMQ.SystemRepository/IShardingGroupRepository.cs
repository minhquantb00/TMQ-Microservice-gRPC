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
    public interface IShardingGroupRepository : ISqlDbBaseRepository<ShardGroup>
    {
        Task<RShardGroup?> GetById(string id);
        Task<RShardGroup[]> GetByIds(string[] ids);
        Task<RShardGroup[]> Gets(string? keyword, RefSqlPaging paging);
    }
}
