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
using TMQ.SystemCommands.Queries;
using TMQ.SystemDomains;
using TMQ.SystemReadModels;
using TMQ.SystemRepository;
using Z.Dapper.Plus;

namespace TMQ.SystemRepositorySQLImplement
{
    public class MenuRepository : IMenuRepository
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public MenuRepository(IDbConnectionFactory dbConnectionFactory)
        {
            _dbConnectionFactory = dbConnectionFactory;
        }

        public async Task<RMenu[]> Gets(string dealerId)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@DealerId", dealerId, DbType.String);
                parameters.Add("@Status", StatusEnum.Active.AsEnumToInt(), DbType.Int32);
                var data = await connection.QueryAsync<RMenu>("[AdminMenu_GetByWebsiteId]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public Task<RMenu[]> Gets(MenuSearchQuery menu)
        {
            return _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Position", menu.Position.AsEnumToInt(), DbType.Int32);
                parameters.Add("@ObjectId", menu.ObjectId, DbType.String);
                var data = await connection.QueryAsync<RMenu>("[AdminMenu_Gets]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RMenu[]> GetsByDisplayPermission(MenusGetByDisplayPermissionQuery query)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                //parameters.Add("@Position", query.Position?.AsEnumToInt(), DbType.Int32);
                parameters.Add("@ObjectId", query.ObjectId, DbType.String);
                parameters.Add("@IsDisplayPermission", query.IsDisplayPermission ? 1 : 0, DbType.Int32);
                parameters.Add("@DealerId", query.DealerId, DbType.String);
                var data = await connection.QueryAsync<RMenu>("[AdminMenu_GetsByDisplayPermission]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task Add(AdminMenu menu)
        {
            await _dbConnectionFactory.WithConnection(async (connection) =>
            {
                await connection.BulkActionAsync(x => x.BulkInsert(menu));
            });
        }

        public async Task Change(AdminMenu menu)
        {
            await _dbConnectionFactory.WithConnection(async (connection) =>
            {
                await connection.BulkActionAsync(x => x.BulkUpdate(menu));
            });
        }

        public async Task Change(AdminMenu[] menus)
        {
            await _dbConnectionFactory.WithConnection(async (connection) =>
            {
                await connection.BulkActionAsync(x => x.BulkUpdate(menus));
            });
        }

        public async Task Delete(AdminMenu menu)
        {
            await _dbConnectionFactory.WithConnection(async (connection) =>
            {
                await connection.BulkActionAsync(x => x.BulkDelete(menu));
            });
        }

        public async Task<RMenu[]?> GetByPosition(MenuPosition menuPosition)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@position", menuPosition, DbType.Int32);
                var data = await connection.QueryAsync<RMenu>("[AdminMenu_GetByPositionId]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RMenu[]?> GetByPosition(MenuPosition[] menuPositions)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@Positions", menuPositions.Select(p => p.AsEnumToInt()).ToArray().AsArrayJoin(),
                    DbType.String);
                var data = await connection.QueryAsync<RMenu>("[AdminMenu_GetByPositions]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }

        public async Task<RMenu?> GetById(string id)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                return await connection.QueryFirstOrDefaultAsync<RMenu>("[AdminMenu_GetById]", parameters,
                    commandType: CommandType.StoredProcedure);
            });
        }

        public async Task<RMenu[]> GetChildrenById(string id)
        {
            return await _dbConnectionFactory.WithConnection(async connection =>
            {
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@id", id);
                var data = await connection.QueryAsync<RMenu>("[AdminMenu_GetChildrenById]", parameters,
                    commandType: CommandType.StoredProcedure);
                var dataReturn = data.ToArray();
                return dataReturn;
            });
        }
    }
}
