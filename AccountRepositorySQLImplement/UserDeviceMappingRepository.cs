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
using TMQ.BaseReadModels;
using TMQ.BaseRepositories;
using TMQ.Common;
using TMQ.EnumDefine;
using Z.Dapper.Plus;

namespace TMQ.AccountRepositorySQLImplement
{
    public class UserDeviceMappingRepository(IDbConnectionFactory dbConnectionFactory)
    : SqlDbBaseRepository<UserDeviceMapping>(dbConnectionFactory),
        IUserDeviceMappingRepository, ISqlDbBaseRepository<UserDeviceMapping>
    {
        public async Task Add(UserDeviceMapping userDeviceMapping, UserDevice userDevice)
        {
            await DbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                await (await transaction.BulkMergeAsync(userDeviceMapping)).BulkMergeAsync(userDevice);
            }
            );
        }

        public async Task Add(UserDevice userDevice, UserDevice[] userDevicesRemove)
        {
            await DbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                await transaction.BulkMergeAsync(userDevice);
                if (userDevicesRemove.Length > 0)
                {
                    await transaction.BulkUpdateAsync(userDevicesRemove);
                }
            }
            );
        }

        public async Task RefreshToken(UserDeviceMapping userDeviceMapping, UserDeviceMapping newUserDeviceMapping,
            UserDevice userDevice)
        {
            await DbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                await transaction.BulkUpdateAsync(userDeviceMapping);
                await transaction.BulkInsertAsync(newUserDeviceMapping);
                await transaction.BulkMergeAsync(userDevice);
            }
            );
        }

        public async Task LogOut(UserDeviceMapping userDeviceMapping, UserDevice? userDevice)
        {
            await DbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                if (userDevice != null)
                {
                    await transaction.BulkUpdateAsync(userDeviceMapping);
                    await transaction.BulkUpdateAsync(userDevice);
                }
                else
                {
                    await transaction.BulkUpdateAsync(userDeviceMapping);
                }
            }
            );
        }

        public async Task<RUserDeviceMapping?> GetById(string id)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.String);
                var data = await connection.QueryFirstOrDefaultAsync<RUserDeviceMapping>("[UserDeviceMapping_GetById]",
                    parameters, commandType: CommandType.StoredProcedure);
                return data;
            });
        }

        public async Task<RUserDeviceMapping[]> GetByIds(string[] ids)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Ids", ids.AsArrayJoin(), DbType.String);
                var data = await connection.QueryAsync<RUserDeviceMapping>("[UserDeviceMapping_GetByIds]", parameters,
                    commandType: CommandType.StoredProcedure);
                return data.ToArray();
            });
        }

        public async Task UserDeviceTrusted(UserDevice userDevice)
        {
            await DbConnectionFactory.WithConnection(async (connection) => { await connection.BulkUpdateAsync(userDevice); }
            );
        }

        public async Task<RUserDevice?> UserDeviceGetById(string id)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.String);
                var data = await connection.QueryFirstOrDefaultAsync<RUserDevice>("[UserDevice_GetById]", parameters,
                    commandType: CommandType.StoredProcedure);
                return data;
            });
        }

        public async Task<RUserDevice[]> UserDeviceGetByIds(string[] ids)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Ids", ids.AsArrayJoin(), DbType.String);
                var data = await connection.QueryAsync<RUserDevice>("[UserDevice_GetBIds]", parameters,
                    commandType: CommandType.StoredProcedure);
                return data.ToArray();
            });
        }

        public async Task<RUserDevice[]> UserDeviceGetByLoginType(LoginTypeEnum loginType)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@LoginType", loginType, DbType.Int32);
                parameters.Add("@Status", StatusEnum.Active, DbType.Int32);
                var data = await connection.QueryAsync<RUserDevice>("[UserDevice_GetByLoginType]", parameters,
                    commandType: CommandType.StoredProcedure);
                return data.ToArray();
            });
        }

        public async Task<RUserDevice[]> UserDeviceGetByUserId(string userId)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", userId, DbType.String);
                parameters.Add("@Status", StatusEnum.Active, DbType.Int32);
                var data = await connection.QueryAsync<RUserDevice>("[UserDevice_GetByUserId]", parameters,
                    commandType: CommandType.StoredProcedure);
                return data.ToArray();
            });
        }

        public async Task<RUserDevice[]> UserDeviceGetByUserIds(string[] userIds)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserIds", userIds.AsArrayJoin(), DbType.String);
                var data = await connection.QueryAsync<RUserDevice>("[UserDevice_GetByUserIds]", parameters,
                    commandType: CommandType.StoredProcedure);
                return data.ToArray();
            });
        }


        public async Task AddOrUpdateUserDeviceLoginNotify(UserDeviceLoginNotify userDevice)
        {
            await DbConnectionFactory.WithConnection(async (connection) => { await connection.BulkMergeAsync(userDevice); }
            );
        }

        public async Task<RUserDeviceLoginNotify?> UserDeviceLoginNotifyGetById(string id)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.String);
                var data = await connection.QueryFirstOrDefaultAsync<RUserDeviceLoginNotify>(
                    "[UserDeviceLoginNotify_GetById]", parameters, commandType: CommandType.StoredProcedure);
                return data;
            });
        }

        public async Task<RUserDevice[]> UserDeviceGetByFCMToken(string fCMToken)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@FCMToken", fCMToken, DbType.String);
                var data = await connection.QueryAsync<RUserDevice>("[UserDevice_GetByFCMToken]", parameters,
                    commandType: CommandType.StoredProcedure);
                return data.ToArray();
            });
        }

        public async Task UserDeviceChange(UserDevice userDevice)
        {
            await DbConnectionFactory.WithConnection(async (connection) => { await connection.BulkUpdateAsync(userDevice); }
            );
        }

        public async Task<RUserDevice[]?> UserDeviceGetByUserId(string userId, int loginType, string clientId,
            RefSqlPaging paging)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserId", userId, DbType.String);
                parameters.Add("@Status", StatusEnum.Active, DbType.Int32);
                parameters.Add("@LoginType", loginType, DbType.Int32);
                parameters.Add("@ClientId", clientId, DbType.String);
                parameters.Add("@PageIndex", paging.PageIndex, DbType.Int32);
                parameters.Add("@PageSize", paging.PageSize, DbType.Int32);
                var data = await connection.QueryAsync<RUserDevice>("[UserDevice_GetByUserIdPaging]", parameters,
                    commandType: CommandType.StoredProcedure);
                var result = data.ToArray();
                if (result?.Length > 0)
                {
                    paging.TotalRow = result[0].TotalRow;
                }

                return result;
            });
        }
    }
}
