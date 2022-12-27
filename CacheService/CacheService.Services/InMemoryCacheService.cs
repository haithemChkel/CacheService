using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;

namespace CacheService.Services
{
    public class InMemoryCacheService : IInMemoryCacheService
    {
        private readonly ConcurrentDictionary<string, MemoryCache> _caches = new ConcurrentDictionary<string, MemoryCache>();
        private readonly ConcurrentDictionary<string, string> _keysDict = new ConcurrentDictionary<string, string>();
        public async Task Add(string store, string key, string value)
        {
            await Task.Run(() =>
            {
                var cacheEntryOptions = new MemoryCacheOptions();
                var storeCache = _caches.GetOrAdd(store, new MemoryCache(cacheEntryOptions));
                storeCache.Set(key, value, new MemoryCacheEntryOptions());
                if (!_keysDict.ContainsKey(key))
                {
                    _keysDict.TryAdd(key, store);
                }
            });
        }

        public async Task AddMany(string store, Dictionary<string, string> keyValues)
        {
            await Task.Run(() =>
            {
                foreach (var key in keyValues.Keys)
                {
                    Add(store, key, keyValues[key]);
                }
            });
        }

        public async Task Clear()
        {
            await Task.Run(() =>
            {
                foreach (var cache in _caches.Values)
                {
                    cache.Dispose();
                }
                _keysDict.Clear();
                _caches.Clear();
            });
        }

        public async Task Clear(string store)
        {
            await Task.Run(() =>
            {
                if (_caches.TryGetValue(store, out var cacheStore))
                {
                    foreach (var key in _keysDict.Keys)
                    {
                        var ifKeyExists = _keysDict.TryGetValue(key, out string storeOfTheKey);
                        if (ifKeyExists && store == storeOfTheKey)
                        {
                            _keysDict.Remove(key, out string value);
                        }
                    }
                    cacheStore.Dispose();
                    _caches.TryRemove(store, out cacheStore);
                }
            });
        }

        public async Task<string> Get(string store, string key)
        {
            var value = await Task.Run(() =>
            {
                if (_caches.TryGetValue(store, out var cacheStore))
                {
                    var value = cacheStore.Get(key);
                    return (string)value;
                }
                return null;
            });
            return value;
        }

        public async Task<IEnumerable<string>> GetAll(string store)
        {
            var values = await Task.Run(() => {
                if (_caches.TryGetValue(store, out var cacheStore))
                {
                    var values = new List<string>();
                    foreach (var item in _keysDict.Keys)
                    {
                        if (cacheStore.TryGetValue(item, out string value))
                        {
                            values.Add(value);
                        }
                    }
                    return values;
                }
                return Enumerable.Empty<string>();
            });
            return values;
        }

        public async Task<IEnumerable<string>> GetByKeys(string store, string[] keys)
        {
            var values = await Task.Run(() =>
            {
                if (_caches.TryGetValue(store, out var cacheStore))
                {
                    var values = new List<string>();
                    for (int i = 0; i < keys.Length; i++)
                    {
                        if (cacheStore.TryGetValue(keys[i], out string value))
                        {
                            values.Add(value);
                        }
                    }
                    return values;
                }
                return Enumerable.Empty<string>();
            });
            return values;
        }

        public async Task<IEnumerable<string>> GetStoreNames()
        {
            var stores = await Task.Run(() => {
                return _caches.Keys;
            });
            return stores;
            
        }

        public async Task Remove(string store, string key)
        {
            await Task.Run(() => {
                if (_caches.TryGetValue(store, out var cacheStore))
                {
                    cacheStore.Remove(key);
                    var ifKeyExist = _keysDict.TryGetValue(key, out string storeOfTheKey);
                    if (ifKeyExist && store == storeOfTheKey)
                    {
                        _keysDict.Remove(key, out string value);
                    }
                }
            });
        }
    }
}