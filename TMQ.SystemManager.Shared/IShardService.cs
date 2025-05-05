using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseCommands;
using TMQ.SystemCommands.Commands;
using TMQ.SystemCommands.Queries;
using TMQ.SystemReadModels;

namespace TMQ.SystemManager.Shared
{
    [ServiceContract]
    public interface IShardService
    {
        [OperationContract]
        Task<BaseCommandResponse<RShardGroup[]?>> GroupGets(ShardingGroupGetsQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RShardGroup?>> GroupGetById(ShardingGroupGetByIdQuery query);

        [OperationContract]
        Task<BaseCommandResponse> GroupAdd(ShardGroupAddCommand command);

        [OperationContract]
        Task<BaseCommandResponse> GroupChange(ShardGroupChangeCommand command);

        [OperationContract]
        Task<BaseCommandResponse<RShard[]?>> Gets(ShardingGetsQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RShard?>> GetById(ShardingGetByIdQuery query);

        [OperationContract]
        Task<BaseCommandResponse> Add(ShardAddCommand command);

        [OperationContract]
        Task<BaseCommandResponse> Change(ShardChangeCommand command);

    }
}
