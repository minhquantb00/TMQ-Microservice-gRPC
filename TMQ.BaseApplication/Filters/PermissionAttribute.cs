using Microsoft.AspNetCore.Mvc;

namespace TMQ.BaseApplication.Filters
{
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(string group, string name, bool isRoot, string key = "") : base(
            typeof(PermissionFilter))
        {
            this.Arguments = [group, name, isRoot, key];
        }
    }
}
