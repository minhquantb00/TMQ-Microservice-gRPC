using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseReadModels;
using TMQ.BaseRepositories;
using TMQ.Common;
using TMQ.SystemDomains;
using TMQ.SystemReadModels;
using TMQ.SystemRepository;

namespace TMQ.SystemRepositorySQLImplement
{
    public class ShardingRepository(IDbConnectionFactory dbConnectionFactory)
    : SqlDbBaseRepository<Shard>(dbConnectionFactory),
        IShardingRepository, ISqlDbBaseRepository<Shard>
    {
        private readonly IDbConnectionFactory _dbConnectionFactory = dbConnectionFactory;

        public async Task<RShard?> GetById(string id)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.String);
                var data = await connection.QueryFirstOrDefaultAsync<RShard>(
                    "[Sharding_GetById]", parameters,
                    commandType: CommandType.StoredProcedure);
                return data;
            });
        }

        public async Task<RShard[]> GetByIds(string[] ids)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Ids", ids.AsArrayJoin(), DbType.String);
                var data = await connection.QueryAsync<RShard>(
                    "[Sharding_GetByIds]", parameters,
                    commandType: CommandType.StoredProcedure);
                return data.ToArray();
            });
        }

        public async Task<RShard[]> Gets(string shardingGroupId, string? keyword, RefSqlPaging paging)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@ShardingGroupId", shardingGroupId, DbType.String);
                parameters.Add("@Keyword", keyword, DbType.String);
                parameters.Add("@OFFSET", paging.OffSet, DbType.String);
                parameters.Add("@FETCH", paging.PageSize, DbType.String);
                var data = await connection.QueryAsync<RShard>(
                    "[Sharding_Gets]", parameters,
                    commandType: CommandType.StoredProcedure);
                return data.ToArray();
            });
        }
    }
}
