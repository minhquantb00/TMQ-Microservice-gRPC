using Dapper;
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
using Z.Dapper.Plus;

namespace TMQ.SystemRepositorySQLImplement
{
    public class ZonePositionHistoryRepository : IZonePositionHistoryRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public ZonePositionHistoryRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task Add(ZonePositionHistory[] histories)
        {
            await _dbConnectionFactory.WithConnection(async connection =>
            {
                var data = connection.BulkInsert(histories);
                return await Task.FromResult(data);
            });
        }

        public async Task<RZonePositionHistory[]> Gets(string zoneId, RefSqlPaging sqlPaging, string changeId,
            bool isDetail)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ZoneId", zoneId, DbType.String);
                parameters.Add("@ChangeId", changeId, DbType.String);
                if (!isDetail)
                {
                    parameters.Add("@Position", 0, DbType.Int32);
                }

                parameters.Add("@OFFSET", sqlPaging.OffSet, DbType.Int32);
                parameters.Add("@FETCH", sqlPaging.PageSize, DbType.Int32);
                var data = await connection.QueryAsync<RZonePositionHistory>("[ZonePositionHistory_Gets]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                if (dataReturn.Length > 0)
                {
                    sqlPaging.TotalRow = dataReturn[0].TotalRow;
                }

                return dataReturn;
            });
        }
    }
}
