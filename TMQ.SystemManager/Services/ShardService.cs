using TMQ.BaseApplication.Services;
using TMQ.BaseCommands;
using TMQ.BaseReadModels;
using TMQ.SystemCommands.Commands;
using TMQ.SystemCommands.Queries;
using TMQ.SystemDomains;
using TMQ.SystemManager.Shared;
using TMQ.SystemReadModels;
using TMQ.SystemRepository;

namespace TMQ.SystemManager.Services
{
    public class ShardService(
    ContextService contextService,
    ILogger<MenuService> logger,
    ICacheService cacheService,
    IShardingRepository shardingRepository,
    IShardingGroupRepository shardingGroupRepository,
    ICommonService commonService
)
    : BaseService(logger, contextService), IShardService
    {
        public async Task<BaseCommandResponse<RShardGroup[]?>> GroupGets(ShardingGroupGetsQuery query)
        {
            return await ProcessCommand<RShardGroup[]?>(async (response) =>
            {
                var paging = new RefSqlPaging(query.PageIndex, query.PageSize);
                var groups = await shardingGroupRepository.Gets(query.Keyword, paging);
                response.Data = groups;
                response.TotalRow = paging.TotalRow;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RShardGroup?>> GroupGetById(ShardingGroupGetByIdQuery query)
        {
            return await ProcessCommand<RShardGroup?>(async (response) =>
            {
                var group = await shardingGroupRepository.GetById(query.ObjectId);
                response.Data = group;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> GroupAdd(ShardGroupAddCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                var id = await commonService.GetNextCode(new GetNextCodeQuery()
                {
                    LoginUid = command.LoginUid,
                    ProcessUid = command.ProcessUid,
                    Prefix = "G",
                    IsDigit = true,
                    TypeName = typeof(RShardGroup).FullName,
                });
                ShardGroup group = new ShardGroup(command, id);
                await shardingGroupRepository.Add(group);
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> GroupChange(ShardGroupChangeCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                RShardGroup? rShardingGroup = await shardingGroupRepository.GetById(command.ObjectId);
                if (rShardingGroup == null)
                {
                    response.SetFail($"RShardingGroup not exist: {command.ObjectId}");
                    return;
                }

                ShardGroup group = new ShardGroup(rShardingGroup);
                group.Change(command);
                await shardingGroupRepository.Change(group);
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RShard[]?>> Gets(ShardingGetsQuery query)
        {
            return await ProcessCommand<RShard[]?>(async (response) =>
            {
                var paging = new RefSqlPaging(query.PageIndex, query.PageSize);
                var shardings = await shardingRepository.Gets(query.ShardingGroupId, query.Keyword, paging);
                response.Data = shardings;
                response.TotalRow = paging.TotalRow;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RShard?>> GetById(ShardingGetByIdQuery query)
        {
            return await ProcessCommand<RShard?>(async (response) =>
            {
                var sharding = await shardingRepository.GetById(query.ObjectId);
                response.Data = sharding;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> Add(ShardAddCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                var id = await commonService.GetNextCode(new GetNextCodeQuery()
                {
                    LoginUid = command.LoginUid,
                    ProcessUid = command.ProcessUid,
                    Prefix = "S",
                    IsDigit = true,
                    TypeName = typeof(RShard).FullName,
                });
                Shard sharding = new Shard(command, id);
                await shardingRepository.Add(sharding);
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> Change(ShardChangeCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                RShard? rSharding = await shardingRepository.GetById(command.ObjectId);
                if (rSharding == null)
                {
                    response.SetFail($"RSharding not exist: {command.ObjectId}");
                    return;
                }

                Shard sharding = new Shard(rSharding);
                sharding.Change(command);
                await shardingRepository.Change(sharding);
                response.SetSuccess();
            });
        }
    }
}
