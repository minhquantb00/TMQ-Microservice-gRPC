using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseCommands;
using TMQ.BaseDomains;
using TMQ.Common;
using TMQ.EnumDefine;
using TMQ.SystemCommands.Commands;
using TMQ.SystemCommands.Events;
using TMQ.SystemReadModels;

namespace TMQ.SystemDomains
{
    [Table("ZonePosition_tbl")]
    public class ZonePosition : BaseDomain
    {
        public ZonePosition(RZonePosition zonePosition) : base(zonePosition)
        {
            Id = zonePosition.Id;
            Type = zonePosition.Type;
            ObjectId = zonePosition.ObjectId;
            ZoneId = zonePosition.ZoneId;
            Status = zonePosition.Status;
            Position = zonePosition.Position;
            Priority = zonePosition.Priority;
            BeginDate = zonePosition.BeginDate;
            EndDate = zonePosition.EndDate;
            IsPin = zonePosition.IsPin;
            Order = zonePosition.Order;
            PinFinish = zonePosition.PinFinish;
            PinStart = zonePosition.PinStart;
            IsAuto = zonePosition.IsAuto;
            DisplayTime = zonePosition.DisplayTime;
            OffTime = zonePosition.OffTime;
            DisplayType = zonePosition.DisplayType;
        }

        public ZonePosition(ZonePositionItemChangeCommand commandItem, ZonePositionChangeCommand command, Zone zone) :
            base(command)
        {
            Id = CommonUtility.GenerateGuid();
            Type = commandItem.Type;
            ObjectId = commandItem.ObjectId;
            ZoneId = zone.Id;
            Status = ActiveStatusEnum.Approved;
            Position = commandItem.Position;
            Priority = commandItem.Priority;
            BeginDate = commandItem.BeginDateTime;
            EndDate = commandItem.EndDateTime;
            //IsPin = commandItem.IsPin;
            Order = zone.LastOrder + (zone.TotalPosition + 1 - commandItem.Position);
            PinFinish = false;
            PinStart = false;
            DisplayType = command.DisplayType;
        }

        public ZonePosition(ZonePositionItemChangeFromNewsCommand commandItem,
            ZonePositionChangeFromNewsCommand command, Zone zone) : base(command)
        {
            Id = CommonUtility.GenerateGuid();
            Type = ZonePositionType.Article;
            ObjectId = command.ObjectId;
            ZoneId = commandItem.ZoneId;
            Status = ActiveStatusEnum.Approved;
            Position = commandItem.Position;
            Priority = 0;
            BeginDate = commandItem.BeginDateTime;
            EndDate = commandItem.EndDateTime;
            //IsPin = commandItem.BeginDateTime.HasValue;
            Order = zone.LastOrder + (zone.TotalPosition + 1 - commandItem.Position);
            PinFinish = false;
            PinStart = false;
            DisplayType = commandItem.DisplayType;
        }

        public void Change(ZonePositionItemChangeCommand commandItem, ZonePositionChangeCommand command, Zone zone)
        {
            Type = commandItem.Type;
            ObjectId = commandItem.ObjectId;
            ZoneId = zone.Id;
            Status = ActiveStatusEnum.Approved;
            Position = commandItem.Position;
            Priority = commandItem.Priority;
            BeginDate = commandItem.BeginDateTime;
            EndDate = commandItem.EndDateTime;
            Order = zone.LastOrder + (zone.TotalPosition + 1 - commandItem.Position);
            DisplayType = command.DisplayType;
            IsPin = BeginDate.HasValue && EndDate.HasValue;
            Changed(command);
        }

        public void Change(ZonePositionItemChangeFromNewsCommand commandItem, ZonePositionChangeFromNewsCommand command,
            Zone zone)
        {
            Status = ActiveStatusEnum.Approved;
            Position = commandItem.Position;
            Priority = 0;
            BeginDate = commandItem.BeginDateTime;
            EndDate = commandItem.EndDateTime;
            Order = zone.LastOrder + (zone.TotalPosition + 1 - commandItem.Position);
            DisplayType = commandItem.DisplayType;
            //IsPin = BeginDate.HasValue && EndDate.HasValue;
            Changed(command);
        }

        public void ChangePosition(int position, Zone zone)
        {
            Position = position;
            Order = zone.LastOrder + (zone.TotalPosition + 1 - Position);
        }

        public void Remove(BaseCommand command)
        {
            Status = ActiveStatusEnum.Cancel;
            OffTime = command.ProcessDate;
            Changed(command);
        }

        public bool PinFinishedRemove()
        {
            if (!BeginDate.HasValue || !EndDate.HasValue || EndDate.Value >= DateTime.Now) return false;
            PinFinish = true;
            EndDate = null;
            IsPin = false;
            IsCurrentPinFinish = true;
            Status = ActiveStatusEnum.Cancel;
            return true;
        }

