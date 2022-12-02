namespace CacheService.Services
{
    public interface ISharedCacheService
    {
        IEnumerable<string> GetStoreNames();
        IEnumerable<string> GetAll(string store);
        IEnumerable<string> GetByKeys(string store, string[] keys);
        string Get(string store, string key);
        void Add(string store, string key, string value);
        void Remove(string store, string key);
        void Clear(string store);
        void Clear();
    }
}