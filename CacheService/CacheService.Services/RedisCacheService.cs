using Microsoft.Extensions.Caching.Distributed;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Text;

namespace CacheService.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;
        private const string _storeKeys = "storesKeys";
        public RedisCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }
        public async Task Add(string store, string key, string value)
        {
            await Task.Run(() =>
            {
                if (_cache.Get(_storeKeys) != null)
                {
                    var val = _cache.Get(_storeKeys);
                    Dictionary<string, string> valuesDict = JsonSerializer.Deserialize<Dictionary<string, string>>(val);
                    if (!valuesDict.ContainsKey(store))
                    {
                        valuesDict.Add(store, key);
                        var storeBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(valuesDict));
                        _cache.Set(_storeKeys, storeBytes);
                    }
                }
                else
                {
                    var stores = new Dictionary<string, string>();
                    stores.Add(store, key);
                    var storeBytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(stores));
                    _cache.Set(_storeKeys, storeBytes);
                }

                if (_cache.Get(store) == null)
                {
                    var valuesDict = new Dictionary<string, string>();
                    valuesDict.Add(key, value);
                    var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(valuesDict));
                    _cache.Set(store, bytes);
                }
                else
                {
                    var val = _cache.Get(store);
                    Dictionary<string, string> valuesDict = JsonSerializer.Deserialize<Dictionary<string, string>>(val);
                    valuesDict.Add(key, value);
                    var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(valuesDict));
                    _cache.Set(store, bytes);
                }

            });
        }

        public async Task Clear()
        {
            await Task.Run(() => {
                var keysValues = _cache.Get(_storeKeys);
                Dictionary<string, string> valuesDict = JsonSerializer.Deserialize<Dictionary<string, string>>(keysValues);
                foreach (var key in valuesDict.Keys)
                {
                    _cache.Remove(key);
                }
                _cache.Remove(_storeKeys);
            });
        }

        public async Task Clear(string store)
        {
            await Task.Run(() => {
                _cache.Remove(store);
                var val = _cache.Get(_storeKeys);
                Dictionary<string, string> valuesDict = JsonSerializer.Deserialize<Dictionary<string, string>>(val);
                valuesDict.Remove(store);
                var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(valuesDict));
                _cache.Set(_storeKeys, bytes);
            });
        }

        public async Task<string> Get(string store, string key)
        {
            var value = await Task.Run(() => {
                if (_cache.Get(store) != null)
                {
                    var cacheStore = _cache.Get(store);
                    Dictionary<string, string> valuesDict = JsonSerializer.Deserialize<Dictionary<string, string>>(cacheStore);
                    var valueByKey = valuesDict.TryGetValue(key, out string value);
                    return value;
                }
                return null;
            });
           return value;
        }

        public async Task<IEnumerable<string>> GetAll(string store)
        {
            var values = await Task.Run(() => {
                var val = _cache.Get(store);
                Dictionary<string, string> valuesDict = JsonSerializer.Deserialize<Dictionary<string, string>>(val);
                var storeValues = valuesDict?.Select(x => x.Value)
                        .Where(val => val != null)
                        .Cast<string>()
                        .ToArray();
                return storeValues;
            });
            return values;
        }

        public async Task<IEnumerable<string>> GetByKeys(string store, string[] keys)
        {
            var values = await Task.Run(() =>
            {
                if (_cache.Get(store) != null)
                {
                    List<string> values = new List<string>();
                    var cacheStore = _cache.Get(store);
                    Dictionary<string, string> valuesDict = JsonSerializer.Deserialize<Dictionary<string, string>>(cacheStore);
                    for (var i = 0; i < keys.Length; i++)
                    {
                        if (valuesDict.ContainsKey(keys[i]))
                        {
                            values.Add(valuesDict[keys[i]]);
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
            var keys = await Task.Run(() => {
                var cacheStore = _cache.Get(_storeKeys);
                Dictionary<string, string> cacheValuesDict = JsonSerializer.Deserialize<Dictionary<string, string>>(cacheStore);
                return cacheValuesDict.Keys;
            });
            return keys;
            
        }

        public async Task Remove(string store, string key)
        {
            await Task.Run(() => {
                if (_cache.Get(store) != null)
                {
                    var cacheStore = _cache.Get(store);
                    Dictionary<string, string> cacheValuesDict = JsonSerializer.Deserialize<Dictionary<string, string>>(cacheStore);
                    cacheValuesDict.Remove(key);
                    var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(cacheValuesDict));
                    _cache.Set(store, bytes);
                }
            });
           
        }

   
    }
}