        public bool PinFinished(int positionAutoChange, int position, int totalPosition)
        {
            if (!BeginDate.HasValue || !EndDate.HasValue || EndDate.Value >= DateTime.Now) return false;
            if (positionAutoChange < 0 || position < 0)
            {
                PinFinish = true;
                PositionOld = Position;
                Position = totalPosition + 1;
                EndDate = null;
                IsPin = false;
                IsCurrentPinFinish = true;
                return true;
            }

            if (Position > positionAutoChange)
            {
                PinFinish = true;
                PositionOld = Position;
                EndDate = null;
                IsPin = false;
                IsCurrentPinFinish = true;
                return true;
            }

            PinFinish = true;
            PositionOld = Position;
            Position = position;
            EndDate = null;
            IsPin = false;
            IsCurrentPinFinish = true;
            return true;
        }

        public bool PinStarted()
        {
            bool result = false;
            if (BeginDate.HasValue && BeginDate.Value <= DateTime.Now &&
                DisplayType == ZonePositionDisplayTypeEnum.Timer)
            {
                PinStart = true;
                DisplayType = ZonePositionDisplayTypeEnum.Active;
                result = true;
            }

            if (BeginDate.HasValue && BeginDate.Value <= DateTime.Now && DisplayType == ZonePositionDisplayTypeEnum.Pin)
            {
                PinStart = true;
                DisplayType = ZonePositionDisplayTypeEnum.Active;
                IsPin = true;
                result = true;
            }

            return result;
        }

        public bool IsChange(ZonePositionItemChangeFromNewsCommand command)
        {
            if (command.Status && Status != ActiveStatusEnum.Approved)
            {
                return true;
            }

            if (!command.Status && Status == ActiveStatusEnum.Approved)
            {
                return true;
            }

            if (command.Position != Position)
            {
                return true;
            }

            if (command.DisplayType != DisplayType)
            {
                return true;
            }

            if (command.BeginDateTime != BeginDate)
            {
                return true;
            }

            if (command.EndDateTime != EndDate)
            {
                return true;
            }

            if (command.ZonePositionId != Id)
            {
                return true;
            }

            return false;
        }

        public void DisplayTypeChange(ZonePositionDisplayTypeEnum displayType)
        {
            DisplayType = displayType;
        }

        public new string Id { get; set; }
        public ZonePositionType Type { get; set; }
        public string ObjectId { get; set; }
        public string ZoneId { get; set; }
        public ActiveStatusEnum Status { get; set; }
        public int Position { get; set; }
        public int PositionOld { get; set; }
        public int Priority { get; set; }
        public DateTime? BeginDate { get; set; }
        public DateTime? EndDate { get; set; }
        public long Order { get; set; }
        public bool IsPin { get; set; }
        public bool PinFinish { get; set; }
        public bool PinStart { get; set; }
        public bool IsAuto { get; set; }
        public DateTime? DisplayTime { get; set; }
        public DateTime? OffTime { get; set; }

        public ZonePositionDisplayTypeEnum DisplayType { get; set; }
        public bool IsCurrentPinFinish { get; set; }

        public ZonePositionItemChangeEvent ToEvent()
        {
            return new ZonePositionItemChangeEvent()
            {
                Id = Id,
                Order = Order,
                Position = Position,
                Priority = Priority,
                Status = Status,
                Type = Type,
                BeginDate = BeginDate,
                EndDate = EndDate,
                //IsPin = IsPin,
                ZoneId = ZoneId,
                ObjectId = ObjectId,
            };
        }

        public ZonePosition Clone()
        {
            return (ZonePosition)MemberwiseClone();
        }

        public RZonePosition ToRZonePosition(string dealerId)
        {
            return new RZonePosition()
            {
                Code = Code,
                Id = Id,
                Order = Order,
                Position = Position,
                Priority = Priority,
                Status = Status,
                Type = Type,
                Version = Version,
                BeginDate = BeginDate,
                CreatedDate = CreatedDate,
                CreatedUid = CreatedUid,
                DisplayTime = DisplayTime,
                DisplayType = DisplayType,
                EndDate = EndDate,
                IsAuto = IsAuto,
                IsPin = IsPin,
                LoginUid = LoginUid,
                NumericalOrder = NumericalOrder,
                ObjectId = ObjectId,
                OffTime = OffTime,
                PinFinish = PinFinish,
                PinStart = PinStart,
                UpdatedDate = UpdatedDate,
                UpdatedUid = UpdatedUid,
                ZoneId = ZoneId,
                CreatedDateUtc = CreatedDateUtc,
                UpdatedDateUtc = UpdatedDateUtc,
                DealerId = dealerId
            };
        }

        public ZonePositionHistoryItemEvent ZonePositionHistoryItemEvent()
        {
            return new ZonePositionHistoryItemEvent()
            {
                Position = Position,
                Priority = Priority,
                Status = Status,
                Type = Type,
                BeginDate = BeginDate,
                EndDate = EndDate,
                IsAuto = IsAuto,
                IsPin = IsPin,
                LoginUid = LoginUid,
                Order = Order,
                ObjectId = ObjectId,
                OffTime = OffTime,
                PinFinish = PinFinish,
                PinStart = PinStart,
                Version = Version,
                DisplayTime = DisplayTime,
                DisplayType = DisplayType,
                ZoneId = ZoneId,
            };
        }
    }
}
