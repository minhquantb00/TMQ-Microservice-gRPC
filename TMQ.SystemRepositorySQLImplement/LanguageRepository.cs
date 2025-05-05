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
    public class LanguageRepository : ILanguageRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public LanguageRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public Task<RLanguage[]> Gets()
        {
            return _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DeletedStatus", StatusEnum.Deleted.AsEnumToInt());
                var data = await connection.QueryAsync<RLanguage>("[Language_GetAll]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task Add(Language language)
        {
            await _dbConnectionFactory.WithConnection(async connection =>
            {
                connection.BulkInsert(language);
                return true;
            });
        }

        public async Task Update(Language language)
        {
            await _dbConnectionFactory.WithConnection(async connection =>
            {
                connection.BulkUpdate(language);
                return true;
            });
        }

        public async Task<RLanguage> GetById(string id)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                return await connection.QueryFirstOrDefaultAsync<RLanguage>("[Language_GetById]", parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        public async Task<RLanguage[]> GetByType(LanguageTypeEnum type)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Type", type);
                var data = await connection.QueryAsync<RLanguage>("[Language_GetByType]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }
    }
}
