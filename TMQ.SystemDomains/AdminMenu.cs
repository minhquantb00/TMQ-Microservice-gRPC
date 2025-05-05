using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseDomains;
using TMQ.Common;
using TMQ.EnumDefine;
using TMQ.SystemCommands.Commands;
using TMQ.SystemCommands.Events;
using TMQ.SystemReadModels;

namespace TMQ.SystemDomains
{
    [Table("AdminMenu_tbl")]
    public class AdminMenu : BaseDomain
    {
        public AdminMenu(RMenu menu) : base(menu)
        {
            Code = menu.Code;
            ParentId = menu.ParentId;
            Name = menu.Name;
            Type = menu.Type;
            Url = menu.Url;
            ActionDefineId = menu.ActionDefineId;
            PositionId = menu.PositionId;
            Priority = menu.Priority;
            ObjectId = menu.ObjectId;
            Condition = menu.Condition;
            DealerId = menu.DealerId;
            CssClassIcon = menu.CssClassIcon;
            IsDisplayPermission = menu.IsDisplayPermission;
            SystemOption = menu.SystemOption;
            PathBase = menu.PathBase;
            BaseUrl = menu.BaseUrl;
            StartDate = menu.StartDate;
            EndDate = menu.EndDate;
        }

        public AdminMenu(MenuAddCommand command) : base(command)
        {
            Code = command.Id;
            ParentId = command.ParentId.AsEmpty();
            Name = command.Name;
            Type = command.Type;
            Url = command.Url;
            ActionDefineId = command.ActionDefineId;
            PositionId = command.PositionId;
            Priority = command.Priority;
            ObjectId = command.ObjectId.AsEmpty();
            Condition = string.Empty;
            DealerId = command.DealerId;
            Status = command.Status;
            CssClassIcon = command.CssClassIcon.AsEmpty();
            IsDisplayPermission = command.IsDisplayPermission;
            SystemOption = command.SystemOption;
            PathBase = command.PathBase;
            BaseUrl = command.BaseUrl;
            StartDate = command.StartDate;
            EndDate = command.EndDate;
        }

        public AdminMenu(MenuPermissionAddCommand command) : base(command)
        {
            Code = command.Id;
            ParentId = command.ParentId.AsEmpty();
            Name = command.Name;
            Type = command.Type;
            Url = command.Url;
            ActionDefineId = string.Empty;
            PositionId = command.PositionId;
            Priority = command.Priority;
            ObjectId = command.ObjectId.AsEmpty();
            Condition = string.Empty;
            DealerId = command.DealerId;
            Status = command.Status;
            CssClassIcon = string.Empty;
            IsDisplayPermission = command.IsDisplayPermission;
            StartDate = command.StartDate;
            EndDate = command.EndDate;
        }

        public AdminMenu(MenuAddToBookMarkCommand command) : base(command)
        {
            Code = command.Id;
            ParentId = string.Empty;
            Name = command.Name;
            Type = MenuTypeEnum.Link;
            Url = command.Url;
            ActionDefineId = string.Empty;
            PositionId = MenuPosition.MenuUser;
            Priority = 0;
            Status = StatusEnum.Active;
            ObjectId = command.ProcessUid;
            Condition = string.Empty;
            DealerId = command.DealerId;
            CssClassIcon = string.Empty;
        }

        public void Change(MenuChangeCommand command)
        {
            Code = command.Id;
            ParentId = command.ParentId.AsEmpty();
            Name = command.Name;
            Type = command.Type;
            Url = command.Url;
            ActionDefineId = command.ActionDefineId;
            PositionId = command.PositionId;
            Priority = command.Priority;
            Status = command.Status;
            ObjectId = command.ObjectId.AsEmpty();
            Condition = string.Empty;
            DealerId = command.DealerId;
            CssClassIcon = command.CssClassIcon.AsEmpty();
            IsDisplayPermission = command.IsDisplayPermission;
            SystemOption = command.SystemOption;
            PathBase = command.PathBase;
            BaseUrl = command.BaseUrl;
            StartDate = command.StartDate;
            EndDate = command.EndDate;
            Changed(command);
        }

        public void Change(MenuPermissionChangeCommand command)
        {
            Code = command.Id;
            ParentId = command.ParentId.AsEmpty();
            Name = command.Name;
            Type = command.Type;
            Url = command.Url;
            PositionId = command.PositionId;
            Priority = command.Priority;
            Status = command.Status;
            ObjectId = command.ObjectId.AsEmpty();
            Condition = string.Empty;
            DealerId = command.DealerId;
            StartDate = command.StartDate;
            EndDate = command.EndDate;
            Changed(command);
        }

        public void ChangeDisplayPermission(MenuChangeDisplayPermissionCommand command)
        {
            IsDisplayPermission = command.IsDisplayPermission;
        }

        #region Properties

        public string ParentId { get; set; }
        public string Name { get; set; }
        public MenuTypeEnum Type { get; set; }
        public string Url { get; set; }
        public string ActionDefineId { get; set; }
        public MenuPosition PositionId { get; set; }
        public int Priority { get; set; }
        public StatusEnum Status { get; set; }
        public string ObjectId { get; set; }
        public string Condition { get; set; }
        public string DealerId { get; set; }
        public string CssClassIcon { get; set; }
        public bool IsDisplayPermission { get; set; }
        public SystemOptionEnum SystemOption { get; set; }
        public string PathBase { get; set; }
        public string BaseUrl { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        #endregion

        public MenuChangeEvent ToEvent()
        {
            return new MenuChangeEvent()
            {
                Id = Id,
                PositionId = PositionId,
                IsTrigger = false,
            };
        }

        // public static CacheChangeEvent ToCacheEvent(MenuPosition position) => new CacheChangeEvent()
        // {
        //     Id = position.AsEnumToInt().ToString(),
        //     CacheType = KeyCacheTypeEnum.MenuPosition,
        //     IsTrigger = true,
        // };
    }
}
