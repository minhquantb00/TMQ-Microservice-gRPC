using Microsoft.AspNetCore.Mvc.Filters;
using TMQ.Common;
using TMQ.Config;

namespace TMQ.BaseApplication.Filters
{
    public class PermissionFilter(string group, string name, bool isRoot, string key, ILogger<PermissionFilter> logger)
    : IAsyncActionFilter
    {
        private readonly ILogger<PermissionFilter> _logger = logger;
        private static readonly HashSet<string> KeysExist = new HashSet<string>();
        private static readonly string KeyPrefix = ConfigSettingEnum.ActionKeyPrefix.GetConfig().AsEmpty();
        private static readonly string AdminWebsiteIdConfig = ConfigSettingEnum.AdminWebsiteIdConfig.GetConfig().AsEmpty();

        private string Group { get; } = group;
        private string Name { get; } = name;
        private bool IsRoot { get; } = isRoot;
        private string Key { get; } = key;

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
        }
    }
}
