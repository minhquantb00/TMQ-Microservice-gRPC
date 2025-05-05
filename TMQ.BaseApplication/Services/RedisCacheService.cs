using StackExchange.Redis;
using TMQ.Cache;
using TMQ.Common;

namespace TMQ.BaseApplication.Services
{
    public class RedisCacheService(
    RedisConnectionPool redisConnectionPool,
    string redisCacheEnvironment,
    ILogger<RedisCacheService> logger)
    : ICacheService
    {
        private IDatabase Database(int? dbId = null)
        {
            var db = dbId.HasValue ? redisConnectionPool.GetDatabase(dbId.Value) : redisConnectionPool.GetDatabase();
            if (db == null)
            {
                throw new Exception("Can not get redis database");
            }

            return db;
        }

        private string GetKey<T>(string? key)
        {
            return $"REDIS_{redisCacheEnvironment.AsEmpty()}_{typeof(T).FullName}_{key}";
        }

        private string GetKey(string? key)
        {
            return $"REDIS_{redisCacheEnvironment.AsEmpty()}_{key}";
        }

        public async Task<bool> SetFireAndForget<T>(T data, string? key, TimeSpan timeOut, int? dbId = null)
        {
            if (data == null)
            {
                return false;
            }

            string keyCache = GetKey<T>(key);
            var dataCache = ConvertInput(data);
            return await Database(dbId).StringSetAsync(keyCache, dataCache, timeOut, flags: CommandFlags.FireAndForget);
        }

        public async Task<bool> Set<T>(T? data, string? key, int? dbId = null)
        {
            if (data == null)
            {
                return false;
            }

            string keyCache = GetKey<T>(key);
            var dataCache = ConvertInput(data);
            return await Database(dbId).StringSetAsync(keyCache, dataCache);
        }

        public async Task<bool> SetNotExpire<T>(T data, string? key, int? dbId = null)
        {
            if (data == null)
            {
                return false;
            }

            string keyCache = GetKey<T>(key);
            var dataCache = ConvertInput(data);
            return await Database(dbId).StringSetAsync(keyCache, dataCache);
        }

        public async Task<bool> Set<T>(T? data, string? key, TimeSpan timeOut, int? dbId = null)
        {
            if (data == null)
            {
                return false;
            }

            var dataCache = ConvertInput(data);
            if (dataCache is not { Length: > 0 })
            {
                return false;
            }

            string keyCache = GetKey<T>(key);
            return await Database(dbId).StringSetAsync(keyCache, dataCache, timeOut);
        }

        public async Task<bool> Set<T>((T, string)[] itemByKeys, int? dbId = null)
        {
            List<KeyValuePair<RedisKey, RedisValue>> dataCaches = new List<KeyValuePair<RedisKey, RedisValue>>();
            foreach (var itemByKey in itemByKeys)
            {
                var dataCache = ConvertInput(itemByKey.Item1);
                if (dataCache != null)
                {
                    string keyCache = GetKey<T>(itemByKey.Item2);
                    dataCaches.Add(new KeyValuePair<RedisKey, RedisValue>(keyCache, dataCache));
                }
            }

            if (dataCaches.Count > 0)
            {
                return await Database(dbId).StringSetAsync(dataCaches.ToArray());
            }

            return true;
        }

        public async Task<bool> Set<T>((T, string)[] itemByKeys, string prefix, int? dbId = null)
        {
            List<KeyValuePair<RedisKey, RedisValue>> dataCaches = new List<KeyValuePair<RedisKey, RedisValue>>();
            foreach (var itemByKey in itemByKeys)
            {
                var dataCache = ConvertInput(itemByKey.Item1);
                if (dataCache != null)
                {
                    string keyCache = GetKey<T>($"{prefix}_{itemByKey.Item2}");
                    dataCaches.Add(new KeyValuePair<RedisKey, RedisValue>(keyCache, dataCache));
                }
            }

            if (dataCaches.Count > 0)
            {
                return await Database(dbId).StringSetAsync(dataCaches.ToArray());
            }

            return true;
        }

        public async Task<bool> Set<T>((T, string)[] itemByKeys, string prefix, TimeSpan timeOut, int? dbId = null)
        {
            foreach (var itemByKey in itemByKeys)
            {
                var dataCache = ConvertInput(itemByKey.Item1);
                if (dataCache == null) continue;
                string keyCache = GetKey<T>($"{prefix}_{itemByKey.Item2}");
                await Database(dbId).StringSetAsync(keyCache, dataCache, timeOut);
            }

            return true;
        }

        public async Task<bool> Set<T>(T data, string key, string prefix, int? dbId = null)
        {
            return await Set(data, $"{prefix}_{key}", dbId);
        }

        public async Task<bool> Set<T>(T data, string key, string prefix, TimeSpan timeOut, int? dbId = null)
        {
            return await Set(data, $"{prefix}_{key}", timeOut, dbId);
        }

        public async Task<T?> Get<T>(string? key, int? dbId = null)
        {
            string keyCache = GetKey<T>(key);
            var value = await Database(dbId).StringGetAsync(keyCache);
            return ConvertOutput<T>(value);
        }

        public async Task<T[]> Get<T>(string[] keys, int? dbId = null)
        {
            if (keys is not { Length: > 0 })
            {
                return [];
            }

            List<RedisKey> keyCaches = new List<RedisKey>();
            foreach (var t in keys)
            {
                keyCaches.Add(GetKey<T>(t));
            }

            RedisValue[] values = await Database(dbId).StringGetAsync(keyCaches.ToArray());

            T?[] results = new T?[keys.Length];
            int j = 0;
            foreach (var redisValue in values)
            {
                if (redisValue.HasValue)
                {
                    var obj = ConvertOutput<T>(redisValue);
                    results[j] = obj;
                }

                j++;
            }

            return results.Where(p => p != null).ToArray()!;
        }

        public async Task<T?> Get<T>(string key, string prefix, int? dbId = null)
        {
            return await Get<T>($"{prefix}_{key}", dbId);
        }

        public async Task<T[]> Get<T>(string[] keys, string prefix, int? dbId = null)
        {
            if (keys is not { Length: > 0 })
            {
                return [];
            }


            List<RedisKey> keyCaches = new List<RedisKey>();
            foreach (var t in keys)
            {
                keyCaches.Add(GetKey<T>($"{prefix}_{t}"));
            }

            RedisValue[] values = await Database(dbId).StringGetAsync(keyCaches.ToArray());

            T?[] results = new T?[keys.Length];
            int j = 0;
            foreach (var redisValue in values)
            {
                if (redisValue.HasValue)
                {
                    var obj = ConvertOutput<T>(redisValue);
                    results[j] = obj;
                }

                j++;
            }

            var data = results.Where(p => p != null).ToArray();
            return data!;
        }

        public async Task<T?> GetOrSetIfMissing<T>(string? key, Func<Task<T>> getData, int? dbId = null)
        {
            var data = await Get<T>(key, dbId);
            if (data != null) return data;
            data = await getData();
            await Set(data, key, dbId);
            return data;
        }

        public async Task<T?> GetOrSetIfMissing<T>(string? key, Func<Task<T>> getData, TimeSpan timeOut, int? dbId = null)
        {
            var data = await Get<T>(key, dbId);
            if (data != null) return data;
            data = await getData();
            await Set(data, key, timeOut, dbId);
            return data;
        }

        public async Task<T?> GetOrSetIfMissing<T>(string key, string prefix, Func<Task<T>> getData, int? dbId = null)
        {
            return await GetOrSetIfMissing($"{prefix}_{key}", getData, dbId);
        }

        public async Task<bool> Remove<T>(string? key, int? dbId = null)
        {
            string keyCache = GetKey<T>(key);
            return await Database(dbId).KeyDeleteAsync(keyCache);
        }

        public async Task<bool> Remove<T>(string key, string prefix, int? dbId = null)
        {
            return await Remove<T>($"{prefix}_{key}", dbId);
        }

        private byte[]? ConvertInput(object? value)
        {
            if (value == null)
            {
                return null;
            }

            try
            {
                return Serialize.ProtoBufSerialize(value, true);
            }
            catch (Exception e)
            {
                logger.LogError(e, "{Message} --- {FullName}", e.Message, value.GetType().FullName);
                throw;
            }
        }

        private T? ConvertOutput<T>(byte[]? redisValue)
        {
            try
            {
                if (redisValue?.Length > 0)
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

        public string[] Scan(int dbId, string pattern)
        {
            var server = redisConnectionPool.GetServer();
            if (server == null)
            {
                return [];
            }

            var keys = server.Keys(dbId, pattern).ToArray();
            return keys.Select(p => p.ToString()).ToArray();
        }

        public int KeysRemoveByPattern(int dbId, string pattern)
        {
            var server = redisConnectionPool.GetServer();
            if (server == null)
            {
                return 0;
            }

            var keys = server.Keys(dbId, pattern, 1000).ToArray();
            if (keys.Length > 0)
            {
                Database(dbId).KeyDelete(keys);
                return keys.Length;
            }

            return 0;
        }

        public async Task<long> GetNextValue<T>(string key, int? dbId = null)
        {
            string keyCache = GetKey<T>(key);
            var t = await Database(dbId).StringIncrementAsync(keyCache);
            return t;
        }

        public async Task<T?> HashGet<T>(string key, string member, int? dbId = null)
        {
            string keyCache = GetKey<T>(key);
            var value = await Database(dbId).HashGetAsync(keyCache, member);
            return ConvertOutput<T>(value);
        }

        public async Task<long?> HashGetLong(string key, string member, int? dbId = null)
        {
            string keyCache = GetKey(key);
            var value = await Database(dbId).HashGetAsync(keyCache, member);
            return (long?)value;
        }


        public async Task<T[]?> HashGet<T>(string key, string[] member, int? dbId = null)
        {
            string keyCache = GetKey<T>(key);
            RedisValue[] fields = member.Select(p => (RedisValue)p).ToArray();
            var values = await Database(dbId).HashGetAsync(keyCache, fields);
            if (values is { Length: > 0 })
            {
                List<T> data = [];
                foreach (var value in values)
                {
                    var dataItem = ConvertOutput<T>(value);
                    if (dataItem != null)
                    {
                        data.Add(dataItem);
                    }
                }

                return data.ToArray();
            }

            return [];
        }

        public async Task<(string Name, T? Value)[]> HashGetAll<T>(string key, int? dbId = null)
        {
            key = GetKey(key);
            HashEntry[] hashEntries = await Database(dbId).HashGetAllAsync(key);
            if (hashEntries.Length > 0)
            {
                List<(string Name, T? Value)> results = [];
                foreach (var hashEntry in hashEntries)
                {
                    results.Add((hashEntry.Name, ConvertOutput<T>(hashEntry.Value))!);
                }

                return results.ToArray();
            }

            return [];
        }


        public async Task<(RedisValue Field, RedisValue Value)[]> HashGetAll(string key, int? dbId = null)
        {
            key = GetKey(key);
            HashEntry[] hashEntries = await Database(dbId).HashGetAllAsync(key);
            if (hashEntries.Length > 0)
            {
                List<(RedisValue Field, RedisValue Value)> results = [];
                foreach (var hashEntry in hashEntries)
                {
                    if (hashEntry.Name.HasValue && hashEntry.Value.HasValue)
                    {
                        results.Add((hashEntry.Name, hashEntry.Value));
                    }
                }

                return results.ToArray();
            }

            return [];
        }

        public async Task<RedisValue[]> HashGet(string key, string[] member, int? dbId = null)
        {
            key = GetKey(key);
            RedisValue[] fields = member.Select(p => (RedisValue)p).ToArray();
            var values = await Database(dbId).HashGetAsync(key, fields);
            return values;
        }

        public async Task<long> HashLength(string key, int? dbId = null)
        {
            key = GetKey(key);
            return await Database(dbId).HashLengthAsync(key);
        }

        public async Task<(string key, (string member, T? value)[]?)[]> HashGetAll<T>(string[] keys, int? dbId = null)
        {
            keys = keys.Select(GetKey<T>).ToArray();
            string luaScript = @"
local function splitString(inputstr, sep)
                                        if sep == nil then
                                                sep = '%s'
                                        end
                                        local t={{}} ; local i=1
                                        for str in string.gmatch(inputstr, '([^'..sep..']+)') do
                                                t[i] = str;
                                                i = i + 1;
                                        end
                                        return t
                                end
local resultKeys = splitString(@keys, '@');
local r = {}
                                    for _, v in pairs(resultKeys) do
                                      r[#r+1] = redis.call('HGETALL', v)
                                    end

                                    return r";
            var redisKeys = string.Join("@", keys);
            var prepared = LuaScript.Prepare(luaScript);
            var vals = await prepared.EvaluateAsync(Database(dbId), new { keys = redisKeys });
            List<(string, (string, T?)[])> valuePairs = [];
            if (!vals.IsNull)
            {
                var results = (RedisResult[])vals!;

                int j = 0;
                foreach (var redisResult in results)
                {
                    List<(string, T)> valuePairsByKey = new List<(string, T)>();
                    if (redisResult.IsNull)
                    {
                        continue;
                    }

                    var resultsByKey = (RedisResult[])redisResult!;
                    for (int i = 0; i < resultsByKey.Length; i += 2)
                    {
                        var key = resultsByKey[i];
                        var value = resultsByKey[i + 1];
                        if (value.IsNull)
                        {
                            var valueObject = ConvertOutput<T>((byte[])value!);
                            valuePairsByKey.Add(((string)key!, valueObject)!);
                        }
                        else
                        {
                            valuePairsByKey.Add(((string)key!, default)!);
                        }
                    }

                    valuePairs.Add((keys[j], valuePairsByKey.ToArray())!);
                    j++;
                }
            }

            return valuePairs.ToArray()!;
        }

        public async Task<T[]?> HashValues<T>(string key, int? dbId = null)
        {
            string keyCache = GetKey<T>(key);
            var values = await Database(dbId).HashValuesAsync(keyCache);
            List<T> results = new List<T>();
            foreach (var value in values)
            {
                if (value.HasValue)
                {
                    var obj = ConvertOutput<T>(value);
                    if (obj != null)
                    {
                        results.Add(obj);
                    }
                }
            }

            return results.ToArray();
        }

        public async Task HashSet<T>(string key, string member, T value, int? dbId = null)
        {
            string keyCache = GetKey<T>(key);
            var dataCache = ConvertInput(value);
            await Database(dbId).HashSetAsync(keyCache, member, dataCache);
        }

        public async Task HashSet(string key, string member, long value, int? dbId = null)
        {
            string keyCache = GetKey(key);
            await Database(dbId).HashSetAsync(keyCache, member, value);
        }

        public async Task HashSet(string key, string member, string value, int? dbId = null)
        {
            string keyCache = GetKey(key);
            await Database(dbId).HashSetAsync(keyCache, member, value);
        }

        public async Task<long> HashIncrement(string key, string member, long value, int? dbId = null)
        {
            string keyCache = GetKey(key);
            return await Database(dbId).HashIncrementAsync(keyCache, member, value);
        }

        public async Task<long> HashIncrement(string key, long member, long value, int? dbId = null)
        {
            string keyCache = GetKey(key);
            return await Database(dbId).HashIncrementAsync(keyCache, member, value);
        }

        public async Task<long> HashIncrement(string key, int member, long value, int? dbId = null)
        {
            string keyCache = GetKey(key);
            return await Database(dbId).HashIncrementAsync(keyCache, member, value);
        }

        public async Task HashSet<T>(string key, (string key, T value)[] members, int? dbId = null)
        {
            string keyCache = GetKey<T>(key);
            List<HashEntry> entries = new List<HashEntry>();
            foreach (var member in members)
            {
                var dataCache = ConvertInput(member.value);
                entries.Add(new HashEntry(member.key, dataCache));
            }

            if (entries.Count > 0)
            {
                await Database(dbId).HashSetAsync(keyCache, entries.ToArray());
            }
        }

        public async Task HashDelete(string key, long[] member, int? dbId = null)
        {
            string keyCache = GetKey(key);
            await Database(dbId).HashDeleteAsync(keyCache, member.Select(p => (RedisValue)p).ToArray());
        }

        public async Task<bool> HashDelete(string key, string member, int? dbId = null)
        {
            string keyCache = GetKey(key);
            return await Database(dbId).HashDeleteAsync(keyCache, member);
        }

        public async Task HashDelete(string key, string[] members, int? dbId = null)
        {
            string keyCache = GetKey(key);
            await Database(dbId).HashDeleteAsync(keyCache, members.Select(p => (RedisValue)p).ToArray());
        }

        public async Task<bool> HashDelete<T>(string key, string member, int? dbId = null)
        {
            string keyCache = GetKey<T>(key);
            return await Database(dbId).HashDeleteAsync(keyCache, member);
        }

        public async Task<long> StringIncrement(string key, long value, TimeSpan timeout, int? dbId = null)
        {
            string keyCache = GetKey(key);
            long increment = await Database(dbId).StringIncrementAsync(keyCache, value);
            await Database(dbId).KeyExpireAsync(key, timeout);
            return increment;
        }

        public async Task<long> StringIncrement(string key, long value, int? dbId = null)
        {
            string keyCache = GetKey(key);
            long increment = await Database(dbId).StringIncrementAsync(keyCache, value);
            return increment;
        }

        public async Task<long?> StringGetLongValue(string key, int? dbId = null)
        {
            string keyCache = GetKey(key);
            var value = await Database(dbId).StringGetAsync(keyCache);
            if (value.HasValue)
            {
                return (long)value;
            }

            return null;
        }

        public async Task<long> ListRightPush(string key, string value, int? dbId = null)
        {
            string keyCache = GetKey(key);
            long increment = await Database(dbId).ListRightPushAsync(keyCache, value);
            return increment;
        }

        public async Task<double> SortedSetIncrement(string key, string member, double value, int? dbId = null)
        {
            string keyCache = GetKey(key);
            var increment = await Database(dbId).SortedSetIncrementAsync(keyCache, member, value);
            return increment;
        }

        public async Task<bool> KeyExpire(string key, TimeSpan timeout, int? dbId = null)
        {
            string keyCache = GetKey(key);
            var result = await Database(dbId).KeyExpireAsync(keyCache, timeout);
            return result;
        }

        public async Task<bool> KeyExpire<T>(string key, TimeSpan timeout, int? dbId = null)
        {
            string keyCache = GetKey<T>(key);
            return await Database(dbId).KeyExpireAsync(keyCache, timeout);
        }

        public async Task<bool> KeyRemove(string key, int? dbId = null)
        {
            string keyCache = GetKey(key);
            return await Database(dbId).KeyDeleteAsync(key);
        }
    }
}
