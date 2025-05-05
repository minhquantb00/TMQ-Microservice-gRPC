using AccountRepositorySQLImplement;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.AccountDomains.Entities;
using TMQ.AccountReadModels;
using TMQ.BaseReadModels;
using TMQ.BaseRepositories;
using TMQ.Common;
using TMQ.EnumDefine;
using Z.Dapper.Plus;

namespace TMQ.AccountRepositorySQLImplement
{
    public class UserRepository(IDbConnectionFactory dbConnectionFactory)
    : SqlDbBaseRepository<User>(dbConnectionFactory),
        IUserRepository, ISqlDbBaseRepository<User>
    {
        public async Task<RUser?> Get(string id)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", id, DbType.String);
                var data = await connection.QueryFirstOrDefaultAsync<RUser>("[Users_GetById]", parameters,
                    commandType: CommandType.StoredProcedure);
                return data;
            });
        }

        public async Task<RUser[]> Gets(string[] ids)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Ids", ids.AsArrayJoin(), DbType.String);
                var data = await connection.QueryAsync<RUser>("[Users_GetByIds]", parameters,
                    commandType: CommandType.StoredProcedure);
                var result = data.ToArray();
                return result;
            });
        }

        public async Task<RUser?> GetByUserNameOrEmailOrPhoneNumber(string keyword)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Key", keyword, DbType.String);
                var data = await connection.QueryFirstOrDefaultAsync<RUser>("[Users_GetByKey]", parameters,
                    commandType: CommandType.StoredProcedure);
                return data;
            });
        }

        public async Task<RUser?> GetByUserName(string userName)
        {
            return await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@UserName", userName, DbType.String);
                var data = await connection.QueryFirstOrDefaultAsync<RUser>("[Users_GetByUserName]", parameters,
                    commandType: CommandType.StoredProcedure);
                return data;
            });
        }

        public async Task Add(IDbConnection dbConnection, IDbTransaction dbTransaction, User user)
        {
            if (!string.IsNullOrEmpty(user.Code))
            {
                DynamicParameters parametersCheckUser = new DynamicParameters();
                parametersCheckUser.Add("@Key", user.Code, DbType.String);
                var userCheck = await dbConnection.QueryFirstOrDefaultAsync<RUser>("[Users_GetByKey]",
                    parametersCheckUser, commandType: CommandType.StoredProcedure, transaction: dbTransaction);
                if (userCheck != null)
                {
                    throw new Exception("USER_EXISTED");
                }
            }

            if (!string.IsNullOrEmpty(user.Email))
            {
                DynamicParameters parametersCheckUser = new DynamicParameters();
                parametersCheckUser.Add("@Key", user.Email, DbType.String);
                var userCheck = await dbConnection.QueryFirstOrDefaultAsync<RUser>("[Users_GetByKey]",
                    parametersCheckUser, commandType: CommandType.StoredProcedure, transaction: dbTransaction);
                if (userCheck != null)
                {
                    throw new Exception("USER_EXISTED");
                }
            }

            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                DynamicParameters parametersCheckUser = new DynamicParameters();
                parametersCheckUser.Add("@Key", user.PhoneNumber, DbType.String);
                var userCheck = await dbConnection.QueryFirstOrDefaultAsync<RUser>("[Users_GetByKey]",
                    parametersCheckUser, commandType: CommandType.StoredProcedure, transaction: dbTransaction);
                if (userCheck != null)
                {
                    throw new Exception("USER_EXISTED");
                }
            }

            await dbTransaction.BulkInsertAsync(user);
        }

        public async Task PasswordChange(User user)
        {
            await DbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", user.Id, DbType.String);
                parameters.Add("@UpdatedDate", user.UpdatedDate, DbType.DateTime);
                parameters.Add("@UpdatedDateUtc", user.UpdatedDateUtc, DbType.DateTime);
                parameters.Add("@UpdatedUid", user.UpdatedUid, DbType.String);
                parameters.Add("@LoginUid", user.LoginUid, DbType.String);
                parameters.Add("@PasswordHash", user.Password, DbType.String);
                parameters.Add("@PasswordSalt", user.PasswordSalt, DbType.String);
                var data = await connection.ExecuteAsync("[Users_PasswordChange]", parameters,
                    commandType: CommandType.StoredProcedure);
                return data;
            });
        }

        public async Task TwoFactorChange(User user)
        {
            await DbConnectionFactory.WithConnection(async (connection) => { await connection.BulkUpdateAsync(user); });
        }

        public async Task PhoneNumberChange(User user)
        {
            await DbConnectionFactory.WithConnection(async (connection) => { await connection.BulkUpdateAsync(user); });
        }


    }
}
