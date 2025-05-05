using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseDomains;
using TMQ.Common;
using TMQ.EnumDefine;
using TMQ.SystemCommands.Events;
using TMQ.SystemReadModels;

namespace TMQ.SystemDomains
{
    [Table("ZonePositionHistory_tbl")]
    public class ZonePositionHistory : BaseDomain
    {
        public ZonePositionHistory(string changeId, string zoneId, string createdUid, DateTime createdDate, bool isPin)
        {
            Code = CommonUtility.GenerateGuid();
            CreatedUid = createdUid;
            CreatedDate = createdDate;
            Position = 0;
            DisplayType = 0;
            ZoneId = zoneId;
            ChangeId = changeId;
            IsAuto = true;
            IsPin = isPin;
        }

        public ZonePositionHistory(string changeId, bool isAuto, ZonePositionHistoryItemEvent @event, string createdUid,
            DateTime createdDate) : base(@event)
        {
            Code = CommonUtility.GenerateGuid();
            CreatedUid = createdUid;
            CreatedDate = createdDate;
            Position = @event.Position;
            DisplayType = @event.DisplayType;
            ZoneId = @event.ZoneId;
            ChangeId = changeId;
            ObjectIdOld = @event.ObjectId;
            ZonePositionOld = ToRModel(@event);
            IsAuto = isAuto;
            TypeOld = @event.Type;
            IsPin = @event.IsPin;
        }

        public ZonePositionHistory(string changeId, bool isAuto, ZonePositionActionTypeEnum actionType,
            ZonePositionHistoryItemEvent @event, string createdUid, DateTime createdDate) : base(@event)
        {
            Code = CommonUtility.GenerateGuid();
            CreatedUid = createdUid;
            CreatedDate = createdDate;
            Position = @event.Position;
            DisplayType = @event.DisplayType;
            ZoneId = @event.ZoneId;
            ChangeId = changeId;
            ObjectIdNew = @event.ObjectId;
            ActionType = actionType;
            ZonePositionNew = ToRModel(@event);
            IsAuto = isAuto;
            TypeNew = @event.Type;
            IsPin = @event.IsPin;
        }

        public void Change(ZonePositionActionTypeEnum actionType, ZonePositionHistoryItemEvent @event)
        {
            ObjectIdNew = @event.ObjectId;
            ActionType = actionType;
            ZonePositionNew = ToRModel(@event);
            TypeNew = @event.Type;
            IsPin = @event.IsPin;
        }

        public int Position { get; set; }
        public ZonePositionDisplayTypeEnum DisplayType { get; set; }
        public string ZoneId { get; set; }
        public RZonePosition ZonePositionOld { get; set; }
        public RZonePosition ZonePositionNew { get; set; }
        public string ChangeId { get; set; }
        public string ObjectIdOld { get; set; }
        public string ObjectIdNew { get; set; }
        public ZonePositionActionTypeEnum ActionType { get; set; }
        public bool IsAuto { get; set; }
        public ZonePositionType TypeOld { get; set; }
        public ZonePositionType TypeNew { get; set; }
        public bool? IsPin { get; set; }

        public RZonePosition ToRModel(ZonePositionHistoryItemEvent @event)
        {
            return new RZonePosition()
            {
                Position = @event.Position,
                Priority = @event.Priority,
                Status = @event.Status,
                Type = @event.Type,
                BeginDate = @event.BeginDate,
                EndDate = @event.EndDate,
                IsAuto = @event.IsAuto,
                IsPin = @event.IsPin,
                LoginUid = @event.LoginUid,
                Order = @event.Order,
                ObjectId = @event.ObjectId,
                OffTime = @event.OffTime,
                PinFinish = @event.PinFinish,
                PinStart = @event.PinStart,
                Version = @event.Version,
                DisplayTime = @event.DisplayTime,
                DisplayType = @event.DisplayType,
                ZoneId = @event.ZoneId,
            };
        }
    }
}
