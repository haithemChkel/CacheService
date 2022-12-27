using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CacheService.Api.Client
{
    public interface ICacheHttpClient
    {
        Task AddInCache(string store, string key, string value);
        Task ClearAllValuesFromCache(string store);
        Task ClearStoreFromCache(string store);
        Task<IEnumerable<string>> GetStoresNameFromCache();
        Task<string> GetValueByStoreAndKey(string store, string key);
        Task<IEnumerable<string>> GetValuesByKeys(string store, string[] keys);
        Task<IEnumerable<string>> GetValuesFromCacheByStore(string store);
        Task RemoveFromCache(string store, string key);
    }
}
