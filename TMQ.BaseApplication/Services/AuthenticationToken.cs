using StackExchange.Redis;
using TMQ.Cache;
using TMQ.Common;
using TMQ.Config;

namespace TMQ.BaseApplication.Services
{
    public class AuthenticationToken(
    IHttpContextAccessor httpContextAccessor,
    RedisConnection redisConnectionPersistence)
    {
        private IDatabase? WriteDatabase => redisConnectionPersistence.GetDatabase(_redisDbId);

        private readonly int _redisDbId = string.IsNullOrEmpty(ConfigSettingEnum.RedisPersistenceDbId.GetConfig())
            ? 2
            : ConfigSettingEnum.RedisPersistenceDbId.GetConfig().AsInt(2);

        private static string StorageKey(string sessionId)
        {
            return $"AuthenticationToken_{sessionId}";
        }

        private string? _sessionKey;

        private string? SessionKeyGet()
        {
            _sessionKey = httpContextAccessor.HttpContext?.User.FindFirst(ContextService.SessionCode)?.Value;
            return _sessionKey;
        }

        public async Task<string?> GetToken()
        {
            string? result = null;
            string? sessionId = SessionKeyGet();
            if (sessionId == null)
            {
                return result;
            }

            string key = StorageKey(sessionId);
            var database = WriteDatabase;
            if (database == null)
            {
                return result;
            }

            RedisValue value = await database.StringGetAsync(key);
            if (value.HasValue)
            {
                result = value;
            }

            return result;
        }

        public async Task<string?> GetToken(string sessionId)
        {
            string key = StorageKey(sessionId);
            RedisValue value = await WriteDatabase.StringGetAsync(key);
            if (value.HasValue)
            {
                return value;
            }

            return string.Empty;
        }
    }
}
