using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.AccountDomains.Entities;
using TMQ.AccountReadModels;
using TMQ.AccountRepository;
using TMQ.BaseRepositories;
using Z.Dapper.Plus;

namespace TMQ.AccountRepositorySQLImplement
{
    public class AccountSettingRepository(IDbConnectionFactory dbConnectionFactory)
    : SqlDbBaseRepository<AccountSetting>(dbConnectionFactory),
        IAccountSettingRepository, ISqlDbBaseRepository<AccountSetting>
    {
        public async Task<RAccountSetting?> GetById(string id)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@id", id, DbType.String);
                var data = await connection.QueryFirstOrDefaultAsync<RAccountSetting>("[AccountSetting_GetById]",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            });
        }

        public async Task<RAccountSetting[]> GetByUserId(string userId)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId, DbType.String);
                var data = await connection.QueryAsync<RAccountSetting>("[AccountSetting_GetByUserId]",
                    parameters, commandType: CommandType.StoredProcedure);
                return data.ToArray();
            });
        }

        public async Task AddOrChange(AccountSetting accountSetting)
        {
            await DbConnectionFactory.WithConnection(async connection =>
            {
                await connection.BulkMergeAsync(accountSetting);
                return await Task.FromResult(true);
            });
        }
    }
}
