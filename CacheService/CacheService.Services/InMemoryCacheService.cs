using System.Collections.Concurrent;
using System.Runtime.Caching;

namespace CacheService.Services
{
    public class InMemoryCacheService : IInMemoryCacheService
    {
        private readonly ConcurrentDictionary<string, MemoryCache> _caches = new ConcurrentDictionary<string, MemoryCache>();

        public void Add(string store, string key, string value)
        {
            var storeCache = _caches.GetOrAdd(store, new MemoryCache(store));
            storeCache.Set(key, value, null);
        }

        public void Clear()
        {
            foreach (var cache in _caches.Values)
            {
                cache.Dispose();
            }
            _caches.Clear();
        }

        public void Clear(string store)
        {
            if (_caches.TryGetValue(store, out var cacheStore))
            {
                cacheStore.Dispose();
                _caches.TryRemove(store, out cacheStore);
            }
        }

        public string Get(string store, string key)
        {
            if (_caches.TryGetValue(store, out var cacheStore))
            {
                var value = cacheStore.Get(key);
                return (string)value;
            }
            return null;
        }

        public IEnumerable<string> GetAll(string store)
        {
            if (_caches.TryGetValue(store, out var cacheStore))
            {
                var values = cacheStore
                    .Select(x => x.Value)
                    .Where(val => val != null)
                    .Cast<string>()
                    .ToArray();
                return values;
            }
            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetByKeys(string store, string[] keys)
        {
            if (_caches.TryGetValue(store, out var cacheStore))
            {
                var values = cacheStore.GetValues(keys).Values.Cast<string>();
                return values;
            }
            return Enumerable.Empty<string>();
        }

        public IEnumerable<string> GetStoreNames()
        {
            return _caches.Keys;
        }

        public void Remove(string store, string key)
        {
            if (_caches.TryGetValue(store, out var cacheStore))
            {
                cacheStore.Remove(key);
            }
        }
    }
}