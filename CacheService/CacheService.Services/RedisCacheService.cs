namespace CacheService.Services
{
    internal class RedisCacheService : IRedisCacheService
    {
        public void Add(string store, string key, string value)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Clear(string store)
        {
            throw new NotImplementedException();
        }

        public string Get(string store, string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetAll(string store)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetByKeys(string store, string[] keys)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetStoreNames()
        {
            throw new NotImplementedException();
        }

        public void Remove(string store, string key)
        {
            throw new NotImplementedException();
        }
    }
}
