using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;
using TMQ.SystemCommands.Queries;
using TMQ.SystemDomains;
using TMQ.SystemReadModels;

namespace TMQ.SystemRepository
{
    public interface IMenuRepository
    {
        public Task<RMenu[]> Gets(string dealerId);
        public Task<RMenu[]> Gets(MenuSearchQuery menu);
        public Task<RMenu[]> GetsByDisplayPermission(MenusGetByDisplayPermissionQuery query);
        public Task Add(AdminMenu menu);
        public Task Change(AdminMenu menu);
        public Task Change(AdminMenu[] menus);
        public Task Delete(AdminMenu menu);
        Task<RMenu[]?> GetByPosition(MenuPosition menuPosition);
        Task<RMenu[]?> GetByPosition(MenuPosition[] menuPositions);
        public Task<RMenu?> GetById(string id);
        public Task<RMenu[]> GetChildrenById(string id);
    }
}
