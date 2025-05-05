using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseCommands;
using TMQ.BaseDomains;
using TMQ.BaseEvents;
using TMQ.Common;
using TMQ.EnumDefine;
using TMQ.SystemCommands.Commands;
using TMQ.SystemCommands.Events;
using TMQ.SystemReadModels;

namespace TMQ.SystemDomains
{
    [Table("Zone_tbl")]
    public class Zone : BaseDomain
    {
        public Zone(RZone zone) : base(zone)
        {
            Id = zone.Id;
            Name = zone.Name;
            Description = zone.Description;
            Status = zone.Status;
            Priority = zone.Priority;
            DealerId = zone.DealerId;
            TotalPosition = zone.TotalPosition;
            LastOrder = zone.LastOrder;
            LockUserId = zone.LockUserId;
            LockSocketId = zone.LockSocketId;
            CategoryId = zone.CategoryId.AsEmpty();
            Thumbnail = zone.Thumbnail;
            ObjectId = zone.ObjectId;
            ObjectType = zone.ObjectType;
            MappingKey = zone.MappingKey;
            GroupId = zone.GroupId;
            MobilePage = zone.MobilePage;
            AutoSetPositionTime = zone.AutoSetPositionTime;
            PRExpiredPosition = zone.PRExpiredPosition;
            Options = zone.Options;
        }

        public Zone(RZone zone, RZonePosition[]? zonePositions) : this(zone)
        {
            ZonePositions = new List<ZonePosition>();
            if (!(zonePositions?.Length > 0)) return;
            zonePositions = zonePositions.OrderByDescending(p => p.Position).ToArray();
            foreach (var zonePosition in zonePositions)
            {
                ZonePositions.Add(new ZonePosition(zonePosition));
            }
        }

        public Zone(RZone zone, RZonePosition[]? zonePositions, RZonePosition[]? zonePositionsTimer) : this(zone,
            zonePositions)
        {
            ZonePositionsTimer = new List<ZonePosition>();
            if (!(zonePositionsTimer?.Length > 0)) return;
            zonePositionsTimer = zonePositionsTimer.OrderByDescending(p => p.Position).ToArray();
            foreach (var zonePosition in zonePositionsTimer)
            {
                ZonePositionsTimer.Add(new ZonePosition(zonePosition));
            }
        }

        public Zone(ZoneAddCommand command) : base(command)
        {
            Id = command.Code;
            Name = command.Name.AsEmpty();
            Description = command.Description.AsEmpty();
            Status = command.Status;
            Priority = command.Priority;
            DealerId = command.DealerId;
            TotalPosition = command.TotalPosition;
            LockUserId = string.Empty;
            LockSocketId = string.Empty;
            CategoryId = command.CategoryId.AsEmpty();
            Thumbnail = command.Thumbnail;
            ObjectId = command.ObjectId.AsEmpty();
            ObjectType = command.ObjectType;
            MappingKey = command.MappingKey.AsEmpty();
            GroupId = command.GroupId.AsEmpty();
            MobilePage = command.MobilePage;
            AutoSetPositionTime = command.AutoSetPositionTime;
            PRExpiredPosition = command.PRExpiredPosition;
            Options = command.Options.GetValueOrDefault();
        }

        public void Change(ZoneChangeCommand command)
        {
            Name = command.Name.AsEmpty();
            Description = command.Description.AsEmpty();
            Status = command.Status;
            Priority = command.Priority;
            DealerId = command.DealerId;
            TotalPosition = command.TotalPosition;
            CategoryId = command.CategoryId.AsEmpty();
            Thumbnail = command.Thumbnail;
            ObjectId = command.ObjectId.AsEmpty();
            ObjectType = command.ObjectType;
            MappingKey = command.MappingKey.AsEmpty();
            GroupId = command.GroupId.AsEmpty();
            MobilePage = command.MobilePage;
            AutoSetPositionTime = command.AutoSetPositionTime;
            PRExpiredPosition = command.PRExpiredPosition;
            Options = command.Options.GetValueOrDefault();
            Changed(command);
        }

        public void ChangeStatus(ZoneChangeStatusCommand command)
        {
            Status = command.Status;
            Changed(command);
        }

        public void ZonePositionsChange(ZonePositionChangeCommand command, string[] rZonePositionIdsRemove, out
            HashSet<string> zonePositionIdsChangeToTimer)
        {
            zonePositionIdsChangeToTimer = new HashSet<string>();
            if (!(command.Items?.Length > 0)) return;
            int totalPosition = TotalPosition * 2;
            if (totalPosition < 50)
            {
                totalPosition = 100;
            }

            ZonePosition?[] zonePositionsResult = new ZonePosition[totalPosition];
            var zonePositionsIsPin =
                ZonePositions.Where(p =>
                        p.DisplayType == ZonePositionDisplayTypeEnum.Active &&
                        p.IsPin &&
                        !rZonePositionIdsRemove!.Contains(p.Id) &&
                        command.Items.All(q => q.Id != p.Id))
                    .OrderBy(p => p.Position)
                    .ToList();

            HashSet<string> idsAdd = new HashSet<string>();
            HashSet<string> objectIdsAdd = new HashSet<string>();
            Queue<ZonePosition> queueZonePositionsActivePin = new Queue<ZonePosition>();
            foreach (var zonePosition in zonePositionsIsPin)
            {
                queueZonePositionsActivePin.Enqueue(zonePosition);
            }

            while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item) && item != null)
            {
                int position = item.Position;
                while (true)
                {
                    if (position >= zonePositionsResult.Length)
                    {
                        break;
                    }

                    if (zonePositionsResult[position] == null)
                    {
                        item.Position = position;
                        zonePositionsResult[position] = item;
                        idsAdd.Add(item.Id);
                        objectIdsAdd.Add(item.ObjectId);
                        break;
                    }

                    if (position > zonePositionsResult.Length)
                    {
                        break;
                    }

                    position += 1;
                }
            }

            var items = command.Items
                .Where(p => p.BeginDateTime == null || p.BeginDateTime <= DateTime.Now)
                .OrderBy(p => p.Position)
                .ToArray();
            var itemsTimer = command.Items
                .Where(p => p.BeginDateTime > DateTime.Now)
                .OrderBy(p => p.Position)
                .ToArray();

