using System.Text;
using System;
using System.Text.Json;
using CacheService.Api.Client;

namespace CacheService.CacheHttpClient
{
    public class CacheHttpClient :HttpClient, ICacheHttpClient
    {
        private const string _getAllValuesByStoreApi = "GetAll";
        private const string _getStoresName = "GetStoreNames";
        private const string _getByKeys = "GetByKeys";
        private const string _clearAll = "ClearAll";
        private const string _clearStore = "ClearStore";
        public CacheHttpClient(string url, HttpClientHandler httpHandler) : base(httpHandler)
        {
            BaseAddress = new Uri(url);
        }
        public Task AddInCache(string store,string key, string value)
        {
            var builder = new UriBuilder();
            builder.Query = $"store={store}&key={key}";
            var url = builder.ToString();
            var json = JsonSerializer.Serialize(value);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            return PostAsync(url,data);
        }
        public async Task<string> GetValueByStoreAndKey(string store,string key)
        {
            var builder = new UriBuilder();
            builder.Query = $"store={store}&key={key}";
            var url = builder.ToString();
            HttpResponseMessage response = await GetAsync(url);
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();
            return resp;
        }
        public async Task<IEnumerable<string>> GetValuesFromCacheByStore(string store)
        {
            var builder = new UriBuilder(_getAllValuesByStoreApi);
            builder.Query = $"store={store}";
            var url = builder.ToString();
            HttpResponseMessage response = await GetAsync(url);
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();
            List<string> result =  JsonSerializer.Deserialize<List<string>>(resp);
            return result;
        }
        public async Task<IEnumerable<string>> GetStoresNameFromCache()
        {
            var builder = new UriBuilder(_getStoresName);
            var url = builder.ToString();
            HttpResponseMessage response = await GetAsync(url);
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();
            List<string> result = JsonSerializer.Deserialize<List<string>>(resp);
            return result;
        }
        public async Task<IEnumerable<string>> GetValuesByKeys(string store, string[] keys)
        {
            var builder = new UriBuilder(_getByKeys);
            builder.Query = $"store={store}&keys={keys}";
            var url = builder.ToString();
            HttpResponseMessage response = await GetAsync(url);
            response.EnsureSuccessStatusCode();
            var resp = await response.Content.ReadAsStringAsync();
            List<string> result = JsonSerializer.Deserialize<List<string>>(resp);
            return result;
        }
        public async Task RemoveFromCache(string store,string key)
        {
            var builder = new UriBuilder();
            builder.Query = $"store={store}&key={key}";
            var url = builder.ToString();
            await DeleteAsync(url);
        }
        public async Task ClearStoreFromCache(string store)
        {
            var builder = new UriBuilder(_clearStore);
            builder.Query = $"store={store}";
            var url = builder.ToString();
            await DeleteAsync(url);
        }
        public async Task ClearAllValuesFromCache(string store)
        {
            var builder = new UriBuilder(_clearAll);
            builder.Query = $"store={store}";
            var url = builder.ToString();
            await DeleteAsync(url);
        }
    }
}