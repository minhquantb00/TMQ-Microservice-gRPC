using StackExchange.Redis;
using TMQ.BaseReadModels;
using TMQ.Cache;
using TMQ.Common;
using TMQ.Config;
using TMQ.EnumDefine;

namespace TMQ.BaseApplication.Services
{
    public class AuthenService(RedisConnectionPersistence redisConnectionPersistence, ILogger<AuthenService> logger)
    {
        private IDatabase WriteDatabase()
        {
            var db = redisConnectionPersistence.GetDatabase(_dbId);
            if (db == null)
            {
                logger.LogError("Redis database could not be found for {DbId}", _dbId);
                throw new Exception($"Redis database could not be found for {_dbId}");
            }

            return db;
        }

        private readonly int _dbId = string.IsNullOrEmpty(ConfigSettingEnum.RedisPersistenceDbId.GetConfig())
            ? 2
            : ConfigSettingEnum.RedisPersistenceDbId.GetConfig().AsInt(2);

        private readonly string _redisCacheEnvironment = ConfigSettingEnum.DataProtectionRedisKey.GetConfig();
        private string GetKey(string key) => $"REDIS{_redisCacheEnvironment.AsEmpty()}_{key}";

        public async Task SetLoginInfo(string key, AccountLoginInfo accountLoginInfo, int minuteExpire)
        {
            TimeSpan timeOut = new TimeSpan(0, minuteExpire, 0);
            if (timeOut.TotalMilliseconds <= 0)
            {
                timeOut = new TimeSpan(0, accountLoginInfo.MinuteExpire, 0);
            }

            key = GetKey(key);
            RedisValue redisValue = ConvertInput(accountLoginInfo);
            var result = await WriteDatabase().StringSetAsync(key, redisValue, timeOut);
            return;
        }

        public async Task ChangeLoginInfo(string key, AccountLoginInfo accountLoginInfo)
        {
            TimeSpan timeOut = accountLoginInfo.InitDate.AddMinutes(accountLoginInfo.MinuteExpire) - DateTime.Now;
            if (timeOut.TotalMilliseconds <= 0)
            {
                timeOut = new TimeSpan(0, accountLoginInfo.MinuteExpire, 0);
            }

            key = GetKey(key);
            RedisValue redisValue = ConvertInput(accountLoginInfo);
            await WriteDatabase().StringSetAsync(key, redisValue, timeOut);
        }

        public async Task<AccountLoginInfo?> GetLoginInfo(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            key = GetKey(key);
            RedisValue value = await WriteDatabase().StringGetAsync(key);
            return ConvertOutput<AccountLoginInfo>(value);
        }

        public AccountLoginInfo? GetLoginInfoSync(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }

            key = GetKey(key);
            RedisValue value = WriteDatabase().StringGet(key);
            return ConvertOutput<AccountLoginInfo>(value);
        }

        public async Task RemoveLoginInfo(string key)
        {
            key = GetKey(key);
            await WriteDatabase().KeyDeleteAsync(key);
        }

        public async Task<bool> LoginInfoCheckExist(string key)
        {
            return await WriteDatabase().KeyExistsAsync(key);
        }

        public async Task<long> OtpLimit(string userId)
        {
            string key = $"OtpLimit_{userId}";
            long increment = await WriteDatabase().StringIncrementAsync(key, 1);
            await WriteDatabase().KeyExpireAsync(key, new TimeSpan(1, 0, 0));
            return increment;
        }

        public async Task OtpLimitReset(string userId)
        {
            string key = $"OtpLimit_{userId}";
            await WriteDatabase().KeyDeleteAsync(key);
        }

        public async Task ReCaptChaLimitReset(string userName)
        {
            string key = $"OtpLimit_{userName}";
            await WriteDatabase().KeyDeleteAsync(key);
        }

        public async Task SetOtpType(string clientId, OtpTypeEnum otpType)
        {
            string key = $"OtpType_{clientId}";
            await WriteDatabase().StringSetAsync(key, (int)otpType, new TimeSpan(1, 0, 0));
        }

        public async Task<bool> CheckOtpType(string clientId, OtpTypeEnum otpType)
        {
            string key = $"OtpType_{clientId}";
            RedisValue redisValue = await WriteDatabase().StringGetAsync(key);
            if (redisValue.HasValue)
            {
                return redisValue == (int)otpType;
            }

            return false;
        }

        public async Task<long> CacheRequestCount(string userId, string cacheType, int cacheMinute)
        {
            string key = $"CacheRequestCount_{userId}_{cacheType}";
            long increment = await WriteDatabase().StringIncrementAsync(key, 1);
            await WriteDatabase().KeyExpireAsync(key, new TimeSpan(0, cacheMinute, 0));
            return increment;
        }

        private byte[] ConvertInput(object value)
        {
            return Serialize.ProtoBufSerialize(value, true);
        }

        private T? ConvertOutput<T>(RedisValue redisValue)
        {
            try
            {
                if (redisValue.HasValue)
                {
                    return Serialize.ProtoBufDeserialize<T>(redisValue, true);
                }

                return default;
            }
            catch (Exception e)
            {
                logger.LogError(e, "{Message}", e.Message);
                return default;
            }
        }
    }
}
