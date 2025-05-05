using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseReadModels;
using TMQ.EnumDefine;
using TMQ.SystemCommands.Queries;
using TMQ.SystemDomains;
using TMQ.SystemReadModels;

namespace TMQ.SystemRepository
{
    public interface IZoneRepository
    {
        public Task<RZone[]?> Search(ZoneSearchQuery query);
        public Task<RZone[]?> Gets(string? keyword, string? dealerId, StatusEnum status, RefSqlPaging paging);
        public Task<RZone[]?> GetByWebsiteId(string dealerId);
        public Task<RZone[]> AutoComplete(ZoneAutoCompleteQuery query);
        public Task<RZone?> GetById(string? id);
        public Task<RZone[]> GetByIds(string[]? ids);
        public Task<RZone[]> GetByLockSocketId(string lockSocketId);
        public Task<RZone[]?> GetByAutoSetPositionTime();
        public Task Add(Zone zone);
        public Task Change(Zone zone);
        Task ChangeZoneAndZonePositions(Zone zone);
        Task Change(Zone[] zone, ZonePosition[]? zonePositionsRemove);
        public Task<bool> ChangePosition(Zone zone, long prevLastOrder, ZonePosition[]? zonePositionsRemove);
        public Task ChangePosition(Zone[] zones);

        public Task<bool> ChangePosition(Zone zone,
            ZonePosition[] zonePositions,
            ZonePosition[]? zonePositionsRemove,
            long prevLastOrder);

        public Task<bool> Remove(ZonePosition[]? zonePositionsRemove);

        // public Task Change(Zone[] zones, ZonePosition[] zonePositions);
        public Task<RZonePosition[]> ZonePositionGetByIds(string[] ids);
        public Task<RZonePosition[]> ZonePositionGetByZoneIdAndObjectIds(string zoneId, string[] objectIds);
        Task<RZonePosition[]> ZonePositionGetByObjectId(string objectId, ZonePositionType type);
        public Task<RZonePosition[]> ZonePositionGetZoneId(string? zoneId, RefSqlPaging paging);
        public Task<RZone[]?> GetByCategoryId(string dealerId, string categoryId);
        public Task<RZone[]?> GetByCategoryIds(string dealerId, string[]? categoryIds);
        public Task<RZonePosition[]> ZonePositionGets(long numericalOrder, string[]? zoneIds, RefSqlPaging paging);

        public Task<RZonePosition[]?> ZonePositionGetCurrent(string? zoneId, ActiveStatusEnum status, int totalPosition,
            DateTime currentDate);

        Task<RZonePosition[]?> ZonePositionGetCurrent(KeyValuePair<string, string>[] zoneAndObjectIds,
            ActiveStatusEnum status, DateTime currentDate);

        Task<RZone> GetByObjectIdAndObjectType(string dealerId, string objectId, ZoneObjectTypeEnum objectType,
            string mappingKey);

        Task<RZone[]> GetByObjectType(ZoneObjectTypeEnum objectType, string? dealerId);
        Task<RZone[]> GetByMobilePage(MobilePageEnum mobilePage, string dealerId);

        public Task<RZonePosition[]?> ZonePositionGetByIsTimerOrIsPin(ActiveStatusEnum status, DateTime currentDate);

        Task<RZonePosition[]?> ZonePositionGetByIsPinDateFinish(ActiveStatusEnum status, DateTime currentDate);
    }
}
