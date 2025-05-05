using TMQ.BaseApplication.Services;
using TMQ.BaseCommands;
using TMQ.BaseEvents;
using TMQ.BaseReadModels;
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
    public class ZoneService(
    ContextService contextService,
    ILogger<ZoneService> logger,
    IZoneRepository zoneRepository,
    ICacheService cacheService,
    IZonePositionHistoryRepository zonePositionHistoryRepository)
    : BaseService(logger, contextService), IZoneService
    {
        public async Task<BaseCommandResponse<RZone?>> GetById(ZoneGetByIdQuery query)
        {
            return await ProcessCommand<RZone?>(async (response) =>
            {
                RZone? zone;
                if (query.IsCache)
                {
                    zone = await cacheService.GetOrSetIfMissing(query.Id,
                        async () => await zoneRepository.GetById(query.Id));
                }
                else
                {
                    zone = await zoneRepository.GetById(query.Id);
                }

                response.Data = zone;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZone[]>> GetByIds(ZoneGetByIdsQuery query)
        {
            return await ProcessCommand<RZone[]>(async (response) =>
            {
                if (query.Ids is not { Length: > 0 })
                {
                    response.SetSuccess();
                    return;
                }

                RZone[]? zones;
                if (query.IsCache)
                {
                    zones = await cacheService.Get<RZone>(query.Ids);
                    string[]? idsMissingCache = null;
                    if (zones is not { Length: > 0 })
                    {
                        idsMissingCache = query.Ids;
                    }
                    else if (zones?.Length > 0 && zones.Length < query.Ids.Length)
                    {
                        idsMissingCache = query.Ids.Where(p => zones.All(q => q.Id != p)).ToArray();
                    }

                    if (idsMissingCache?.Length > 0)
                    {
                        var dataMissing = await zoneRepository.GetByIds(idsMissingCache);
                        if (dataMissing?.Length > 0)
                        {
                            await cacheService.Set(dataMissing.Select(p => (p, p.Id)).ToArray());
                            zones = zones == null ? dataMissing : zones.Union(dataMissing).ToArray();
                        }
                    }
                }
                else
                {
                    zones = await zoneRepository.GetByIds(query.Ids);
                }

                response.Data = zones;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZone[]>> Search(ZoneSearchQuery query)
        {
            return await ProcessCommand<RZone[]>(async (response) =>
            {
                var zones = await zoneRepository.Search(query);
                response.Data = zones;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZone[]>> Gets(ZoneSearchQuery query)
        {
            return await ProcessCommand<RZone[]>(async (response) =>
            {
                var paging = new RefSqlPaging(query.PageIndex, query.PageSize);
                var zones = await zoneRepository.Gets(query.Keyword, query.DealerId, query.Status, paging);
                response.TotalRow = paging.TotalRow;
                response.Data = zones;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZone[]>> GetsByWebsiteId(ZoneGetsByWebsiteIdQuery query)
        {
            return await ProcessCommand<RZone[]>(async (response) =>
            {
                var zones =
                    await cacheService.GetOrSetIfMissing(query.DealerId,
                        async () =>
                        {
                            var data = await zoneRepository.GetByWebsiteId(query.DealerId);
                            return data?.Where(p => p.Status == StatusEnum.Active).ToArray();
                        });
                response.Data = zones;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZone[]>> AutoComplete(ZoneAutoCompleteQuery query)
        {
            return await ProcessCommand<RZone[]>(async (response) =>
            {
                var zones = await zoneRepository.AutoComplete(query);
                response.Data = zones;
                response.SetSuccess();
            });
        }

        private void SendEvent(Zone? zone, Zone? zoneOld)
        {
            if (zoneOld != null && zoneOld.CategoryId != zone?.CategoryId)
            {
                EventAdd(zoneOld.ToEvent());
            }

            if (zoneOld != null)
            {
                EventAdd(zoneOld.ToCacheEvent());
            }

            if (zone != null)
            {
                EventAdd(zone.ToEvent());
                EventAdd(zone.ToCacheEvent());
            }
        }

        private void SendZonePositionEvent(Zone zone)
        {
            EventAdd(zone.ToZonePositionChangeEvent());
        }

        public async Task<BaseCommandResponse> Add(ZoneAddCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                Zone zone = new Zone(command);
                await zoneRepository.Add(zone);
                SendEvent(zone, null);
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> Change(ZoneChangeCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                RZone? rZone = await zoneRepository.GetById(command.Id);
                if (rZone == null)
                {
                    response.SetFail("ZONE.CHANGE.NOT.EXIST");
                    return;
                }

                Zone zone = new Zone(rZone);
                zone.Change(command);
                await zoneRepository.Change(zone);
                Zone zoneOld = new Zone(rZone);
                SendEvent(zone, zoneOld);
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> ChangeStatus(ZoneChangeStatusCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                RZone? rZone = await zoneRepository.GetById(command.Id);
                if (rZone == null)
                {
                    response.SetFail("ZONE.CHANGE.NOT.EXIST");
                    return;
                }

                Zone zone = new Zone(rZone);
                zone.ChangeStatus(command);
                await zoneRepository.Change(zone);
                Zone zoneOld = new Zone(rZone);
                SendEvent(zone, zoneOld);
                response.SetSuccess();
            });
        }

        public static bool OverlappingPeriods(DateTime? aStart, DateTime? aEnd,
            DateTime? bStart, DateTime? bEnd)
        {
            if (aStart.HasValue && aEnd.HasValue && aStart > aEnd)
                throw new ArgumentException("A start can not be after its end.");

            if (bStart.HasValue && bEnd.HasValue && bStart > bEnd)
                throw new ArgumentException("B start can not be after its end.");

            return !((aEnd < bStart && aStart < bStart) ||
                     (bEnd < aStart && bStart < aStart));
        }

        public async Task<BaseCommandResponse<RZonePositionChange[]>> ZonePositionRemove(
            ZonePositionRemoveCommand command)
        {
            return await ProcessCommand<RZonePositionChange[]>(async (response) =>
            {
                if (string.IsNullOrEmpty(command.ZoneId))
                {
                    response.SetFail("ZONE.NOT.EXIST");
                    return;
                }

                if (command.ZonePositionIds is not { Length: > 0 })
                {
                    response.SetFail("zone.item.not.null.or.empty");
                    return;
                }

                RZone? rZone = await zoneRepository.GetById(command.ZoneId);
                if (rZone == null)
                {
                    response.SetFail("ZONE.NOT.EXIST");
                    return;
                }

                RZonePosition[]? rZonePositions = await zoneRepository.ZonePositionGetByIds(command.ZonePositionIds);
                if (rZonePositions is not { Length: > 0 })
                {
                    response.SetFail("ZONE.POSITION.NOT.EXIST");
                    return;
                }

                List<ZonePosition> zonePositions = [];
                foreach (var rZonePosition in rZonePositions)
                {
                    ZonePosition zonePosition = new ZonePosition(rZonePosition);
                    zonePosition.Remove(command);
                    zonePositions.Add(zonePosition);
                }

                if (zonePositions.Count > 0)
                {
                    await zoneRepository.Remove(zonePositions.ToArray());
                }

                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZonePosition[]>> Sort(ZonePositionChangeCommand command)
        {
            return await ProcessCommand<RZonePosition[]>(async (response) =>
            {
                if (string.IsNullOrEmpty(command.ZoneId))
                {
                    response.SetFail("ZONE.CHANGE.NOT.EXIST");
                    return;
                }

                if (command.Items is not { Length: > 0 })
                {
                    response.SetFail("zone.item.not.null.or.empty");
                    return;
                }

                RZone? rZone = await zoneRepository.GetById(command.ZoneId);
                if (rZone == null)
                {
                    response.SetFail("ZONE.CHANGE.NOT.EXIST");
                    return;
                }

                var itemsIsPinGroup = command.Items
                    .Where(p => p is { BeginDateTime: not null, EndDateTime: not null })
                    .GroupBy(p => p.Position)
                    .ToDictionary(p => p.Key, p => p.ToArray()).ToArray();
                if (itemsIsPinGroup?.Length > 0)
                {
                    foreach (var itemsIsPin in itemsIsPinGroup)
                    {
                        foreach (var itemValidate in itemsIsPin.Value)
                        {
                            foreach (var item in itemsIsPin.Value)
                            {
                                if (itemValidate.Priority == item.Priority)
                                {
                                    continue;
                                }

                                bool validateDate = OverlappingPeriods(itemValidate.BeginDateTime,
                                    itemValidate.EndDateTime,
                                    item.BeginDateTime, item.EndDateTime);
                                if (validateDate)
                                {
                                    response.SetFail("zone.item.position");
                                    response.SetFail((itemValidate.Position + 1).ToString());
                                    response.SetFail("zone.item.position.date.overlap");
                                    return;
                                }
                            }
                        }
                    }
                }

                RZonePosition[]? rZonePositions = await zoneRepository.ZonePositionGetCurrent(rZone.Id,
                    ActiveStatusEnum.Approved, rZone.TotalPosition, DateTime.Now);
                Zone zone = new Zone(rZone, rZonePositions);
                if (command.DisplayType == ZonePositionDisplayTypeEnum.Active)
                {
                    // Kiếm tra xem có bài nào PIN thay đổi thời gian không?
                    var zonePositionPinIds = zone.ZonePositions?.Where(p => p.IsPin).Select(p => p.Id).ToArray();
                    if (zonePositionPinIds?.Length > 0)
                    {
                        var zonePositionsPinChange = command.Items.Where(p =>
                                zonePositionPinIds.Contains(p.Id) && p.BeginDateTime.HasValue &&
                                p.BeginDateTime > DateTime.Now)
                            .ToArray();
                        if (zonePositionsPinChange.Length > 0)
                        {
                            ZonePositionChangeCommand pinCommand = new ZonePositionChangeCommand()
                            {
                                DisplayType = ZonePositionDisplayTypeEnum.Pin,
                                DealerId = command.DealerId,
                                ObjectId = command.ObjectId,
                                ZoneId = command.ZoneId,
                                LoginUid = command.LoginUid,
                                ProcessDate = command.ProcessDate,
                                LastOrder = command.LastOrder,
                                IsSave = command.IsSave,
                                ProcessUid = command.ProcessUid,
                                ByPassCheckLock = command.ByPassCheckLock,
                                Items = zonePositionsPinChange.Select(p => new ZonePositionItemChangeCommand()
                                {
                                    Position = p.Position,
                                    Priority = p.Priority,
                                    Status = p.Status,
                                    DealerId = p.DealerId,
                                    Id = p.Id,
                                    Type = p.Type,
                                    LoginUid = p.LoginUid,
                                    ProcessUid = p.ProcessUid,
                                    ObjectId = p.ObjectId,
                                    ProcessDate = p.ProcessDate,
                                    EndDateTime = p.EndDateTime,
                                    BeginDateTime = p.BeginDateTime,
                                }).ToArray()
                            };
                            zone.SortTimer(pinCommand);
                            command.Items = command.Items.Where(p => zonePositionsPinChange.All(q => q.Id != p.Id))
                                .ToArray();
                            if (command.IsSave == true)
                            {
                                await zoneRepository.ChangePosition(zone, zone.LastOrder, null);
                                rZonePositions = await zoneRepository.ZonePositionGetCurrent(rZone.Id,
                                    ActiveStatusEnum.Approved, rZone.TotalPosition, DateTime.Now);
                                zone = new Zone(rZone, rZonePositions);
                            }
                        }
                    }
                }

                bool isChange = false;
                if (command.Items?.Length > 0)
                {
                    switch (command.DisplayType)
                    {
                        case ZonePositionDisplayTypeEnum.Active:
                            zone.SortActive(command);
                            break;
                        case ZonePositionDisplayTypeEnum.Timer:
                        case ZonePositionDisplayTypeEnum.Pin:
                            zone.SortTimer(command);
                            break;
                        default:
                            response.SetFail("DisplayType invalid");
                            return;
                    }

                    isChange = true;
                }

                if (command.IsSave == true)
                {
                    if (isChange)
                    {
                        await zoneRepository.ChangePosition(zone, zone.LastOrder, null);
                    }

                    if (command.DisplayType is ZonePositionDisplayTypeEnum.Timer or ZonePositionDisplayTypeEnum.Pin)
                    {
                        ZonePositionPinChangeEvent zonePositionPinChangeEvent = new ZonePositionPinChangeEvent()
                        {
                            ZoneId = zone.Id
                        };
                        EventAdd(zonePositionPinChangeEvent);
                    }

                    SendEvent(zone, null);
                    SendZonePositionEvent(zone);
                    AddHistoryEvent(command.ProcessUid, command.ProcessDate, zone.Id, rZonePositions,
                        zone.ZonePositions?.ToArray(), false);
                }

                response.Data = zone.ZonePositions?.Select(p => p.ToRZonePosition(zone.DealerId)).ToArray();
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> Sorts(ZonePositionChangeCommand[] commands)
        {
            return await ProcessCommand(async (response) =>
            {
                if (commands is not { Length: > 0 })
                {
                    return;
                }

                List<Zone> zones = new List<Zone>();
                Dictionary<string, RZonePosition[]> zonePositionsByZones = new Dictionary<string, RZonePosition[]>();
                foreach (var command in commands)
                {
                    if (string.IsNullOrEmpty(command?.ZoneId))
                    {
                        response.SetFail("ZONE.CHANGE.NOT.EXIST");
                        return;
                    }

                    if (command?.Items == null || command?.Items.Length <= 0)
                    {
                        response.SetFail("zone.item.not.null.or.empty");
                        return;
                    }

                    RZone? rZone = await zoneRepository.GetById(command!.ZoneId);
                    if (rZone == null)
                    {
                        response.SetFail("ZONE.CHANGE.NOT.EXIST");
                        return;
                    }

                    var itemsIsPinGroup = command.Items
                        .Where(p => p is { BeginDateTime: not null, EndDateTime: not null })
                        .GroupBy(p => p.Position)
                        .ToDictionary(p => p.Key, p => p.ToArray()).ToArray();
                    if (itemsIsPinGroup?.Length > 0)
                    {
                        foreach (var itemsIsPin in itemsIsPinGroup)
                        {
                            foreach (var itemValidate in itemsIsPin.Value)
                            {
                                foreach (var item in itemsIsPin.Value)
                                {
                                    if (itemValidate.Priority == item.Priority)
                                    {
                                        continue;
                                    }

                                    bool validateDate = OverlappingPeriods(itemValidate.BeginDateTime,
                                        itemValidate.EndDateTime,
                                        item.BeginDateTime, item.EndDateTime);
                                    if (validateDate)
                                    {
                                        response.SetFail("zone.item.position");
                                        response.SetFail((itemValidate.Position + 1).ToString());
                                        response.SetFail("zone.item.position.date.overlap");
                                        return;
                                    }
                                }
                            }
                        }
                    }

                    RZonePosition[]? rZonePositions = await zoneRepository.ZonePositionGetCurrent(rZone.Id,
                        ActiveStatusEnum.Approved, rZone.TotalPosition, DateTime.Now);
                    if (command.Items.Length == 1)
                    {
                        var commandItem = command.Items[0];
                        if (commandItem.Status)
                        {
                            var zonePositionCheck =
                                rZonePositions?.FirstOrDefault(p =>
                                    p.ObjectId.Length > 0 && commandItem.ObjectId.Length > 0 &&
                                    p.ObjectId == commandItem.ObjectId && p.BeginDate == commandItem.BeginDateTime) ??
                                rZonePositions?.FirstOrDefault(p =>
                                    p.Id == commandItem.Id && p.BeginDate == commandItem.BeginDateTime);
                            if (
                                zonePositionCheck != null &&
                                zonePositionCheck.Position == commandItem.Position &&
                                zonePositionCheck.BeginDate == commandItem.BeginDateTime &&
                                zonePositionCheck.EndDate == commandItem.EndDateTime
                            )
                            {
                                continue;
                            }
                        }
                    }

                    if (rZonePositions?.Length > 0)
                    {
                        zonePositionsByZones.TryAdd(rZone.Id, rZonePositions);
                    }

                    Zone zone = new Zone(rZone, rZonePositions);
                    switch (command.DisplayType)
                    {
                        case ZonePositionDisplayTypeEnum.Active:
                            zone.SortActive(command);
                            break;
                        case ZonePositionDisplayTypeEnum.Timer:
                        case ZonePositionDisplayTypeEnum.Pin:
                            zone.SortTimer(command);
                            break;
                        default:
                            response.SetFail("DisplayType invalid");
                            return;
                    }

                    if (command.IsSave == true)
                    {
                        zones.Add(zone);
                    }
                }

                if (zones.Count > 0)
                {
                    await zoneRepository.ChangePosition(zones.ToArray());
                    foreach (var zone in zones)
                    {
                        zonePositionsByZones.TryGetValue(zone.Id, out RZonePosition[]? zonePositions);
                        AddHistoryEvent(commands[0].ProcessUid, commands[0].ProcessDate, zone.Id, zonePositions,
                            zone.ZonePositions.ToArray(), false);
                        var command = commands.FirstOrDefault(p =>
                            p.ZoneId == zone.Id && p.DisplayType == ZonePositionDisplayTypeEnum.Active);
                        if (command == null)
                        {
                            continue;
                        }

                        SendEvent(zone, null);
                        SendZonePositionEvent(zone);
                    }
                }

                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZonePositionChange[]>> EventZonePositionChange(
            ZonePositionChangeCommand command)
        {
            return await ProcessCommand<RZonePositionChange[]>(async (response) =>
            {
                if (string.IsNullOrEmpty(command?.ZoneId))
                {
                    response.SetFail("ZONE.CHANGE.NOT.EXIST");
                    return;
                }

                if (command?.Items == null || command?.Items.Length <= 0)
                {
                    response.SetFail("zone.item.not.null.or.empty");
                    return;
                }

                RZone? rZone = await zoneRepository.GetById(command.ZoneId);
                if (rZone == null)
                {
                    response.SetFail("ZONE.CHANGE.NOT.EXIST");
                    return;
                }

                RZonePosition[]? rZonePositions = await zoneRepository.ZonePositionGetCurrent(rZone.Id,
                    ActiveStatusEnum.Approved, rZone.TotalPosition, DateTime.Now);
                Zone zone = new Zone(rZone, rZonePositions);
                zone.EventZonePositionsChange(command);
                await zoneRepository.ChangeZoneAndZonePositions(zone);
                SendEvent(zone, null);
                SendZonePositionEvent(zone);
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZonePosition[]>> ZonePositionGetByZone(
            ZonePositionGetByZoneIdQuery query)
        {
            return await ProcessCommand<RZonePosition[]>(async (response) =>
            {
                if (string.IsNullOrEmpty(query.ZoneId))
                {
                    response.Data = null;
                    response.SetSuccess();
                }

                RZonePosition[]? rZonePositions;
                if (query.IsCache)
                {
                    RCZonePosition? rcZonePosition = await cacheService.GetOrSetIfMissing(query.ZoneId,
                        async () =>
                        {
                            var data = await zoneRepository.ZonePositionGetCurrent(query.ZoneId,
                                ActiveStatusEnum.Approved,
                                query.TotalPosition, DateTime.Now);
                            return new RCZonePosition()
                            {
                                Items = data,
                                ZoneId = query.ZoneId
                            };
                        });
                    rZonePositions = rcZonePosition?.Items;
                }
                else
                {
                    rZonePositions = await zoneRepository.ZonePositionGetCurrent(query.ZoneId,
                        ActiveStatusEnum.Approved,
                        query.TotalPosition, DateTime.Now);
                }

                response.Data = rZonePositions?.Where(p => p.Position >= 0).ToArray();
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZonePosition[]>> ZonePositionGetByIds(ZonePositionGetByIdsQuery query)
        {
            return await ProcessCommand<RZonePosition[]>(async (response) =>
            {
                if (query?.Ids is not { Length: > 0 })
                {
                    response.SetSuccess();
                    return;
                }

                response.Data = await zoneRepository.ZonePositionGetByIds(query.Ids);
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZonePosition[]>> ZonePositionGetByObjectId(
            ZonePositionGetByObjectIdQuery query)
        {
            return await ProcessCommand<RZonePosition[]>(async (response) =>
            {
                response.Data = await zoneRepository.ZonePositionGetByObjectId(query.ObjectId, query.Type);
                response.Data = response.Data?.Where(p => p.Status == ActiveStatusEnum.Approved).ToArray();
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZonePosition[]>> ZonePositionGetByZones(
            ZonePositionGetByZoneIdQuery?[]? query)
        {
            return await ProcessCommand<RZonePosition[]>(async (response) =>
            {
                if (query == null || query?.Length <= 0)
                {
                    response.SetSuccess();
                    return;
                }

                query = query?.Where(p => p?.ZoneId?.Length > 0).ToArray();
                if (query == null || query?.Length <= 0)
                {
                    response.SetSuccess();
                    return;
                }

                List<RZonePosition> zonePositions = new List<RZonePosition>();
                foreach (var item in query!)
                {
                    if (item == null)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(item.ZoneId))
                    {
                        continue;
                    }

                    var data = await ZonePositionGetByZone(item);
                    if (data is { Status: true, Data.Length: > 0 })
                    {
                        zonePositions.AddRange(data.Data);
                    }
                }

                response.Data = zonePositions.ToArray();
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZone[]>> GetByCategoryId(ZoneGetByCategoryIdQuery query)
        {
            return await ProcessCommand<RZone[]>(async (response) =>
            {
                RZone[]? zones;
                if (query.IsCache)
                {
                    var zonesAll = await cacheService.GetOrSetIfMissing(query.DealerId, string.Empty,
                        async () =>
                        {
                            var data = await zoneRepository.Search(new ZoneSearchQuery()
                            {
                                Status = StatusEnum.Active,
                                DealerId = query.DealerId
                            });
                            return data;
                        });
                    zones = zonesAll?.Where(p => p.CategoryId == query.CategoryId).ToArray();
                }
                else
                {
                    zones = await zoneRepository.GetByCategoryId(query.DealerId, query.CategoryId);
                }

                if (!query.AllowNull)
                {
                    if (zones == null)
                    {
                        response.SetFail("ZONE.CHANGE.NOT.EXIST");
                        return;
                    }
                }

                response.Data = zones;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZone[]>> GetByCategoryIds(ZoneGetByCategoryIdsQuery query)
        {
            return await ProcessCommand<RZone[]>(async (response) =>
            {
                if (query.CategoryIds == null || query.CategoryIds.Length <= 0)
                {
                    response.SetSuccess();
                    return;
                }

                RZone[]? zones;
                if (query.IsCache)
                {
                    zones = await cacheService.GetOrSetIfMissing(
                        query.CategoryIds.OrderBy(p => p).ToArray().AsArrayJoin(), string.Empty,
                        async () =>
                        {
                            var data =
                                await zoneRepository.GetByCategoryIds(query.DealerId, query.CategoryIds);
                            return data;
                        });
                }
                else
                {
                    zones = await zoneRepository.GetByCategoryIds(query.DealerId, query.CategoryIds);
                }

                zones = zones?.Where(p => p.Status == StatusEnum.Active).ToArray();
                response.Data = zones;
                response.SetSuccess();
            });
        }

        public async Task ZonePositionEventChange(ZonePositionChangeEvent @event)
        {
            await ProcessEvent(async () =>
            {
                await cacheService.Remove<RCZonePosition>(@event.ZoneId);
                var zoneCache = new CacheChangeEvent()
                {
                    Id = @event.ZoneId,
                    CacheType = KeyCacheTypeEnum.Zone,
                    IsTrigger = true
                };
                EventAdd(zoneCache);
            });
        }

        public async Task<BaseCommandResponse> ZonePositionSyncToES(ZonePositionSyncToESCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                ZonePositionSyncToESEvent @event = new ZonePositionSyncToESEvent()
                {
                    LoginUid = command.LoginUid,
                    ProcessUid = command.ProcessUid,
                    DealerId = command.DealerId
                };
                EventAdd(@event);
                response.SetSuccess();
            });
        }

        public async Task ZonePositionSync(ZonePositionSyncToESEvent @event)
        {
            await ProcessEvent(async () =>
            {
                var zones = await zoneRepository.Search(new ZoneSearchQuery()
                {
                    DealerId = @event.DealerId,
                    Status = StatusEnum.Active
                });
                if (zones is not { Length: > 0 })
                {
                    return;
                }

                LogInformation("ZonePositionSync total zone:" + zones.Length);
                foreach (var zone in zones)
                {
                    LogInformation($"ZonePositionSync:{zone.Id}---{zone.Name}");
                    if (zone.TotalPosition <= 0)
                    {
                        continue;
                    }

                    var zonePositions = await zoneRepository.ZonePositionGetCurrent(zone.Id, ActiveStatusEnum.Approved,
                        zone.TotalPosition, DateTime.Now);
                    if (zonePositions is not { Length: > 0 })
                    {
                        continue;
                    }

                    LogInformation($"ZonePositionSync length:{zonePositions.Length}");
                    RCZonePosition zonePosition = new RCZonePosition()
                    {
                        ZoneId = zone.Id,
                        Items = zonePositions
                    };
                    await cacheService.Set(zonePosition, zonePosition.ZoneId);
                }
            });
        }

        public async Task<BaseCommandResponse> ZonePositionChangeFromNews(ZonePositionChangeFromNewsCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                var rZonePositionIdsRemove = command.ZonePositionIdsRemove ?? Array.Empty<string>();
                var zoneIds = command.Items?.Select(p => p.ZoneId).Distinct().Where(p => p?.Length > 0).ToArray();
                RZone[] rZones = await zoneRepository.GetByIds(zoneIds);
                List<Zone> zonesChange = new List<Zone>();
                List<ZonePosition> zonePositionsRemoveAllZone = new List<ZonePosition>();
                if (rZones?.Length > 0)
                {
                    foreach (var rZone in rZones)
                    {
                        var itemsChangeInZone = command.Items?.FirstOrDefault(p => p.ZoneId == rZone.Id);
                        if (itemsChangeInZone?.IsReadOnly == true)
                        {
                            continue;
                        }

                        RZonePosition[]? rZonePositionsByZone = await zoneRepository.ZonePositionGetCurrent(
                            rZone.Id,
                            ActiveStatusEnum.Approved,
                            rZone.TotalPosition,
                            DateTime.Now);
                        // check is change
                        var itemInDb = rZonePositionsByZone?.FirstOrDefault(p =>
                                           p.ObjectId.Length > 0 && command.ObjectId?.Length > 0 &&
                                           p.ObjectId == command.ObjectId) ??
                                       rZonePositionsByZone?.FirstOrDefault(p =>
                                           p.Id == itemsChangeInZone?.ZonePositionId);
                        if (itemInDb != null && itemsChangeInZone != null)
                        {
                            ZonePosition zonePosition = new ZonePosition(itemInDb);
                            if (!zonePosition.IsChange(itemsChangeInZone))
                            {
                                continue;
                            }
                        }
                        else if (itemInDb == null && itemsChangeInZone is { ZonePositionId.Length: > 0 })
                        {
                            rZonePositionIdsRemove = rZonePositionIdsRemove
                                .Union(new[] { itemsChangeInZone.ZonePositionId }).ToArray();
                            itemsChangeInZone.ZonePositionId = string.Empty;
                        }

                        if (itemsChangeInZone != null)
                        {
                            if (itemsChangeInZone.LastOrder != rZone.LastOrder &&
                                itemsChangeInZone.ZonePositionId?.Length > 0)
                            {
                                response.SetFail("article.changed.but.zone.not.change");
                                return;
                            }
                        }

                        if (itemsChangeInZone!.DisplayType == ZonePositionDisplayTypeEnum.Active)
                        {
                            var removeDuplicateItems = rZonePositionsByZone?
                                .Where(p => command.ObjectId == p.ObjectId &&
                                            p.DisplayType != itemsChangeInZone.DisplayType).ToArray();
                            ZonePosition[]? zonePositionsRemove =
                                removeDuplicateItems?.Select(p => new ZonePosition(p)).ToArray();
                            if (zonePositionsRemove?.Length > 0)
                            {
                                zonePositionsRemoveAllZone.AddRange(zonePositionsRemove);
                            }

                            rZonePositionsByZone = rZonePositionsByZone?.Where(p =>
                                    p.Status == ActiveStatusEnum.Approved &&
                                    p.DisplayType == ZonePositionDisplayTypeEnum.Active)
                                .ToArray();
                            Zone zone = new Zone(rZone, rZonePositionsByZone);
                            zone.ZonePositionsChange(itemsChangeInZone, command, rZonePositionIdsRemove);
                            zonesChange.Add(zone);
                        }
                        else
                        {
                            var removeDuplicateItems = rZonePositionsByZone?
                                .Where(p => command.ObjectId == p.ObjectId &&
                                            p.DisplayType != itemsChangeInZone.DisplayType &&
                                            p.Id != itemsChangeInZone.ZonePositionId
                                ).Select(p => p.Id).Distinct()
                                .ToArray();


                            rZonePositionsByZone = rZonePositionsByZone?.Where(p =>
                                    p.Status == ActiveStatusEnum.Approved)
                                .ToArray();

                            Zone zone = new Zone(rZone, rZonePositionsByZone.ToArray());
                            zone.ZonePositionsChangeTimer(itemsChangeInZone, command, rZonePositionIdsRemove,
                                removeDuplicateItems);
                            zonesChange.Add(zone);
                        }
                    }
                }

                if (rZonePositionIdsRemove?.Length > 0)
                {
                    var zonePositionsRemove = await zoneRepository.ZonePositionGetByIds(rZonePositionIdsRemove);
                    var zoneIdsRemove = zonePositionsRemove.Select(p => p.ZoneId).ToArray();
                    RZone[] rZonesRemove = await zoneRepository.GetByIds(zoneIdsRemove);
                    foreach (var zonePositionRemove in zonePositionsRemove)
                    {
                        if (zonePositionRemove.DisplayType == ZonePositionDisplayTypeEnum.Active)
                        {
                            var rZone = rZonesRemove.FirstOrDefault(p => p.Id == zonePositionRemove.ZoneId);
                            RZonePosition[]? rZonePositionsByZone = await zoneRepository.ZonePositionGetCurrent(
                                rZone.Id,
                                ActiveStatusEnum.Approved, rZone.TotalPosition, DateTime.Now);

                            Zone zone = new Zone(rZone, rZonePositionsByZone.ToArray());
                            zone.ZonePositionsRemove(new[] { zonePositionRemove.Id }, command);
                            zonesChange.Add(zone);
                        }
                        else
                        {
                            ZonePosition zonePosition = new ZonePosition(zonePositionRemove);
                            zonePositionsRemoveAllZone.Add(zonePosition);
                        }
                    }
                }

                if (zonesChange.Count > 0 || zonePositionsRemoveAllZone.Count > 0)
                {
                    await zoneRepository.Change(zonesChange.ToArray(), zonePositionsRemoveAllZone.ToArray());
                    foreach (var zone in zonesChange)
                    {
                        SendEvent(zone, null);
                        SendZonePositionEvent(zone);
                    }
                }

                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> ZonePositionRemoveFromNews(ZonePositionRemoveFromNewsCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                if (command.ObjectId is not { Length: > 0 })
                {
                    response.SetSuccess();
                    return;
                }

                RZonePosition[] rZonePositions = await zoneRepository.ZonePositionGetByObjectId(command.ObjectId, 0);
                if (rZonePositions is not { Length: > 0 })
                {
                    response.SetSuccess();
                    return;
                }

                var zoneIds = rZonePositions.Select(p => p.ZoneId).Distinct().Where(p => p?.Length > 0).ToArray();
                RZone[] rZones = await zoneRepository.GetByIds(zoneIds);
                if (rZones is not { Length: > 0 })
                {
                    response.SetSuccess();
                    return;
                }

                List<Zone> zonesChange = new List<Zone>();
                foreach (var rZone in rZones)
                {
                    RZonePosition[]? rZonePositionsByZone = await zoneRepository.ZonePositionGetCurrent(
                        rZone.Id,
                        ActiveStatusEnum.Approved,
                        rZone.TotalPosition,
                        DateTime.Now);
                    Zone zone = new Zone(rZone, rZonePositionsByZone);
                    zone.RemoveByObjectId(command);
                    zonesChange.Add(zone);
                }

                if (zonesChange.Count > 0)
                {
                    await zoneRepository.Change(zonesChange.ToArray(), null);
                    foreach (var zone in zonesChange)
                    {
                        SendEvent(zone, null);
                        SendZonePositionEvent(zone);
                    }
                }

                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZone>> GetByObjectIdAndObjectType(
            ZoneGetByObjectIdAndObjectTypeQuery query)
        {
            return await ProcessCommand<RZone>(async (response) =>
            {
                var zones = await zoneRepository.GetByObjectIdAndObjectType(query.DealerId, query.ObjectId,
                    query.Type,
                    query.MappingKey);
                response.Data = zones;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZone[]?>> GetByMobilePage(ZoneGetByMobilePageQuery query)
        {
            return await ProcessCommand<RZone[]?>(async (response) =>
            {
                var zones = await zoneRepository.GetByMobilePage(query.MobilePage, query.DealerId);
                response.Data = zones;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RZone[]?>> GetByObjectType(ZoneGetByObjectTypeQuery query)
        {
            return await ProcessCommand<RZone[]?>(async (response) =>
            {
                var zones = await zoneRepository.GetByObjectType(query.Type, query.DealerId);
                response.Data = zones;
                response.SetSuccess();
            });
        }

        public async Task Process(ZoneChangeEvent @event)
        {
            await ProcessEvent(async () =>
            {
                var zone = await zoneRepository.GetById(@event.Id);
                if (zone != null)
                {
                    LogWarning($"Process ZoneChangeEvent DealerId:{zone.DealerId}");
                    await cacheService.Remove<RZone>(zone.Id);
                    await cacheService.Remove<RZone[]>(zone.DealerId);
                    if (zone.CategoryId?.Length > 0)
                    {
                        LogWarning($"Process(ZoneChangeEvent CategoryId):{zone.CategoryId}");
                        var categoryCache = new CacheChangeEvent()
                        {
                            Id = zone.CategoryId,
                            CacheType = KeyCacheTypeEnum.NewsCategory,
                            IsTrigger = true
                        };
                        EventAdd(categoryCache);
                    }

                    var zoneCache = new CacheChangeEvent()
                    {
                        Id = zone.Id,
                        CacheType = KeyCacheTypeEnum.Zone,
                        IsTrigger = true
                    };
                    EventAdd(zoneCache);
                }
            });
        }

        public async Task ProcessAutoChangePositionPublishDate()
        {
            while (true)
            {
                try
                {
                    LogWarning($"ProcessAutoChangePositionPublishDate");
                    var data = await zoneRepository.ZonePositionGetByIsTimerOrIsPin(ActiveStatusEnum.Approved,
                        DateTime.Now);
                    if (data is not { Length: > 0 })
                    {
                        continue;
                    }

                    var zoneIds = data.Select(p => p.ZoneId).Distinct().ToArray();
                    var zones = await zoneRepository.GetByIds(zoneIds);
                    foreach (var zone in zones)
                    {
                        await ProcessAutoChangePositionPublishDate(zone);
                    }
                }
                catch (Exception e)
                {
                    LogError(e, e.Message);
                }
                finally
                {
                    LogWarning($"ProcessAutoChangePositionPublishDate sleep");
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
            }
        }

        private async Task ProcessAutoChangePositionPublishDate(RZone zone)
        {
            LogWarning($"ProcessAutoChangePositionPublishDate by zone {zone.Id}");
            try
            {
                var zonePositions = await zoneRepository.ZonePositionGetCurrent(zone.Id,
                    ActiveStatusEnum.Approved, zone.TotalPosition, DateTime.Now);
                if (zonePositions is not { Length: > 0 })
                {
                    return;
                }

                Zone zoneDomain = new Zone(zone, zonePositions);
                bool isPin = zoneDomain.PinStarted(out ZonePosition[] zonePositionsSort);
                if (isPin)
                {
                    var zonePositionsRemove = zoneDomain.ZonePositions?
                        .Where(p =>
                            p.Position >= zonePositionsSort.Length &&
                            p.DisplayType == ZonePositionDisplayTypeEnum.Active &&
                            zonePositionsSort.All(q => q.Id != p.Id)).ToArray();
                    var result =
                        await zoneRepository.ChangePosition(zoneDomain, zonePositionsSort,
                            zonePositionsRemove, zone.LastOrder);
                    if (result)
                    {
                        SendEvent(zoneDomain, null);
                        SendZonePositionEvent(zoneDomain);
                        AddHistoryEvent("ProcessAutoChangePositionPublishDate", DateTime.Now, zoneDomain.Id,
                            zonePositions, zonePositionsSort, true);
                        //NotifyEvent();
                    }
                }
            }
            catch (Exception e)
            {
                LogWarning($"ProcessAutoChangePosition Error:{zone.Id}");
                LogError(e, e.Message);
            }
        }

        public async Task ProcessAutoRemoveCachePositionByPinDateFinish()
        {
            while (true)
            {
                try
                {
                    LogWarning("ProcessAutoRemoveCachePositionByPinDateFinish");
                    var data = await zoneRepository.ZonePositionGetByIsPinDateFinish(ActiveStatusEnum.Approved,
                        DateTime.Now);
                    if (data == null || data.Length <= 0)
                    {
                        continue;
                    }

                    var zoneIds = data.Select(p => p.ZoneId).Distinct().ToArray();
                    var zones = await zoneRepository.GetByIds(zoneIds);
                    foreach (var zoneId in zoneIds)
                    {
                        try
                        {
                            LogWarning($"ProcessAutoRemoveCachePositionByPinDateFinish:{zoneId}");
                            var zone = zones.FirstOrDefault(p => p.Id == zoneId);
                            if (zone == null)
                            {
                                continue;
                            }

                            var zonePositions = await zoneRepository.ZonePositionGetCurrent(zoneId,
                                ActiveStatusEnum.Approved, zone.TotalPosition, DateTime.Now);
                            if (zonePositions is not { Length: > 0 })
                            {
                                continue;
                            }

                            Zone zoneDomain = new Zone(zone, zonePositions);
                            var isRemove = (zoneDomain.Options.HasFlag(ZoneOptionEnum.VideoAuto) ||
                                            zoneDomain.Options.HasFlag(ZoneOptionEnum.EventAuto) ||
                                            zoneDomain.Options.HasFlag(ZoneOptionEnum.SeriesAuto)
                                           ) &&
                                           zoneDomain.Options.HasFlag(ZoneOptionEnum.PinSupport);
                            if (isRemove)
                            {
                                List<ZonePosition> zonePositionsChange = zoneDomain.PinFinishedRemove();
                                if (zonePositionsChange.Count > 0)
                                {
                                    var result = await zoneRepository.Remove(zonePositionsChange.ToArray());
                                    if (result)
                                    {
                                        SendEvent(zoneDomain, null);
                                        SendZonePositionEvent(zoneDomain);
                                        AddHistoryEvent("ProcessAutoRemoveCachePositionByPinDateFinishRemove", DateTime.Now,
                                            zoneDomain.Id, zonePositions, zonePositionsChange.ToArray(), true);
                                        //NotifyEvent();
                                    }
                                }
                            }
                            else
                            {
                                List<ZonePosition> zonePositionsChange = zoneDomain.PinFinished();
                                if (zonePositionsChange?.Count > 0)
                                {
                                    var result = await zoneRepository.ChangePosition(zoneDomain,
                                        zonePositionsChange.ToArray(), null, zone.LastOrder);
                                    if (result)
                                    {
                                        SendEvent(zoneDomain, null);
                                        SendZonePositionEvent(zoneDomain);
                                        AddHistoryEvent("ProcessAutoRemoveCachePositionByPinDateFinish", DateTime.Now,
                                            zoneDomain.Id, zonePositions, zonePositionsChange.ToArray(), true);
                                        //NotifyEvent();
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            LogWarning($"ProcessAutoRemoveCachePositionByPinDateFinish Error:{zoneId}");
                            LogError(e, e.Message);
                        }
                    }
                }
                catch (Exception e)
                {
                    LogError(e, e.Message);
                }
                finally
                {
                    LogWarning("ProcessAutoRemoveCachePositionByPinDateFinish sleep");
                    await Task.Delay(TimeSpan.FromMinutes(1));
                }
            }
        }

        public async Task ProcessEvent(ZonePositionHistoryEvent @event)
        {
            await ProcessEvent(async () =>
            {
                //var command = Common.Serialize.JsonDeserializeObject<ZonePositionItemChangeCommand>(@event.Command);
                List<ZonePositionHistory> histories = new List<ZonePositionHistory>();
                if (@event.ZonePositionsOld?.Length > 0)
                {
                    foreach (var itemEvent in @event.ZonePositionsOld)
                    {
                        histories.Add(new ZonePositionHistory(@event.ChangeId, @event.IsAuto, itemEvent,
                            @event.ProcessUid,
                            @event.ProcessDate));
                    }
                }

                if (@event.ZonePositionsNew?.Length > 0)
                {
                    foreach (var itemEvent in @event.ZonePositionsNew)
                    {
                        ZonePositionActionTypeEnum actionType = ZonePositionActionTypeEnum.Change;
                        if (itemEvent.Status != ActiveStatusEnum.Approved)
                        {
                            actionType = ZonePositionActionTypeEnum.Remove;
                        }

                        var position = histories.FirstOrDefault(p =>
                            p.Position == itemEvent.Position && p.DisplayType == itemEvent.DisplayType);
                        if (position == null)
                        {
                            histories.Add(
                                new ZonePositionHistory(@event.ChangeId, @event.IsAuto, actionType, itemEvent,
                                    @event.ProcessUid,
                                    @event.ProcessDate));
                        }
                        else
                        {
                            position.Change(actionType, itemEvent);
                        }
                    }
                }

                if (histories?.Count > 0)
                {
                    var position0 = histories.FirstOrDefault(p => p.Position == 0);
                    if (position0 == null)
                    {
                        histories.Add(new ZonePositionHistory(@event.ChangeId, @event.ZoneId, @event.ProcessUid,
                            @event.ProcessDate, false));
                    }

                    await zonePositionHistoryRepository.Add(histories.ToArray());
                }
            });
        }

        public async Task<BaseCommandResponse<RZonePositionHistory[]?>> ZonePositionHistoryGets(
            ZonePositionHistoryGetQuery query)
        {
            return await ProcessCommand<RZonePositionHistory[]?>(async (response) =>
            {
                RefSqlPaging sqlPaging = new RefSqlPaging(query.PageIndex, query.PageSize);
                var zones = await zonePositionHistoryRepository.Gets(query.ZoneId, sqlPaging, query.ChangeId,
                    query.IsGetDetail);
                response.Data = zones;
                response.SetSuccess();
            });
        }

        public async Task ProcessEvent(ZonePositionPinChangeEvent @event)
        {
            await ProcessEvent(async () =>
            {
                var zone = await zoneRepository.GetById(@event.ZoneId);
                if (zone == null)
                {
                    return;
                }

                await ProcessAutoChangePositionPublishDate(zone);
            });
        }

        private void AddHistoryEvent(string createdUid, DateTime date, string zoneId, RZonePosition[]? zonePositionsOld,
            ZonePosition[]? zonePositionsNew, bool isAuto)
        {
            string changeId = Common.CommonUtility.GenerateGuid();
            ZonePositionHistoryEvent @event = new ZonePositionHistoryEvent()
            {
                ChangeId = changeId,
                ZoneId = zoneId,
                ZonePositionsOld = zonePositionsOld?.Select(p => new ZonePositionHistoryItemEvent()
                {
                    Position = p.Position,
                    Priority = p.Priority,
                    Status = p.Status,
                    Type = p.Type,
                    BeginDate = p.BeginDate,
                    EndDate = p.EndDate,
                    IsAuto = p.IsAuto,
                    IsPin = p.IsPin,
                    LoginUid = p.LoginUid,
                    Order = p.Order,
                    ObjectId = p.ObjectId,
                    OffTime = p.OffTime,
                    PinFinish = p.PinFinish,
                    PinStart = p.PinStart,
                    Version = p.Version,
                    DisplayTime = p.DisplayTime,
                    DisplayType = p.DisplayType,
                    ZoneId = p.ZoneId,
                }).ToArray(),
                ZonePositionsNew = zonePositionsNew?.Select(p => p.ZonePositionHistoryItemEvent()).ToArray(),
                IsAuto = isAuto,
                ProcessUid = createdUid,
                ProcessDate = date
            };
            EventAdd(@event);
        }
    }
}
