using TMQ.BaseApplication.Services;
using TMQ.BaseCommands;
using TMQ.Common;
using TMQ.EnumDefine;
using TMQ.SystemCommands.Commands;
using TMQ.SystemCommands.Events;
using TMQ.SystemCommands.Queries;
using TMQ.SystemDomains;
using TMQ.SystemManager.Shared;
using TMQ.SystemReadModels;
using TMQ.SystemRepository;

namespace TMQ.SystemManager.Services
{
    public class MenuService(
    ContextService contextService,
    IMenuRepository menuRepository,
    ILogger<MenuService> logger,
    ICacheService cacheService)
    : BaseService(logger, contextService), IMenuService
    {
        public async Task<BaseCommandResponse<RMenu?>> GetById(MenuGetByIdQuery query)
        {
            return await ProcessCommand<RMenu?>(async (response) =>
            {
                var menu = await menuRepository.GetById(query.Id);
                response.Data = menu;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RMenu[]>> GetsByWebsiteId(MenusGetByWebsiteIdQuery query)
        {
            return await ProcessCommand<RMenu[]>(async (response) =>
            {
                var menus = await menuRepository.Gets(query.DealerId);
                response.Data = menus;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RMenu[]>> Gets(MenuSearchQuery menu)
        {
            return await ProcessCommand<RMenu[]>(async (response) =>
            {
                var menus = await menuRepository.Gets(menu);
                response.Data = menus;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RMenu[]>> GetsByDisplayPermission(MenusGetByDisplayPermissionQuery query)
        {
            return await ProcessCommand<RMenu[]>(async (response) =>
            {
                var menus = await menuRepository.GetsByDisplayPermission(query);
                response.Data = menus;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> Add(MenuAddCommand menu)
        {
            return await ProcessCommand(async (response) =>
            {
                var input = new AdminMenu(menu);
                await menuRepository.Add(input);
                //EventAdd(input.ToEvent());
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> AddMenuPermission(MenuPermissionAddCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                var menu = new AdminMenu(command);
                await menuRepository.Add(menu);
                //EventAdd(menu.ToEvent());
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> AddToBookMark(MenuAddToBookMarkCommand menu)
        {
            return await ProcessCommand(async (response) =>
            {
                var input = new AdminMenu(menu);
                await menuRepository.Add(input);
                //EventAdd(input.ToEvent());
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> Change(MenuChangeCommand menu)
        {
            return await ProcessCommand(async (response) =>
            {
                var rLocale = await menuRepository.GetById(menu.Id);
                if (rLocale == null)
                {
                    response.SetFail("Menu not exist");
                    return;
                }

                var input = new AdminMenu(rLocale);
                input.Change(menu);
                await menuRepository.Change(input);
                //EventAdd(input.ToEvent());
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> ChangeMenuPermission(MenuPermissionChangeCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                var rMenu = await menuRepository.GetById(command.Id);
                if (rMenu == null)
                {
                    response.SetFail("Menu not exist");
                    return;
                }

                var menu = new AdminMenu(rMenu);
                menu.Change(command);
                await menuRepository.Change(menu);
                //EventAdd(menu.ToEvent());
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> Delete(MenuDeleteCommand menu)
        {
            return await ProcessCommand(async (response) =>
            {
                if (string.IsNullOrEmpty(menu.Id))
                {
                    response.SetFail($"Menu input can't null {menu.Id}!");
                    return;
                }

                var rMenu = await menuRepository.GetById(menu.Id);
                if (rMenu == null)
                {
                    response.SetFail("Menu not exist");
                    return;
                }

                var input = new AdminMenu(rMenu);
                await menuRepository.Delete(input);
                //EventAdd(input.ToEvent());
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RMenu[]?>> GetByPosition(MenuGetByPositionQuery query)
        {
            return await ProcessCommand<RMenu[]?>(async (response) =>
            {
                string prefix = "ByPosition";
                RMenu[]? menus = null;
                if (query.IsCache)
                {
                    menus = await cacheService.GetOrSetIfMissing(query.Position.AsEnumToInt().ToString(), prefix,
                        async () => await menuRepository.GetByPosition(query.Position));
                }
                else
                {
                    menus = await menuRepository.GetByPosition(query.Position);
                }

                response.Data = menus;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RMenu[]?>> GetByPositions(MenuGetByPositionsQuery query)
        {
            return await ProcessCommand<RMenu[]?>(async (response) =>
            {
                if (query.Positions is not { Length: > 0 })
                {
                    response.SetSuccess();
                    return;
                }

                query.Positions = query.Positions.Distinct().ToArray();
                RMenu[]? menus;
                if (query.IsCache)
                {
                    string prefix = "ByPosition";
                    var keys = query.Positions.Select(p => p.AsEnumToInt().ToString()).ToArray();
                    var menusFromCache = await cacheService.Get<RMenu[]>(keys, prefix);
                    MenuPosition[]? idsMissingCache = null;
                    if (menusFromCache is not { Length: > 0 })
                    {
                        idsMissingCache = query.Positions;
                    }
                    else if (menusFromCache?.Length > 0 && menusFromCache.Length < keys.Length)
                    {
                        var categoryIds = menusFromCache.SelectMany(p => p.Select(q => q.PositionId)).Distinct()
                            .ToArray();
                        idsMissingCache = query.Positions.Where(p => !categoryIds.Contains(p)).ToArray();
                    }

                    menus = menusFromCache?.SelectMany(p => p).ToArray();
                    if (idsMissingCache?.Length > 0)
                    {
                        var dataMissing = await menuRepository.GetByPosition(idsMissingCache);
                        if (dataMissing?.Length > 0)
                        {
                            await cacheService.Set(dataMissing
                                    .GroupBy(p => p.PositionId).Select(
                                        p => (p.ToArray(), p.Key.AsEnumToInt().ToString())
                                    ).ToArray()
                                , prefix);
                            menus = menus == null
                                ? dataMissing
                                : menus.Union(dataMissing).ToArray();
                        }
                    }
                }
                else
                {
                    menus = await menuRepository.GetByPosition(query.Positions);
                }

                response.Data = menus;
                response.SetSuccess();
            });
        }

        public async Task MenuChangeEventProcess(MenuChangeEvent @event)
        {
            await ProcessEvent(async () =>
            {
                string prefix = "ByPosition";
                await cacheService.Remove<RMenu[]>(@event.PositionId.AsEnumToInt().ToString(), prefix);
                //EventAdd(AdminMenu.ToCacheEvent(@event.PositionId));
            });
        }
    }
}
