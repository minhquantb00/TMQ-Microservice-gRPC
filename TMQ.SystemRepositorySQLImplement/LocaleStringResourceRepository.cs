using Dapper;
using Z.Dapper.Plus;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseReadModels;
using TMQ.BaseRepositories;
using TMQ.SystemDomains;
using TMQ.SystemReadModels;
using TMQ.SystemRepository;
using TMQ.SystemCommands.Commands;

namespace TMQ.SystemRepositorySQLImplement
{
    public class LocaleStringResourceRepository : ILocaleStringResourceRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public LocaleStringResourceRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<RLocaleStringResource[]?> Gets(LocaleSearchCommand command, RefSqlPaging paging)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@lang", command.LanguageId, DbType.String);
                parameters.Add("@name", command.ResourceName, DbType.String);
                parameters.Add("@val", command.ResourceValue, DbType.String);
                parameters.Add("@OFFSET", paging.OffSet, DbType.Int32);
                parameters.Add("@FETCH", paging.PageSize, DbType.Int32);
                var data = await connection.QueryAsync<RLocaleStringResource>("[LocaleStringResource_GetByLanguage]",
                    parameters, commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                if (dataReturn?.Length > 0)
                {
                    paging.TotalRow = dataReturn[0].TotalRow;
                }

                return dataReturn;
            });
        }

        public async Task Insert(Locale command)
        {
            await _dbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                transaction.BulkInsert(command);
                return await Task.FromResult(true);
            });
        }

        public async Task Add(Locale[] command)
        {
            await _dbConnectionFactory.WithConnection(async (connection) =>
            {
                foreach (Locale item in command)
                {
                    try
                    {
                        connection.BulkInsert(command);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                return await Task.FromResult(true);
            });
        }

        public async Task AddOrChange(Locale[] command)
        {
            await _dbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                transaction.BulkMerge(command);
                return await Task.FromResult(true);
            });
        }

        public async Task Update(Locale command)
        {
            await _dbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                transaction.BulkUpdate(command);
                return await Task.FromResult(true);
            });
        }

        public async Task Delete(Locale command)
        {
            await _dbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                transaction.BulkDelete(command);
                return await Task.FromResult(true);
            });
        }

        public async Task<RLocaleStringResource[]> GetByLanguageId(string languageId)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@LanguageId", languageId, DbType.String);
                var data = await connection.QueryAsync<RLocaleStringResource>("[LocaleStringResource_GetByLanguageId]",
                    parameters, commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RLocaleStringResource> GetByDoubleKeys(string languageId, string resource)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@lang", languageId);
                parameters.Add("@resource", resource);
                return await connection.QueryFirstOrDefaultAsync<RLocaleStringResource>(
                    "[LocaleStringResource_GetByDoubleKeys]", parameters, commandType: CommandType.StoredProcedure);
            });
        }

        public async Task<RLocaleStringResource[]> GetByKeys(KeyValuePair<string, string>[] keys)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Keys", string.Join("`", keys.Select(p => $"{p.Key}|{p.Value}")), DbType.String);
                var data = await connection.QueryAsync<RLocaleStringResource>("[LocaleStringResource_GetByKeys]",
                    parameters, commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RLocaleStringResource> GetById(string id)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                return await connection.QueryFirstOrDefaultAsync<RLocaleStringResource>(
                    "[LocaleStringResource_GetById]", parameters, commandType: CommandType.StoredProcedure);
            });
        }
    }
}
