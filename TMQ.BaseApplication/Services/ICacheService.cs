using StackExchange.Redis;

namespace TMQ.BaseApplication.Services
{
    public interface ICacheService
    {
        public Task<bool> SetNotExpire<T>(T data, string? key, int? dbId = null);
        public Task<bool> Set<T>(T data, string? key, TimeSpan timeOut, int? dbId = null);
        public Task<bool> SetFireAndForget<T>(T data, string? key, TimeSpan timeOut, int? dbId = null);
        public Task<bool> Set<T>(T data, string? key, int? dbId = null);
        public Task<bool> Set<T>((T, string)[] itemByKeys, int? dbId = null);
        public Task<bool> Set<T>((T, string)[] itemByKeys, string prefix, int? dbId = null);
        public Task<bool> Set<T>((T, string)[] itemByKeys, string prefix, TimeSpan timeOut, int? dbId = null);
        public Task<bool> Set<T>(T data, string key, string prefix, int? dbId = null);
        public Task<bool> Set<T>(T data, string key, string prefix, TimeSpan timeOut, int? dbId = null);
        public Task<T?> Get<T>(string? key, int? dbId = null);
        public Task<T[]> Get<T>(string[] keys, int? dbId = null);
        public Task<T?> Get<T>(string key, string prefix, int? dbId = null);
        public Task<T[]> Get<T>(string[] keys, string prefix, int? dbId = null);
        public Task<T?> GetOrSetIfMissing<T>(string? key, Func<Task<T>> getData, int? dbId = null);
        public Task<T?> GetOrSetIfMissing<T>(string? key, Func<Task<T>> getData, TimeSpan timeOut, int? dbId = null);
        public Task<T?> GetOrSetIfMissing<T>(string key, string prefix, Func<Task<T>> getData, int? dbId = null);
        public Task<bool> Remove<T>(string? key, int? dbId = null);
        public Task<bool> Remove<T>(string key, string prefix, int? dbId = null);
        string[]? Scan(int dbId, string pattern);
        int KeysRemoveByPattern(int dbId, string pattern);
        Task<long> GetNextValue<T>(string key, int? dbId = null);
        Task<T?> HashGet<T>(string key, string member, int? dbId = null);
        Task<long?> HashGetLong(string key, string member, int? dbId = null);
        Task<T[]?> HashGet<T>(string key, string[] member, int? dbId = null);

        Task<(string Name, T? Value)[]> HashGetAll<T>(string key, int? dbId = null);

        //Task<(long Field, long Value)[]> HashGetAll(string key);
        Task<long> HashLength(string key, int? dbId = null);
        Task<(RedisValue Field, RedisValue Value)[]> HashGetAll(string key, int? dbId = null);
        Task<RedisValue[]> HashGet(string key, string[] member, int? dbId = null);
        Task<(string key, (string member, T? value)[]?)[]> HashGetAll<T>(string[] keys, int? dbId = null);
        Task<T[]?> HashValues<T>(string key, int? dbId = null);
        Task HashSet<T>(string key, string member, T value, int? dbId = null);
        Task HashSet(string key, string member, long value, int? dbId = null);
        Task HashSet(string key, string member, string value, int? dbId = null);
        Task<long> HashIncrement(string key, string member, long value, int? dbId = null);
        Task<long> HashIncrement(string key, long member, long value, int? dbId = null);
        Task<long> HashIncrement(string key, int member, long value, int? dbId = null);
        Task HashSet<T>(string key, (string key, T value)[] members, int? dbId = null);
        Task HashDelete(string key, long[] member, int? dbId = null);
        Task<bool> HashDelete(string key, string member, int? dbId = null);
        Task HashDelete(string key, string[] members, int? dbId = null);
        Task<bool> HashDelete<T>(string key, string member, int? dbId = null);
        Task<long> StringIncrement(string key, long value, TimeSpan timeout, int? dbId = null);
        Task<long> StringIncrement(string key, long value, int? dbId = null);
        Task<long?> StringGetLongValue(string key, int? dbId = null);
        Task<long> ListRightPush(string key, string value, int? dbId = null);
        Task<double> SortedSetIncrement(string key, string member, double value, int? dbId = null);
        Task<bool> KeyExpire(string key, TimeSpan timeout, int? dbId = null);
        Task<bool> KeyExpire<T>(string key, TimeSpan timeout, int? dbId = null);
        Task<bool> KeyRemove(string key, int? dbId = null);
    }
}
