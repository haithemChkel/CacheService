namespace CacheService.Services
{
    public interface ISharedCacheService
    {
        Task<IEnumerable<string>> GetStoreNames();
        Task<IEnumerable<string>> GetAll(string store);
        Task<IEnumerable<string>> GetByKeys(string store, string[] keys);
        Task<string> Get(string store, string key);
        Task Add(string store, string key, string value);
        Task AddMany(string store, Dictionary<string,string> keyValues);
        Task Remove(string store, string key);
        Task Clear(string store);
        Task Clear();
    }
}