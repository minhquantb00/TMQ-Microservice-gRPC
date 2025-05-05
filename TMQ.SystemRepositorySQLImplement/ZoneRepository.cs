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
using TMQ.EnumDefine;
using TMQ.SystemCommands.Queries;
using TMQ.SystemDomains;
using TMQ.SystemReadModels;
using TMQ.SystemRepository;
using Z.Dapper.Plus;

namespace TMQ.SystemRepositorySQLImplement
{
    public class ZoneRepository : IZoneRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public ZoneRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<RZone[]?> Search(ZoneSearchQuery query)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@DealerId", query.DealerId, DbType.String);
                parameters.Add("@Status", query.Status.AsEnumToInt(), DbType.Int32);

                var data = await connection.QueryAsync<RZone>("[Zone_Search]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RZone[]?> Gets(string? keyword, string? dealerId, StatusEnum status, RefSqlPaging paging)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Keyword", keyword, DbType.String);
                parameters.Add("@DealerId", dealerId, DbType.String);
                parameters.Add("@Status", status.AsEnumToInt(), DbType.Int32);
                parameters.Add("@OFFSET", paging.OffSet, DbType.Int32);
                parameters.Add("@FETCH", paging.PageSize, DbType.Int32);
                var data = await connection.QueryAsync<RZone>("[Zone_Gets]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                if (dataReturn.Length > 0)
                {
                    paging.TotalRow = dataReturn[0].TotalRow;
                }

                return dataReturn;
            });
        }

        public async Task<RZone[]?> GetByWebsiteId(string dealerId)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@DealerId", dealerId, DbType.String);
                var data = await connection.QueryAsync<RZone>("Zone_GetByWebsiteId", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RZone[]> AutoComplete(ZoneAutoCompleteQuery query)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Keyword", query.Keyword, DbType.String);
                parameters.Add("@DealerId", query.DealerId, DbType.String);
                parameters.Add("@Status", query.Status, DbType.Int32);

                var data = await connection.QueryAsync<RZone>("[Zone_AutoComplete]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RZone> GetById(string? id)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Id", id);
                return await connection.QueryFirstOrDefaultAsync<RZone>("[Zone_GetById]", parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        public async Task<RZone[]> GetByIds(string[]? ids)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Ids", ids.AsArrayJoin(), DbType.String);
                var data = await connection.QueryAsync<RZone>("[Zone_GetByIds]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RZone[]> GetByLockSocketId(string lockSocketId)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@LockSocketId", lockSocketId, DbType.String);
                var data = await connection.QueryAsync<RZone>("[Zone_GetByLockSocketId]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RZone[]> GetByAutoSetPositionTime()
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Status", StatusEnum.Active.AsEnumToInt(), DbType.Int32);
                var data = await connection.QueryAsync<RZone>("[Zone_GetByAutoSetPositionTime]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task Add(Zone zone)
        {
            await _dbConnectionFactory.WithConnection(async (connection) =>
            {
                connection.BulkInsert(zone);
                return await Task.FromResult(true);
            });
        }

        public async Task Change(Zone zone)
        {
            await _dbConnectionFactory.WithConnection(async (connection) =>
            {
                connection.BulkUpdate(zone);
                return await Task.FromResult(true);
            });
        }

        public async Task ChangeZoneAndZonePositions(Zone zone)
        {
            await _dbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                transaction.BulkUpdate(zone);
                var changeItems = zone.ZonePositions.Where(p => p.Status == ActiveStatusEnum.Approved).ToArray();
                transaction.BulkMerge(changeItems);
                var removeItems = zone.ZonePositions.Where(p => p.Status != ActiveStatusEnum.Approved).ToArray();
                transaction.BulkDelete(removeItems);
                return await Task.FromResult(true);
            });
        }

        public async Task Change(Zone[] zones, ZonePosition[]? zonePositionsRemove)
        {
            await _dbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                foreach (var zone in zones)
                {
                    transaction.BulkUpdate(zone);
                    var changeItems = zone.ZonePositions.Where(p => p.Status == ActiveStatusEnum.Approved).ToArray();
                    transaction.BulkMerge(changeItems);
                    var removeItems = zone.ZonePositions.Where(p => p.Status != ActiveStatusEnum.Approved).ToArray();
                    transaction.BulkDelete(removeItems);
                }

                if (zonePositionsRemove?.Length > 0)
                {
                    transaction.BulkDelete(zonePositionsRemove);
                }

                return await Task.FromResult(true);
            });
        }

        public async Task<bool> ChangePosition(Zone zone, long prevLastOrder, ZonePosition[]? zonePositionsRemove)
        {
            return await _dbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                transaction.BulkUpdate(zone);
                var changeItems = zone.ZonePositions?.Where(p => p.Status == ActiveStatusEnum.Approved).ToArray();
                if (changeItems?.Length > 0)
                {
                    transaction.BulkMerge(changeItems);
                }

                var removeItems = zone.ZonePositions?.Where(p => p.Status != ActiveStatusEnum.Approved).ToArray();
                if (removeItems?.Length > 0)
                {
                    transaction.BulkDelete(removeItems);
                }

                if (zonePositionsRemove?.Length > 0)
                {
                    transaction.BulkDelete(zonePositionsRemove);
                }
                return await Task.FromResult(true);
            });
        }

        public async Task ChangePosition(Zone[] zones)
        {
            await _dbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                foreach (var zone in zones)
                {
                    transaction.BulkUpdate(zone);
                    var changeItems = zone.ZonePositions.Where(p => p.Status == ActiveStatusEnum.Approved).ToArray();
                    transaction.BulkMerge(changeItems);
                    var removeItems = zone.ZonePositions.Where(p => p.Status != ActiveStatusEnum.Approved).ToArray();
                    transaction.BulkDelete(removeItems);
                }

                return true;
            });
        }

        public async Task<bool> ChangePosition(Zone zone,
            ZonePosition[] zonePositions,
            ZonePosition[]? zonePositionsRemove,
            long prevLastOrder)
        {
            return await _dbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@PrevLastOrder", prevLastOrder, DbType.Int64);
                parameters.Add("@LastOrder", zone.LastOrder, DbType.Int64);
                parameters.Add("@Id", zone.Id, DbType.String);
                var rowChange = await connection.ExecuteAsync("[Zone_ChangeLastOrder]", parameters,
                    transaction: transaction, commandType: CommandType.StoredProcedure);
                if (rowChange != 1) return false;
                transaction.BulkUpdate(zonePositions);
                if (zonePositionsRemove?.Length > 0)
                {
                    transaction.BulkDelete(zonePositionsRemove);
                }

                return await Task.FromResult(true);
            });
        }

        public async Task<bool> Remove(ZonePosition[]? zonePositionsRemove)
        {
            return await _dbConnectionFactory.WithConnection(async (connection, transaction) =>
            {
                if (zonePositionsRemove?.Length > 0)
                {
                    transaction.BulkDelete(zonePositionsRemove);
                }

                return await Task.FromResult(true);
            });
        }

        public async Task<RZonePosition[]> ZonePositionGetByIds(string[] ids)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Ids", ids.AsArrayJoin(), DbType.String);
                var data = await connection.QueryAsync<RZonePosition>("[ZonePosition_GetByIds]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RZonePosition[]> ZonePositionGetByZoneIdAndObjectIds(string zoneId, string[] objectIds)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ZoneId", zoneId, DbType.String);
                parameters.Add("@ObjectIds", objectIds.AsArrayJoin(), DbType.String);
                var data = await connection.QueryAsync<RZonePosition>("[ZonePosition_GetByZoneIdAndObjectIds]",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RZonePosition[]> ZonePositionGetByObjectId(string objectId, ZonePositionType type)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ObjectId", objectId, DbType.String);
                parameters.Add("@Type", type.AsEnumToInt(), DbType.String);
                var data = await connection.QueryAsync<RZonePosition>("[ZonePosition_GetByObjectId]",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RZonePosition[]> ZonePositionGetZoneId(string zoneId, RefSqlPaging paging)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ZoneId", zoneId, DbType.String);
                parameters.Add("@Status", ActiveStatusEnum.Approved.AsEnumToInt(), DbType.Int32);
                parameters.Add("@OFFSET", paging.OffSet, DbType.String);
                parameters.Add("@FETCH", paging.PageSize, DbType.String);
                var data = await connection.QueryAsync<RZonePosition>("[ZonePosition_GetByZoneId]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RZone[]?> GetByCategoryId(string dealerId, string categoryId)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CategoryIds", categoryId, DbType.String);
                parameters.Add("@DealerId", dealerId, DbType.String);
                var dataReturn = await connection.QueryAsync<RZone>("[Zone_GetByCategoryIds]", parameters,
                    commandType: CommandType.StoredProcedure);
                return dataReturn.ToArray();
            });
        }

        public async Task<RZone[]?> GetByCategoryIds(string dealerId, string[]? categoryIds)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@CategoryIds", categoryIds.AsArrayJoin(), DbType.String);
                parameters.Add("@DealerId", dealerId, DbType.String);
                var data = await connection.QueryAsync<RZone>("[Zone_GetByCategoryIds]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RZonePosition[]> ZonePositionGets(long numericalOrder, string[]? zoneIds, RefSqlPaging paging)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@NumericalOrder", numericalOrder, DbType.Int64);
                parameters.Add("@ZoneIds", zoneIds.AsArrayJoin(), DbType.String);
                parameters.Add("@OFFSET", paging.OffSet, DbType.Int32);
                parameters.Add("@FETCH", paging.PageSize, DbType.Int32);
                var data = await connection.QueryAsync<RZonePosition>("[ZonePosition_GetByZoneIdsAndNumericalOrder]",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RZonePosition[]?> ZonePositionGetCurrent(string? zoneId, ActiveStatusEnum status,
            int totalPosition, DateTime currentDate)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ZoneId", zoneId, DbType.String);
                parameters.Add("@Status", status.AsEnumToInt(), DbType.Int32);
                parameters.Add("@TotalPosition", totalPosition, DbType.Int32);
                parameters.Add("@CurrentDate", currentDate, DbType.DateTime);
                var data = await connection.QueryAsync<RZonePosition>("[ZonePosition_GetCurrent]",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RZonePosition[]?> ZonePositionGetCurrent(KeyValuePair<string, string>[] zoneAndObjectIds,
            ActiveStatusEnum status, DateTime currentDate)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ZoneAndObjectIds",
                    zoneAndObjectIds.Select(p => $"{p.Key}|{p.Value}").ToArray().AsArrayJoin(), DbType.String);
                parameters.Add("@Status", status.AsEnumToInt(), DbType.Int32);
                parameters.Add("@CurrentDate", currentDate, DbType.DateTime);
                var data = await connection.QueryAsync<RZonePosition>("[ZonePosition_GetCurrentByZoneAndObjectIds]",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RZone> GetByObjectIdAndObjectType(string dealerId, string objectId,
            ZoneObjectTypeEnum objectType,
            string mappingKey)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ObjectId", objectId, DbType.String);
                parameters.Add("@ObjectType", objectType.AsEnumToInt(), DbType.Int32);
                parameters.Add("@MappingKey", mappingKey, DbType.String);
                parameters.Add("@DealerId", dealerId, DbType.String);
                var data = await connection.QueryFirstOrDefaultAsync<RZone>("[Zone_GetByObjectIdAndObjectType]",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data;
                return dataReturn;
            });
        }

        public async Task<RZone[]> GetByObjectType(ZoneObjectTypeEnum objectType, string? dealerId)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@ObjectType", objectType.AsEnumToInt(), DbType.Int32);
                parameters.Add("@DealerId", dealerId, DbType.String);
                var data = await connection.QueryAsync<RZone>("[Zone_GetByObjectType]",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data;
                return dataReturn.ToArray();
            });
        }

        public async Task<RZone[]> GetByMobilePage(MobilePageEnum mobilePage)
        {
            throw new NotImplementedException();
        }

        public async Task<RZone[]> GetByMobilePage(MobilePageEnum mobilePage, string? dealerId)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@MobilePage", mobilePage.AsEnumToInt(), DbType.Int32);
                parameters.Add("@Status", StatusEnum.Active.AsEnumToInt(), DbType.Int32);
                parameters.Add("@DealerId", dealerId, DbType.String);
                var dataReturn = await connection.QueryAsync<RZone>("[Zone_MobilePage]", parameters,
                    commandType: CommandType.StoredProcedure);
                return dataReturn.ToArray();
            });
        }

        public async Task<RZonePosition[]?> ZonePositionGetByIsTimerOrIsPin(ActiveStatusEnum status,
            DateTime currentDate)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Status", status.AsEnumToInt(), DbType.Int32);
                parameters.Add("@CurrentDate", currentDate, DbType.DateTime);
                var data = await connection.QueryAsync<RZonePosition>("[ZonePosition_GetByIsTimerOrIsPin]",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RZonePosition[]?> ZonePositionGetByIsPinDateFinish(ActiveStatusEnum status,
            DateTime currentDate)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                var parameters = new DynamicParameters();
                parameters.Add("@Status", status.AsEnumToInt(), DbType.Int32);
                parameters.Add("@CurrentDate", currentDate, DbType.DateTime);
                var data = await connection.QueryAsync<RZonePosition>("[ZonePosition_GetByIsPinDateFinish]",
                    parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }
    }
}
