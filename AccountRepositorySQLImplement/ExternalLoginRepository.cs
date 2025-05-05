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
using TMQ.Common;
using TMQ.EnumDefine;
using Z.Dapper.Plus;

namespace TMQ.AccountRepositorySQLImplement
{
    public class ExternalLoginRepository(IDbConnectionFactory dbConnectionFactory)
    : SqlDbBaseRepository<ExternalLogin>(dbConnectionFactory),
        IExternalLoginRepository, ISqlDbBaseRepository<ExternalLogin>
    {
        public async Task<RExternalLogin?> GetById(string id)
        {
            return await DbConnectionFactory.WithConnection((connection) =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.String);
                var data = connection.QueryFirstOrDefaultAsync<RExternalLogin>("ExternalLogin_GetById", parameters,
                    commandType: CommandType.StoredProcedure);
                return data;
            });
        }

        public async Task<RExternalLogin[]> Get(string userId)
        {
            return await DbConnectionFactory.WithConnection(async (connection) =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", userId, DbType.String);
                var data = await connection.QueryAsync<RExternalLogin>("ExternalLogin_GetByUserId", parameters,
                    commandType: CommandType.StoredProcedure);
                return data.ToArray();
            });
        }

        public async Task StatusChange(ExternalLogin externalLogin)
        {
            await DbConnectionFactory.WithConnection(async (connection) =>
            {
                await connection.BulkUpdateAsync(externalLogin);
            }
            );
        }

        public async Task Add(ExternalLogin externalLogin, User? user, Func<IDbConnection, IDbTransaction, User, Task>? add)
        {
            await DbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                transaction.BulkInsert(externalLogin);
                if (user != null && add != null)
                {
                    await add(connection, transaction, user);
                }

                return true;
            });
        }

        public async Task<RExternalLogin?> GetByProviderKey(string key, ExternalLoginProviderEnum loginProvider)
        {
            return await DbConnectionFactory.WithConnection(async (connection) =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@LoginProvider", loginProvider, DbType.Int32);
                parameters.Add("@ProviderKey", key.AsEmpty(), DbType.String);
                var data = await connection.QueryFirstOrDefaultAsync<RExternalLogin>("ExternalLogin_GetByProviderKey",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            });
        }
    }
}