            foreach (var commandItem in items)
            {
                if (!commandItem.Status)
                {
                    continue;
                }

                ZonePosition? zonePosition =
                    ZonePositions.FirstOrDefault(p =>
                        p.ObjectId.Length > 0 && commandItem.ObjectId.Length > 0 && p.ObjectId == commandItem.ObjectId) ??
                    ZonePositions.FirstOrDefault(p => p.Id == commandItem.Id);
                if (zonePosition is { IsPin: true })
                {
                    if (idsAdd.Contains(zonePosition.Id))
                    {
                        continue;
                    }

                    int position = zonePosition.Position;
                    while (true)
                    {
                        if (position >= zonePositionsResult.Length)
                        {
                            break;
                        }

                        if (zonePositionsResult[position] == null)
                        {
                            zonePosition.Position = position;
                            zonePositionsResult[position] = zonePosition;
                            idsAdd.Add(zonePosition.Id);
                            objectIdsAdd.Add(zonePosition.ObjectId);
                            break;
                        }

                        if (position > zonePositionsResult.Length)
                        {
                            break;
                        }

                        position += 1;
                    }
                }
            }

            List<ZonePosition> zonePositionsRemove = new List<ZonePosition>();
            foreach (var commandItem in items)
            {
                ZonePosition? zonePosition =
                    ZonePositions.FirstOrDefault(p =>
                        p.ObjectId.Length > 0 && commandItem.ObjectId.Length > 0 && p.ObjectId == commandItem.ObjectId) ??
                    ZonePositions.FirstOrDefault(p => p.Id == commandItem.Id);
                if (zonePosition == null)
                {
                    ZonePosition zonePositionAdd = new ZonePosition(commandItem, command, this);
                    int position = zonePositionAdd.Position;
                    while (true)
                    {
                        if (position >= zonePositionsResult.Length)
                        {
                            break;
                        }

                        if (zonePositionsResult[position] == null)
                        {
                            zonePositionAdd.Position = position;
                            zonePositionsResult[position] = zonePositionAdd;
                            idsAdd.Add(zonePositionAdd.Id);
                            objectIdsAdd.Add(zonePositionAdd.ObjectId);
                            break;
                        }

                        if (position > zonePositionsResult.Length)
                        {
                            break;
                        }

                        position += 1;
                    }
                }
                else
                {
                    if (idsAdd.Contains(zonePosition.Id))
                    {
                        continue;
                    }

                    if (commandItem.Status)
                    {
                        if (zonePosition.IsPin)
                        {
                            continue;
                        }

                        zonePosition.Change(commandItem, command, this);
                        int position = zonePosition.Position;
                        while (true)
                        {
                            if (position >= zonePositionsResult.Length)
                            {
                                break;
                            }

                            if (zonePositionsResult[position] == null)
                            {
                                zonePosition.Position = position;
                                zonePositionsResult[position] = zonePosition;
                                idsAdd.Add(zonePosition.Id);
                                objectIdsAdd.Add(zonePosition.ObjectId);
                                break;
                            }

                            if (position > zonePositionsResult.Length)
                            {
                                break;
                            }

                            position += 1;
                        }
                    }
                    else
                    {
                        zonePosition.Remove(command);
                        zonePositionsRemove.Add(zonePosition);
                    }
                }
            }

            if (itemsTimer?.Length > 0)
            {
                foreach (var commandItem in itemsTimer)
                {
                    if (commandItem.Id?.Length > 0)
                    {
                        if (idsAdd.Contains(commandItem.Id))
                        {
                            continue;
                        }

                        idsAdd.Add(commandItem.Id);
                        ZonePosition? zonePositionRemove =
                            ZonePositions.FirstOrDefault(p =>
                                p.ObjectId.Length > 0 && commandItem.ObjectId.Length > 0 &&
                                p.ObjectId == commandItem.ObjectId) ??
                            ZonePositions.FirstOrDefault(p => p.Id == commandItem.Id);
                        if (zonePositionRemove != null)
                        {
                            zonePositionRemove.Remove(command);
                            zonePositionsRemove.Add(zonePositionRemove);
                        }
                    }
                }
            }

            var zonePositionsIsActive = ZonePositions
                .Where(p =>
                    p.DisplayType == ZonePositionDisplayTypeEnum.Active &&
                    p.IsPin == false &&
                    !rZonePositionIdsRemove!.Contains(p.Id) &&
                    !idsAdd!.Contains(p.Id)
                )
                .OrderBy(p => p.Position).ToList();

            queueZonePositionsActivePin = new Queue<ZonePosition>();
            foreach (var zonePositionIsActive in zonePositionsIsActive)
            {
                queueZonePositionsActivePin.Enqueue(zonePositionIsActive);
            }

