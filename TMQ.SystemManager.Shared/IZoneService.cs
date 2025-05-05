using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseCommands;
using TMQ.SystemCommands.Commands;
using TMQ.SystemCommands.Events;
using TMQ.SystemCommands.Queries;
using TMQ.SystemReadModels;

namespace TMQ.SystemManager.Shared
{
    [ServiceContract]
    public interface IZoneService
    {
        [OperationContract]
        Task<BaseCommandResponse<RZone?>> GetById(ZoneGetByIdQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RZone[]>> GetByIds(ZoneGetByIdsQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RZone[]>> Search(ZoneSearchQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RZone[]>> Gets(ZoneSearchQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RZone[]>> GetsByWebsiteId(ZoneGetsByWebsiteIdQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RZone[]>> AutoComplete(ZoneAutoCompleteQuery query);

        [OperationContract]
        Task<BaseCommandResponse> Add(ZoneAddCommand command);

        [OperationContract]
        Task<BaseCommandResponse> Change(ZoneChangeCommand command);

        [OperationContract]
        Task<BaseCommandResponse> ChangeStatus(ZoneChangeStatusCommand command);

        [OperationContract]
        Task<BaseCommandResponse<RZonePositionChange[]>> ZonePositionRemove(ZonePositionRemoveCommand command);

        [OperationContract]
        Task<BaseCommandResponse<RZonePosition[]>> Sort(ZonePositionChangeCommand command);

        [OperationContract]
        Task<BaseCommandResponse> Sorts(ZonePositionChangeCommand[] commands);

        [OperationContract]
        Task<BaseCommandResponse<RZonePositionChange[]>> EventZonePositionChange(ZonePositionChangeCommand command);

        [OperationContract]
        Task<BaseCommandResponse<RZonePosition[]>> ZonePositionGetByZone(ZonePositionGetByZoneIdQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RZonePosition[]>> ZonePositionGetByIds(ZonePositionGetByIdsQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RZonePosition[]>> ZonePositionGetByObjectId(ZonePositionGetByObjectIdQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RZonePosition[]>> ZonePositionGetByZones(ZonePositionGetByZoneIdQuery[]? query);

        // Task<BaseCommandResponse<RZonePosition[]>> ZonePositionGetByZoneAndObjectIds(
        //     ZonePositionGetByZoneIdAndObjectIdsQuery? query);

        [OperationContract]
        Task<BaseCommandResponse<RZone[]>> GetByCategoryId(ZoneGetByCategoryIdQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RZone[]>> GetByCategoryIds(ZoneGetByCategoryIdsQuery query);

        [OperationContract]
        Task ZonePositionEventChange(ZonePositionChangeEvent @event);

        [OperationContract]
        Task<BaseCommandResponse> ZonePositionSyncToES(ZonePositionSyncToESCommand command);

        [OperationContract]
        Task ZonePositionSync(ZonePositionSyncToESEvent @event);

        [OperationContract]
        Task<BaseCommandResponse> ZonePositionChangeFromNews(ZonePositionChangeFromNewsCommand command);

        [OperationContract]
        Task<BaseCommandResponse> ZonePositionRemoveFromNews(ZonePositionRemoveFromNewsCommand command);

        [OperationContract]
        Task<BaseCommandResponse<RZone>> GetByObjectIdAndObjectType(ZoneGetByObjectIdAndObjectTypeQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RZone[]?>> GetByMobilePage(ZoneGetByMobilePageQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RZone[]?>> GetByObjectType(ZoneGetByObjectTypeQuery query);

        Task Process(ZoneChangeEvent @event);
        Task ProcessAutoChangePositionPublishDate();

        Task ProcessAutoRemoveCachePositionByPinDateFinish();

        // [OperationContract]
        // Task<BaseCommandResponse> Display(ZonePositionDisplayCommand command);
        Task ProcessEvent(ZonePositionHistoryEvent @event);

        [OperationContract]
        Task<BaseCommandResponse<RZonePositionHistory[]?>> ZonePositionHistoryGets(ZonePositionHistoryGetQuery query);

        Task ProcessEvent(ZonePositionPinChangeEvent @event);
    }
}
