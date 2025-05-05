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
    public interface IMenuService
    {
        [OperationContract]
        Task<BaseCommandResponse<RMenu?>> GetById(MenuGetByIdQuery query);
        [OperationContract]
        Task<BaseCommandResponse<RMenu[]>> GetsByWebsiteId(MenusGetByWebsiteIdQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RMenu[]>> Gets(MenuSearchQuery menu);

        [OperationContract]
        Task<BaseCommandResponse<RMenu[]>> GetsByDisplayPermission(MenusGetByDisplayPermissionQuery query);

        [OperationContract]
        Task<BaseCommandResponse> Add(MenuAddCommand menu);

        [OperationContract]
        Task<BaseCommandResponse> AddMenuPermission(MenuPermissionAddCommand command);

        [OperationContract]
        Task<BaseCommandResponse> AddToBookMark(MenuAddToBookMarkCommand menu);

        [OperationContract]
        Task<BaseCommandResponse> Change(MenuChangeCommand menu);

        [OperationContract]
        Task<BaseCommandResponse> ChangeMenuPermission(MenuPermissionChangeCommand command);

        // [OperationContract]
        // Task<BaseCommandResponse> ChangeDisplayPermission(MenuChangeDisplayPermissionCommand command);

        [OperationContract]
        Task<BaseCommandResponse> Delete(MenuDeleteCommand menu);

        [OperationContract]
        Task<BaseCommandResponse<RMenu[]?>> GetByPosition(MenuGetByPositionQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RMenu[]?>> GetByPositions(MenuGetByPositionsQuery query);

        Task MenuChangeEventProcess(MenuChangeEvent @event);
    }
}