            while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item) && item != null)
            {
                int position = 0;
                while (true)
                {
                    if (position >= zonePositionsResult.Length)
                    {
                        break;
                    }

                    if (zonePositionsResult[position] == null)
                    {
                        item.Position = position;
                        zonePositionsResult[position] = item;
                        break;
                    }

                    if (position > zonePositionsResult.Length)
                    {
                        break;
                    }

                    position += 1;
                }
            }

            ZonePositions = zonePositionsResult.Where(p => p != null).OrderBy(p => p.Position).ToList();
            int i = 0;

            foreach (var zonePosition in ZonePositions)
            {
                if (zonePosition.Status != ActiveStatusEnum.Approved)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(zonePosition.ObjectId))
                {
                    zonePosition.Remove(command);
                    continue;
                }

                zonePosition.ChangePosition(i, this);
                i++;
                if (zonePosition.Position > totalPosition)
                {
                    zonePosition.Remove(command);
                }
            }

            if (zonePositionsRemove?.Count > 0)
            {
                ZonePositions = ZonePositions.Union(zonePositionsRemove).ToList();
            }

            List<ZonePosition> zonePositionsTimer = new List<ZonePosition>();
            if (itemsTimer?.Length > 0)
            {
                foreach (var commandItem in itemsTimer)
                {
                    ZonePosition? zonePosition =
                        ZonePositionsTimer.FirstOrDefault(p =>
                            p.ObjectId.Length > 0 && commandItem.ObjectId.Length > 0 &&
                            p.ObjectId == commandItem.ObjectId) ??
                        ZonePositionsTimer.FirstOrDefault(p => p.Id == commandItem.Id);
                    if (zonePosition == null)
                    {
                        ZonePosition zonePositionAdd =
                            new ZonePosition(commandItem, command, this);
                        zonePositionAdd.DisplayTypeChange(ZonePositionDisplayTypeEnum.Timer);
                        zonePositionsTimer.Add(zonePositionAdd);
                    }
                    else
                    {
                        if (commandItem.Status)
                        {
                            zonePosition.Change(commandItem, command, this);
                        }
                        else
                        {
                            zonePosition.Remove(command);
                        }

                        zonePosition.DisplayTypeChange(ZonePositionDisplayTypeEnum.Timer);
                        zonePositionsTimer.Add(zonePosition);
                        zonePositionIdsChangeToTimer.Add(zonePosition.Id);
                    }
                }
            }

            if (zonePositionsTimer?.Count > 0)
            {
                ZonePositions = ZonePositions.Union(zonePositionsTimer).ToList();
            }

            LastOrder = ZonePositions.Max(p => p.Order);
        }

        public void ZonePositionsChangeTimer(ZonePositionChangeCommand command,
            string[]? rZonePositionIdsRemove,
            string[] zonePositionsRemoveDuplicate)
        {
            if (rZonePositionIdsRemove?.Length > 0)
            {
                foreach (var zonePosition in ZonePositions)
                {
                    if (rZonePositionIdsRemove.Contains(zonePosition.Id))
                    {
                        zonePosition.Remove(command);
                    }
                }
            }

            var items = command.Items.OrderBy(p => p.Position)
                .ToArray();
            foreach (var commandItem in items)
            {
                ZonePosition? zonePosition =
                    ZonePositions.FirstOrDefault(p =>
                        p.ObjectId.Length > 0 && commandItem.ObjectId.Length > 0 && p.ObjectId == commandItem.ObjectId) ??
                    ZonePositions.FirstOrDefault(p => p.Id == commandItem.Id);
                if (zonePosition == null)
                {
                    ZonePosition zonePositionAdd = new ZonePosition(commandItem, command, this);
                    ZonePositions.Add(zonePositionAdd);
                }
                else
                {
                    if (commandItem.Status)
                    {
                        zonePosition.Change(commandItem, command, this);
                    }
                    else
                    {
                        zonePosition.Remove(command);
                    }
                }
            }

            foreach (var zonePosition in ZonePositions)
            {
                if (string.IsNullOrEmpty(zonePosition.ObjectId))
                {
                    zonePosition.Remove(command);
                }
            }

            ZonePositionsRemove(zonePositionsRemoveDuplicate, command);

            LastOrder = ZonePositions.Max(p => p.Order);
        }

        public void ZonePositionsChange(ZonePositionItemChangeFromNewsCommand? commandItem,
            ZonePositionChangeFromNewsCommand command, string[]? rZonePositionIdsRemove)
        {
            int totalPosition = TotalPosition * 2;
            if (totalPosition < 50)
            {
                totalPosition = 100;
            }

            rZonePositionIdsRemove ??= Array.Empty<string>();
            if (rZonePositionIdsRemove?.Length > 0)
            {
                foreach (var zonePosition in ZonePositions)
                {
                    if (rZonePositionIdsRemove.Contains(zonePosition.Id))
                    {
                        zonePosition.Remove(command);
                    }
                }
            }

            ZonePosition?[] zonePositionsResult = new ZonePosition[totalPosition];
            var zonePositionsIsPin =
                ZonePositions.Where(p =>
                        p.DisplayType == ZonePositionDisplayTypeEnum.Active &&
                        p.IsPin &&
                        !rZonePositionIdsRemove!.Contains(p.Id) &&
                        (commandItem == null || commandItem.ZonePositionId != p.Id)
                    )
                    .OrderBy(p => p.Position)
                    .ToList();
            HashSet<string> idsAdd = new HashSet<string>();
            Queue<ZonePosition> queueZonePositionsActivePin = new Queue<ZonePosition>();
            foreach (var zonePosition in zonePositionsIsPin)
            {
                queueZonePositionsActivePin.Enqueue(zonePosition);
            }

            while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item) && item != null)
            {
                int position = item.Position;
                while (true)
                {
                    if (position >= zonePositionsResult.Length)
                    {
                        break;
                    }

                    if (zonePositionsResult[position] == null)
                    {
                        item.Position = position;
                        zonePositionsResult[position] = item;
                        idsAdd.Add(item.Id);
                        break;
                    }

                    if (position > zonePositionsResult.Length)
                    {
                        break;
                    }

                    position += 1;
                }
            }

            if (commandItem != null)
            {
                ZonePosition? zonePosition =
                    ZonePositions.FirstOrDefault(p =>
                        p.ObjectId.Length > 0 && command.ObjectId?.Length > 0 && p.ObjectId == command.ObjectId) ??
                    ZonePositions.FirstOrDefault(p => p.Id == commandItem.ZonePositionId);

                if (zonePosition == null)
                {
                    ZonePosition zonePositionAdd = new ZonePosition(commandItem, command, this);
                    int position = zonePositionAdd.Position;
                    while (true)
                    {
                        if (position >= zonePositionsResult.Length)
                        {
                            break;
                        }

                        if (zonePositionsResult[position] == null)
                        {
                            zonePositionAdd.Position = position;
                            zonePositionsResult[position] = zonePositionAdd;
                            idsAdd.Add(zonePositionAdd.Id);
                            break;
                        }

                        if (position > zonePositionsResult.Length)
                        {
                            break;
                        }

                        position += 1;
                    }

                    var zonePositionsIsActive = ZonePositions
                        .Where(p =>
                            p.DisplayType == ZonePositionDisplayTypeEnum.Active &&
                            p.IsPin == false &&
                            !rZonePositionIdsRemove!.Contains(p.Id) &&
                            !idsAdd!.Contains(p.Id)
                        )
                        .OrderBy(p => p.Position).ToList();

                    queueZonePositionsActivePin = new Queue<ZonePosition>();
                    foreach (var zonePositionIsActive in zonePositionsIsActive)
                    {
                        queueZonePositionsActivePin.Enqueue(zonePositionIsActive);
                    }

                    while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item) && item != null)
                    {
                        position = 0;
                        while (true)
                        {
                            if (position >= zonePositionsResult.Length)
                            {
                                break;
                            }

                            if (zonePositionsResult[position] == null)
                            {
                                item.Position = position;
                                zonePositionsResult[position] = item;
                                break;
                            }

                            if (position > zonePositionsResult.Length)
                            {
                                break;
                            }

                            position += 1;
                        }
                    }
                }
                else
                {
                    if (commandItem.Status)
                    {
                        zonePosition.Change(commandItem, command, this);
                        if (zonePosition.DisplayType == ZonePositionDisplayTypeEnum.Active)
                        {
                            zonePosition.IsPin = false;
                        }

                        int position = zonePosition.Position;
                        while (true)
                        {
                            if (position >= zonePositionsResult.Length)
                            {
                                break;
                            }

                            if (zonePositionsResult[position] == null)
                            {
                                zonePosition.Position = position;
                                zonePositionsResult[position] = zonePosition;
                                idsAdd.Add(zonePosition.Id);
                                break;
                            }

                            if (position > zonePositionsResult.Length)
                            {
                                break;
                            }

                            position += 1;
                        }
                    }
                    else
                    {
                        zonePosition.Remove(command);
                    }

                    var zonePositionsIsActive = ZonePositions
                        .Where(p =>
                            p.DisplayType == ZonePositionDisplayTypeEnum.Active &&
                            p.IsPin == false &&
                            !rZonePositionIdsRemove!.Contains(p.Id) &&
                            !idsAdd!.Contains(p.Id)
                        )
                        .OrderBy(p => p.Position).ToList();

                    queueZonePositionsActivePin = new Queue<ZonePosition>();
                    foreach (var zonePositionIsActive in zonePositionsIsActive)
                    {
                        queueZonePositionsActivePin.Enqueue(zonePositionIsActive);
                    }

                    while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item) && item != null)
                    {
                        int position = 0;
                        while (true)
                        {
                            if (position >= zonePositionsResult.Length)
                            {
                                break;
                            }

                            if (zonePositionsResult[position] == null)
                            {
                                item.Position = position;
                                zonePositionsResult[position] = item;
                                break;
                            }

                            if (position > zonePositionsResult.Length)
                            {
                                break;
                            }

                            position += 1;
                        }
                    }
                }
            }

            zonePositionsResult = zonePositionsResult.Where(p => p != null).OrderBy(p => p.Position).ToArray();

            var zonePositionsRemove = ZonePositions.Where(p =>
                p.DisplayType == ZonePositionDisplayTypeEnum.Active &&
                zonePositionsResult.All(q => q.Id != p.Id)
            ).ToArray();
            ZonePositions = zonePositionsResult.ToList();


            int i = 0;
            foreach (var zonePosition in ZonePositions)
            {
                if (zonePosition.Status != ActiveStatusEnum.Approved)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(zonePosition.ObjectId))
                {
                    zonePosition.Remove(command);
                    continue;
                }

                zonePosition.ChangePosition(i, this);
                i++;
                if (zonePosition.Position > totalPosition)
                {
                    zonePosition.Remove(command);
                }
            }

            if (zonePositionsRemove?.Length > 0)
            {
                foreach (var zonePosition in zonePositionsRemove)
                {
                    zonePosition.Remove(command);
                    ZonePositions.Add(zonePosition);
                }
            }

            LastOrder = ZonePositions.Max(p => p.Order);
        }

        public void ZonePositionsRemove(string[] zonePositionsRemoveDuplicate, BaseCommand command)
        {
            int totalPosition = TotalPosition * 2;
            if (totalPosition < 50)
            {
                totalPosition = 100;
            }

            if (zonePositionsRemoveDuplicate == null)
            {
                zonePositionsRemoveDuplicate = Array.Empty<string>();
            }

            foreach (var position in ZonePositions)
            {
                if (zonePositionsRemoveDuplicate.Contains(position.Id))
                {
                    position.Remove(command);
                }
            }

            ZonePosition?[] zonePositionsResult = new ZonePosition[totalPosition];
            var zonePositionsIsPin =
                ZonePositions.Where(p =>
                        p.DisplayType == ZonePositionDisplayTypeEnum.Active &&
                        p.IsPin &&
                        !zonePositionsRemoveDuplicate.Contains(p.Id))
                    .OrderBy(p => p.Position)
                    .ToList();
            HashSet<string> idsAdd = new HashSet<string>();
            Queue<ZonePosition> queueZonePositionsActivePin = new Queue<ZonePosition>();
            foreach (var zonePosition in zonePositionsIsPin)
            {
                queueZonePositionsActivePin.Enqueue(zonePosition);
            }

            while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item) && item != null)
            {
                int position = item.Position;
                while (true)
                {
                    if (position >= zonePositionsResult.Length)
                    {
                        break;
                    }

                    if (zonePositionsResult[position] == null)
                    {
                        item.Position = position;
                        zonePositionsResult[position] = item;
                        break;
                    }

                    if (position > zonePositionsResult.Length)
                    {
                        break;
                    }

                    position += 1;
                }
            }

            var zonePositionsIsActive = ZonePositions
                .Where(p =>
                    p.DisplayType == ZonePositionDisplayTypeEnum.Active &&
                    p.IsPin == false &&
                    !zonePositionsRemoveDuplicate.Contains(p.Id) &&
                    !idsAdd!.Contains(p.Id)
                )
                .OrderBy(p => p.Position).ToList();

            queueZonePositionsActivePin = new Queue<ZonePosition>();
            foreach (var zonePositionIsActive in zonePositionsIsActive)
            {
                queueZonePositionsActivePin.Enqueue(zonePositionIsActive);
            }

            while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item) && item != null)
            {
                int position = 0;
                while (true)
                {
                    if (position >= zonePositionsResult.Length)
                    {
                        break;
                    }

                    if (zonePositionsResult[position] == null)
                    {
                        item.Position = position;
                        zonePositionsResult[position] = item;
                        break;
                    }

                    if (position > zonePositionsResult.Length)
                    {
                        break;
                    }

                    position += 1;
                }
            }
        }

        public void ZonePositionsChangeTimer(
            ZonePositionItemChangeFromNewsCommand commandItem,
            ZonePositionChangeFromNewsCommand command,
            string[]? rZonePositionIdsRemove,
            string[] zonePositionsRemoveDuplicate)
        {
            if (rZonePositionIdsRemove?.Length > 0)
            {
                foreach (var zonePosition in ZonePositions)
                {
                    if (rZonePositionIdsRemove.Contains(zonePosition.Id))
                    {
                        zonePosition.Remove(command);
                    }
                }
            }

            if (commandItem != null)
            {
                ZonePosition? zonePosition =
                    ZonePositions.FirstOrDefault(p =>
                        p.ObjectId.Length > 0 && command.ObjectId?.Length > 0 &&
                        p.ObjectId == command.ObjectId &&
                        p.DisplayType == commandItem.DisplayType
                    ) ??
                    ZonePositions.FirstOrDefault(p => p.Id == commandItem.ZonePositionId);
                if (zonePosition == null)
                {
                    ZonePosition zonePositionAdd = new ZonePosition(commandItem, command, this);
                    ZonePositions.Add(zonePositionAdd);
                }
                else
                {
                    if (commandItem.Status)
                    {
                        zonePosition.Change(commandItem, command, this);
                    }
                    else
                    {
                        zonePosition.Remove(command);
                    }
                }
            }

            ZonePositionsRemove(zonePositionsRemoveDuplicate, command);

            foreach (var zonePosition in ZonePositions)
            {
                if (string.IsNullOrEmpty(zonePosition.ObjectId))
                {
                    zonePosition.Remove(command);
                }
            }

            LastOrder = ZonePositions.Max(p => p.Order);
        }

        public List<ZonePosition> PinFinishedRemove()
        {
            List<ZonePosition> zonePositionsChange = new List<ZonePosition>();
            if (!(ZonePositions?.Count > 0)) return zonePositionsChange;
            foreach (var zonePosition in ZonePositions)
            {
                var isChange =
                    zonePosition.PinFinishedRemove();
                if (isChange)
                {
                    zonePositionsChange.Add(zonePosition);
                }
            }

            return zonePositionsChange.ToList()!;
        }

        public List<ZonePosition> PinFinished()
        {
            List<ZonePosition> zonePositionsChange = new List<ZonePosition>();
            if (!(ZonePositions?.Count > 0)) return zonePositionsChange;
            foreach (var zonePosition in ZonePositions)
            {
                var isChange =
                    zonePosition.PinFinished((int)AutoSetPositionTime, PRExpiredPosition, TotalPosition);
                if (isChange)
                {
                    zonePositionsChange.Add(zonePosition);
                }
            }

            int totalPosition = TotalPosition * 2;
            if (totalPosition < 50)
            {
                totalPosition = 100;
            }

            ZonePosition?[] zonePositionsResult = new ZonePosition[totalPosition + zonePositionsChange.Count];

            if (zonePositionsChange?.Count > 0)
            {
                var zonePositionIdsChange = zonePositionsChange.Select(p => p.Id).ToArray();

                var zonePositionsIsPin =
                    ZonePositions.Where(p =>
                            p is { DisplayType: ZonePositionDisplayTypeEnum.Active, IsPin: true } &&
                            !zonePositionIdsChange.Contains(p.Id)
                        )
                        .OrderBy(p => p.Position)
                        .ToList();
                HashSet<string> idsAdd = new HashSet<string>();
                Queue<ZonePosition> queueZonePositionsActivePin = new Queue<ZonePosition>();
                foreach (var zonePosition in zonePositionsIsPin)
                {
                    queueZonePositionsActivePin.Enqueue(zonePosition);
                }

                while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item) && item != null)
                {
                    int position = item.Position;
                    while (true)
                    {
                        if (position >= zonePositionsResult.Length)
                        {
                            break;
                        }

                        if (zonePositionsResult[position] == null)
                        {
                            item.Position = position;
                            zonePositionsResult[position] = item;
                            idsAdd.Add(item.Id);
                            break;
                        }

                        if (position > zonePositionsResult.Length)
                        {
                            break;
                        }

                        position += 1;
                    }
                }

                var zonePositionsChangeIsCurrentPinFinish = ZonePositions
                    .Where(p => p is
                    { DisplayType: ZonePositionDisplayTypeEnum.Active, IsPin: false, IsCurrentPinFinish: true })
                    .OrderBy(p => p.Position).ToList();
                queueZonePositionsActivePin = new Queue<ZonePosition>();
                foreach (var zonePosition in zonePositionsChangeIsCurrentPinFinish)
                {
                    queueZonePositionsActivePin.Enqueue(zonePosition);
                }

                while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item) && item != null)
                {
                    int position = item.Position;
                    while (true)
                    {
                        if (position >= zonePositionsResult.Length)
                        {
                            break;
                        }

                        if (zonePositionsResult[position] == null)
                        {
                            item.Position = position;
                            zonePositionsResult[position] = item;
                            break;
                        }

                        if (position > zonePositionsResult.Length)
                        {
                            break;
                        }

                        position += 1;
                    }
                }


                var zonePositionsChangeIsActive = ZonePositions
                    .Where(p => p is
                    { DisplayType: ZonePositionDisplayTypeEnum.Active, IsPin: false, IsCurrentPinFinish: false })
                    .OrderBy(p => p.Position).ThenByDescending(p => p.IsCurrentPinFinish).ToList();
                queueZonePositionsActivePin = new Queue<ZonePosition>();
                foreach (var zonePosition in zonePositionsChangeIsActive)
                {
                    queueZonePositionsActivePin.Enqueue(zonePosition);
                }

                while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item) && item != null)
                {
                    int position = 0;
                    while (true)
                    {
                        if (position >= zonePositionsResult.Length)
                        {
                            break;
                        }

                        if (zonePositionsResult[position] == null)
                        {
                            item.Position = position;
                            zonePositionsResult[position] = item;
                            break;
                        }

                        if (position > zonePositionsResult.Length)
                        {
                            break;
                        }

                        position += 1;
                    }
                }
            }

            zonePositionsResult = zonePositionsResult.Where(p => p != null).ToArray();
            LastOrder += 1;
            return zonePositionsResult.ToList()!;
        }

        public bool PinStarted(out ZonePosition[] zonePositionsSort)
        {
            zonePositionsSort = [];
            List<ZonePosition> zonePositionsChange = new List<ZonePosition>();
            if (!(ZonePositions?.Count > 0)) return false;
            foreach (var zonePosition in ZonePositions)
            {
                var isChange = zonePosition.PinStarted();
                if (isChange)
                {
                    zonePositionsChange.Add(zonePosition);
                }
            }

            if (zonePositionsChange.Count <= 0)
            {
                return false;
            }

            int totalPosition = TotalPosition * 2;
            if (totalPosition < 50)
            {
                totalPosition = 100;
            }

            ZonePosition?[] zonePositionsResult = new ZonePosition[totalPosition + zonePositionsChange.Count];
            var zonePositionsActivePin = ZonePositions
                .Where(p => p is { DisplayType: ZonePositionDisplayTypeEnum.Active, IsPin: true }).OrderBy(p => p.BeginDate)
                .ToList();
            HashSet<string> idsAdd = new HashSet<string>();
            Queue<ZonePosition> queueZonePositionsActivePin = new Queue<ZonePosition>();
            foreach (var zonePosition in zonePositionsActivePin)
            {
                queueZonePositionsActivePin.Enqueue(zonePosition);
            }

            while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item))
            {
                int position = item.Position;
                while (true)
                {
                    if (position >= zonePositionsResult.Length)
                    {
                        break;
                    }

                    if (zonePositionsResult[position] == null)
                    {
                        item.Position = position;
                        zonePositionsResult[position] = item;
                        idsAdd.Add(item.Id);
                        break;
                    }

                    if (position > zonePositionsResult.Length)
                    {
                        break;
                    }

                    position += 1;
                }
            }

            var zonePositionsChangeIsTimer = zonePositionsChange.Where(p => p.IsPin == false)
                .OrderBy(p => p.BeginDate).ToList();
            queueZonePositionsActivePin = new Queue<ZonePosition>();
            foreach (var zonePosition in zonePositionsChangeIsTimer)
            {
                queueZonePositionsActivePin.Enqueue(zonePosition);
            }

            while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item))
            {
                int position = item.Position;
                while (true)
                {
                    if (position >= zonePositionsResult.Length)
                    {
                        break;
                    }

                    if (zonePositionsResult[position] == null)
                    {
                        item.Position = position;
                        zonePositionsResult[position] = item;
                        idsAdd.Add(item.Id);
                        break;
                    }

                    if (position > zonePositionsResult.Length)
                    {
                        break;
                    }

                    position += 1;
                }
            }

            var zonePositionsChangeIsActive = ZonePositions
                .Where(p => p is { DisplayType: ZonePositionDisplayTypeEnum.Active, IsPin: false } &&
                            !idsAdd.Contains(p.Id))
                .OrderBy(p => p.Position).ToList();
            queueZonePositionsActivePin = new Queue<ZonePosition>();
            foreach (var zonePosition in zonePositionsChangeIsActive)
            {
                queueZonePositionsActivePin.Enqueue(zonePosition);
            }

            while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item))
            {
                int position = item.Position;
                while (true)
                {
                    if (position >= zonePositionsResult.Length)
                    {
                        break;
                    }

                    if (zonePositionsResult[position] == null)
                    {
                        item.Position = position;
                        zonePositionsResult[position] = item;
                        break;
                    }

                    if (position > zonePositionsResult.Length)
                    {
                        break;
                    }

                    position += 1;
                }
            }

            zonePositionsSort = zonePositionsResult.Where(p => p != null).Select(p => p!).ToArray();
            var positionsIsPin = zonePositionsSort.Where(p => p.IsPin).Select(p => p.Position).ToArray();
            int i = 0;
            foreach (var position in zonePositionsSort)
            {
                if (position.IsPin)
                {
                    continue;
                }

                while (true)
                {
                    if (positionsIsPin.Contains(i))
                    {
                        i++;
                    }
                    else
                    {
                        break;
                    }
                }

                position!.Position = i;
                i++;
            }

            LastOrder += 1;
            return true;
        }

        public void EventZonePositionsChange(ZonePositionChangeCommand command)
        {
            if (command.Items is not { Length: > 0 })
            {
                return;
            }

            var items = command.Items
                .OrderBy(p => p.Position)
                .ToArray();
            foreach (var commandItem in items)
            {
                ZonePosition? zonePosition =
                    ZonePositions.FirstOrDefault(p =>
                        p.ObjectId.Length > 0 && commandItem.ObjectId.Length > 0 && p.ObjectId == commandItem.ObjectId);
                if (zonePosition == null)
                {
                    if (commandItem.Status)
                    {
                        ZonePosition zonePositionAdd = new ZonePosition(commandItem, command, this);
                        ZonePositions.Add(zonePositionAdd);
                    }
                }
                else
                {
                    if (commandItem.Status)
                    {
                        zonePosition.Change(commandItem, command, this);
                    }
                    else
                    {
                        zonePosition.Remove(command);
                    }
                }
            }

            foreach (var zonePosition in ZonePositions)
            {
                if (string.IsNullOrEmpty(zonePosition.ObjectId))
                {
                    zonePosition.Remove(command);
                }
            }

            ZonePositions = ZonePositions.OrderBy(p => p.Position).ThenByDescending(p => p.UpdatedDate).ToList();
            int totalPosition = TotalPosition * 2;
            if (totalPosition < 50)
            {
                totalPosition = 100;
            }

            int i = 0;
            foreach (var zonePosition in ZonePositions)
            {
                if (zonePosition.Status == ActiveStatusEnum.Cancel)
                {
                    continue;
                }

                if (i > totalPosition)
                {
                    zonePosition.Remove(command);
                    continue;
                }

                zonePosition.Position = i;
                i++;
            }

            if (ZonePositions?.Count > 0)
            {
                LastOrder = ZonePositions.Max(p => p.Order);
            }

            LastOrder += 1;
        }

        public void SortActive(ZonePositionChangeCommand command)
        {
            // if (command.Items is not { Length: > 0 })
            // {
            //     throw new Exception("Item sort not null");
            // }
            command.Items ??= [];

            // chuyển trạng thái các phần tử remove
            List<ZonePosition> zonePositionsRemove = [];
            foreach (var item in command.Items)
            {
                if (item.Status)
                {
                    continue;
                }

                var itemRemove = ZonePositions?.FirstOrDefault(p =>
                                     p.ObjectId.Length > 0 && item.ObjectId.Length > 0 && p.ObjectId == item.ObjectId) ??
                                 ZonePositions?.FirstOrDefault(p => p.Id == item.Id);
                if (itemRemove != null)
                {
                    itemRemove.Remove(command);
                    zonePositionsRemove.Add(itemRemove);
                }
            }

            // tạo hash các phần từ ID hoặc NewsId để check các phần tử đã xử lý
            HashSet<string> idsAdd = new HashSet<string>();
            HashSet<string> objectIdsAdd = new HashSet<string>();
            // Khởi tạo số phần tử sort
            int totalPosition = TotalPosition + 10;
            ZonePosition?[] zonePositionsSortResult = new ZonePosition[totalPosition];
            // xử lý các phần từ IsPin
            // lấy danh sách các phần từ IsPin ko bị sửa đổi
            var zonePositionsIsPin =
                ZonePositions?.Where(p =>
                        p is
                        {
                            DisplayType: ZonePositionDisplayTypeEnum.Active, IsPin: true, Status: ActiveStatusEnum.Approved
                        } &&
                        command.Items.All(q => q.Id != p.Id))
                    .OrderBy(p => p.Position)
                    .ToList();
            // tạo queue các phần từ IsPin không bị sửa đổi để đưa vào vị trí
            if (zonePositionsIsPin?.Count > 0)
            {
                Queue<ZonePosition> queueZonePositionsActivePin = new Queue<ZonePosition>();
                foreach (var zonePosition in zonePositionsIsPin)
                {
                    queueZonePositionsActivePin.Enqueue(zonePosition);
                }

                while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item))
                {
                    int position = item.Position;
                    while (true)
                    {
                        if (position >= totalPosition)
                        {
                            break;
                        }

                        if (zonePositionsSortResult[position] == null)
                        {
                            item.Position = position;
                            zonePositionsSortResult[position] = item;
                            idsAdd.Add(item.Id);
                            objectIdsAdd.Add(item.ObjectId);
                            break;
                        }

                        if (position > totalPosition)
                        {
                            break;
                        }

                        position += 1;
                    }
                }
            }

            // các phần từ thêm mới hoặc thay đổi
            var items = command.Items
                .Where(p =>
                    (p.BeginDateTime == null || p.BeginDateTime <= DateTime.Now) && p.Status
                )
                .OrderBy(p => p.Position)
                .ThenByDescending(p => p.Priority)
                .ToArray();
            // các phần từ thêm mới hoặc thay đổi hẹn giờ
            var itemsTimer = command.Items
                .Where(p => p.BeginDateTime > DateTime.Now)
                .OrderBy(p => p.Position)
                .ThenByDescending(p => p.Priority)
                .ToArray();
            // xử lý các phần tử IsPin trước
            foreach (var commandItem in items)
            {
                ZonePosition? zonePosition =
                    ZonePositions?.FirstOrDefault(p =>
                        p.ObjectId.Length > 0 && commandItem.ObjectId.Length > 0 && p.ObjectId == commandItem.ObjectId) ??
                    ZonePositions?.FirstOrDefault(p => p.Id == commandItem.Id);
                if (zonePosition is { IsPin: true })
                {
                    zonePosition.Change(commandItem, command, this);
                    if (idsAdd.Contains(zonePosition.Id))
                    {
                        continue;
                    }

                    int position = zonePosition.Position;
                    while (true)
                    {
                        if (position >= totalPosition)
                        {
                            break;
                        }

                        if (zonePositionsSortResult[position] == null)
                        {
                            zonePosition.Position = position;
                            zonePositionsSortResult[position] = zonePosition;
                            idsAdd.Add(zonePosition.Id);
                            objectIdsAdd.Add(zonePosition.ObjectId);
                            break;
                        }

                        if (position > totalPosition)
                        {
                            break;
                        }

                        position += 1;
                    }
                }
            }

            // xử lý các phần tử không IsPin
            foreach (var commandItem in items)
            {
                ZonePosition? zonePosition =
                    ZonePositions?.FirstOrDefault(p =>
                        p.ObjectId.Length > 0 && commandItem.ObjectId.Length > 0 && p.ObjectId == commandItem.ObjectId) ??
                    ZonePositions?.FirstOrDefault(p => p.Id == commandItem.Id);
                if (zonePosition == null)
                {
                    ZonePosition zonePositionAdd = new ZonePosition(commandItem, command, this);
                    int position = zonePositionAdd.Position;
                    while (true)
                    {
                        if (position >= totalPosition)
                        {
                            break;
                        }

                        if (zonePositionsSortResult[position] == null)
                        {
                            zonePositionAdd.Position = position;
                            zonePositionsSortResult[position] = zonePositionAdd;
                            idsAdd.Add(zonePositionAdd.Id);
                            objectIdsAdd.Add(zonePositionAdd.ObjectId);
                            break;
                        }

                        if (position > totalPosition)
                        {
                            break;
                        }

                        position += 1;
                    }
                }
                else
                {
                    if (idsAdd.Contains(zonePosition.Id))
                    {
                        continue;
                    }

                    if (zonePosition.IsPin)
                    {
                        continue;
                    }

                    zonePosition.Change(commandItem, command, this);
                    int position = zonePosition.Position;
                    while (true)
                    {
                        if (position >= totalPosition)
                        {
                            break;
                        }

                        if (zonePositionsSortResult[position] == null)
                        {
                            zonePosition.Position = position;
                            zonePositionsSortResult[position] = zonePosition;
                            idsAdd.Add(zonePosition.Id);
                            objectIdsAdd.Add(zonePosition.ObjectId);
                            break;
                        }

                        if (position > totalPosition)
                        {
                            break;
                        }

                        position += 1;
                    }
                }
            }

            if (itemsTimer?.Length > 0)
            {
                foreach (var commandItem in itemsTimer)
                {
                    if (commandItem.Id?.Length > 0)
                    {
                        if (idsAdd.Contains(commandItem.Id))
                        {
                            continue;
                        }

                        idsAdd.Add(commandItem.Id);
                        ZonePosition? zonePositionRemove =
                            ZonePositions?.FirstOrDefault(p =>
                                p.ObjectId.Length > 0 && commandItem.ObjectId.Length > 0 &&
                                p.ObjectId == commandItem.ObjectId) ??
                            ZonePositions?.FirstOrDefault(p => p.Id == commandItem.Id);
                        if (zonePositionRemove != null)
                        {
                            zonePositionRemove.Remove(command);
                        }
                    }
                }
            }

            // xử lý các phần tử cũ
            var zonePositionsIsActive = ZonePositions?
                .Where(p =>
                    p.DisplayType == ZonePositionDisplayTypeEnum.Active &&
                    p.IsPin == false &&
                    p.Status == ActiveStatusEnum.Approved &&
                    !idsAdd!.Contains(p.Id)
                )
                .OrderBy(p => p.Position).ToList();
            if (zonePositionsIsActive?.Count > 0)
            {
                var queueZonePositionsActivePin = new Queue<ZonePosition>();
                foreach (var zonePositionIsActive in zonePositionsIsActive)
                {
                    queueZonePositionsActivePin.Enqueue(zonePositionIsActive);
                }

                while (queueZonePositionsActivePin.TryDequeue(out ZonePosition? item))
                {
                    if (idsAdd.Contains(item.Id))
                    {
                        continue;
                    }

                    int position = 0;
                    while (true)
                    {
                        if (position >= totalPosition)
                        {
                            break;
                        }

                        if (zonePositionsSortResult[position] == null)
                        {
                            item.Position = position;
                            zonePositionsSortResult[position] = item;
                            idsAdd.Add(item.Id);
                            objectIdsAdd.Add(item.ObjectId);
                            break;
                        }

                        if (position > totalPosition)
                        {
                            break;
                        }

                        position += 1;
                    }
                }
            }

            zonePositionsSortResult = zonePositionsSortResult.Where(p => p != null).OrderBy(p => p!.Position).ToArray();
            // săp xếp lại toàn bộ thứ tự
            int i = 0;
            foreach (var zonePosition in zonePositionsSortResult)
            {
                zonePosition!.ChangePosition(i, this);
                i++;
            }

            List<ZonePosition> zonePositionsTimer = new List<ZonePosition>();
            if (itemsTimer?.Length > 0)
            {
                foreach (var commandItem in itemsTimer)
                {
                    ZonePosition? zonePosition =
                        ZonePositions?.FirstOrDefault(p =>
                            p.ObjectId.Length > 0 && commandItem.ObjectId.Length > 0 &&
                            p.ObjectId == commandItem.ObjectId) ??
                        ZonePositions?.FirstOrDefault(p => p.Id == commandItem.Id);
                    if (zonePosition == null)
                    {
                        ZonePosition zonePositionAdd =
                            new ZonePosition(commandItem, command, this);
                        zonePositionAdd.DisplayTypeChange(ZonePositionDisplayTypeEnum.Timer);
                        zonePositionsTimer.Add(zonePositionAdd);
                    }
                    else
                    {
                        if (commandItem.Status)
                        {
                            zonePosition.Change(commandItem, command, this);
                        }
                        else
                        {
                            zonePosition.Remove(command);
                        }

                        zonePosition.DisplayTypeChange(ZonePositionDisplayTypeEnum.Timer);
                        zonePositionsTimer.Add(zonePosition);
                    }
                }
            }

            ZonePositions = ZonePositions!.Where(p => p.DisplayType == ZonePositionDisplayTypeEnum.Active).ToList();
            if (ZonePositions.Count > 0)
            {
                foreach (var zonePosition in ZonePositions)
                {
                    if (!idsAdd.Contains(zonePosition.Id))
                    {
                        zonePosition.Remove(command);
                    }
                }
            }


            ZonePositions = ZonePositions!.Where(p => p.Status == ActiveStatusEnum.Cancel).ToList();
            if (zonePositionsSortResult.Length > 0)
            {
                foreach (var zonePosition in zonePositionsSortResult)
                {
                    ZonePositions.Add(zonePosition!);
                }
            }

            if (zonePositionsTimer.Count > 0)
            {
                foreach (var zonePosition in zonePositionsTimer)
                {
                    ZonePositions.Add(zonePosition!);
                }
            }

            if (zonePositionsRemove?.Count > 0)
            {
                foreach (var zonePosition in zonePositionsRemove)
                {
                    if (ZonePositions.All(p => p.Id != zonePosition.Id))
                    {
                        ZonePositions.Add(zonePosition!);
                    }
                }
            }


            if (ZonePositions?.Count > 0)
            {
                LastOrder = ZonePositions.Max(p => p.Order);
            }
        }

        public void SortTimer(ZonePositionChangeCommand command)
        {
            if (command?.Items == null || command.Items.Length <= 0)
            {
                throw new Exception("Item sort not null");
            }

            ZonePositions ??= [];
            var items = command.Items
                .OrderBy(p => p.Position)
                .ToArray();
            foreach (var commandItem in items)
            {
                ZonePosition? zonePosition =
                    ZonePositions.FirstOrDefault(p =>
                        p.ObjectId.Length > 0 && commandItem.ObjectId.Length > 0 && p.ObjectId == commandItem.ObjectId) ??
                    ZonePositions.FirstOrDefault(p => p.Id == commandItem.Id);
                if (zonePosition == null)
                {
                    if (commandItem.Status)
                    {
                        ZonePosition zonePositionAdd = new ZonePosition(commandItem, command, this);
                        ZonePositions.Add(zonePositionAdd);
                    }
                }
                else
                {
                    if (commandItem.Status)
                    {
                        zonePosition.Change(commandItem, command, this);
                    }
                    else
                    {
                        zonePosition.Remove(command);
                    }
                }
            }

            foreach (var zonePosition in ZonePositions)
            {
                if (string.IsNullOrEmpty(zonePosition.ObjectId))
                {
                    zonePosition.Remove(command);
                }
            }

            ZonePositions = ZonePositions.Where(p =>
                p.DisplayType == command.DisplayType || (command.DisplayType == ZonePositionDisplayTypeEnum.Pin && p.IsPin)
            ).ToList();
            if (ZonePositions?.Count > 0)
            {
                LastOrder = ZonePositions.Max(p => p.Order);
            }
        }

        public void RemoveByObjectId(ZonePositionRemoveFromNewsCommand command)
        {
            if (command.ObjectId is not { Length: > 0 })
            {
                throw new Exception("Item remove not null");
            }

            if (ZonePositions is not { Count: > 0 })
            {
                return;
            }

            foreach (var zonePosition in ZonePositions)
            {
                if (zonePosition.ObjectId == command.ObjectId)
                {
                    zonePosition.Remove(command);
                }
            }

            var zonePositionsActive =
                ZonePositions.Where(p => p.DisplayType == ZonePositionDisplayTypeEnum.Active).OrderBy(p => p.Position)
                    .ToList();
            if (zonePositionsActive?.Count > 0)
            {
                int i = 0;
                foreach (var zonePosition in zonePositionsActive)
                {
                    if (zonePosition.Status != ActiveStatusEnum.Approved)
                    {
                        continue;
                    }

                    zonePosition!.ChangePosition(i, this);
                    i++;
                }
            }
        }

        public new string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public StatusEnum Status { get; set; }
        public int Priority { get; set; }
        public string DealerId { get; set; }
        public int TotalPosition { get; set; }
        public string LockUserId { get; set; }
        public string LockSocketId { get; set; }
        public long LastOrder { get; set; }
        public string CategoryId { get; set; }
        public string Thumbnail { get; set; }
        public List<ZonePosition>? ZonePositions { get; set; }
        public List<ZonePosition> ZonePositionsTimer { get; set; }
        public string ObjectId { get; set; }
        public ZoneObjectTypeEnum ObjectType { get; set; }
        public string MappingKey { get; set; }
        public string GroupId { get; set; }

        public MobilePageEnum MobilePage { get; set; }
        public long AutoSetPositionTime { get; set; }
        public int PRExpiredPosition { get; set; }
        public ZoneOptionEnum Options { get; set; }

        public ZoneChangeEvent ToEvent()
        {
            return new ZoneChangeEvent()
            {
                Id = Id
            };
        }

        public CacheChangeEvent ToCacheEvent()
        {
            return new CacheChangeEvent()
            {
                Id = Id,
                CacheType = KeyCacheTypeEnum.Zone,
                IsTrigger = true
            };
        }

        public ZonePositionChangeEvent ToZonePositionChangeEvent()
        {
            return new ZonePositionChangeEvent()
            {
                ZoneId = Id,
                Items = ZonePositions?.Where(p => p.Status == ActiveStatusEnum.Approved)
                    .Select(p => p.ToEvent())
                    .ToArray()
            };
        }
    }
}
