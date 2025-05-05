using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseRepositories;
using TMQ.Common;
using TMQ.EnumDefine;
using TMQ.SystemDomains;
using TMQ.SystemReadModels;
using TMQ.SystemRepository;
using Z.Dapper.Plus;

namespace TMQ.SystemRepositorySQLImplement
{
    public class ConfigRepository : IConfigRepository
    {
        protected readonly IDbConnectionFactory DbConnectionFactory;

        public ConfigRepository(IDbConnectionFactory dbConnectionFactory)
        {
            DbConnectionFactory = dbConnectionFactory;
        }

        public async Task<RConfig?> GetConfigByKey(string key)
        {
            return await DbConnectionFactory.WithConnection((connection) =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Key", key, DbType.String);
                return connection.QueryFirstOrDefaultAsync<RConfig>("Config_GetById", parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        public async Task Add(ConfigDomain config)
        {
            await DbConnectionFactory.WithConnection(async (connection) =>
            {
                await connection.BulkActionAsync(
                    _ => { connection.BulkInsert(config); }
                );
            });
        }

        public async Task<bool> Change(ConfigDomain config)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                connection.BulkUpdate(config);
                return await Task.FromResult(true);
            });
        }

        public async Task<RConfig[]> Gets(ConfigTypeEnum type)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Type", type.AsEnumToInt(), DbType.Int32);
                var data = await connection.QueryAsync<RConfig>("[Config_GetByType]", parameters,
                    commandType: CommandType.StoredProcedure);
                var result = data.ToArray();
                return result;
            });
        }
    }
}
