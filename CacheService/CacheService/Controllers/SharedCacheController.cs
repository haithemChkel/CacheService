using CacheService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CacheService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SharedCacheController : ControllerBase //,ISharedCacheService
    {
        private readonly ISharedCacheService _sharedCacheService;
        private readonly ILogger<ISharedCacheService> _logger;

        public SharedCacheController(ISharedCacheService sharedCacheService, ILogger<ISharedCacheService> logger)
        {
            _sharedCacheService = sharedCacheService;
            _logger = logger;   
        }

        [HttpPost]
        public void Add([FromQuery] string store, [FromQuery] string key, [FromBody] string value)
        {
            _logger.LogInformation($"store : {store}");
            _sharedCacheService.Add(store, key, value);
        }
        [HttpPost]
        [Route("AddMany")]
        public void AddMany([FromQuery] string store, [FromBody] Dictionary<string,string> keyValues)
        {
            _sharedCacheService.AddMany(store, keyValues);
        }

        [HttpGet]
        public Task<string> Get([FromQuery] string store, [FromQuery] string key)
        {
            _logger.LogInformation($"store : {store}");
            return _sharedCacheService.Get(store, key);
        }

        [HttpGet]
        [Route("GetAll")]
        public Task<IEnumerable<string>> GetAll([FromQuery] string store)
        {
            return _sharedCacheService.GetAll(store);
        }

        [HttpGet]
        [Route("GetByKeys")]
        public Task<IEnumerable<string>> GetByKeys([FromQuery] string store, [FromQuery] string[] keys)
        {
            return _sharedCacheService.GetByKeys(store, keys);
        }

        [HttpGet]
        [Route("GetStoreNames")]
        public Task<IEnumerable<string>> GetStoreNames()
        {
            return _sharedCacheService.GetStoreNames();
        }

        [HttpDelete]
        public void Remove([FromQuery] string store, [FromQuery] string key)
        {
            _sharedCacheService.Remove(store, key);
        }

        [HttpDelete]
        [Route("ClearStore")]
        public void Clear([FromQuery] string store)
        {
            _sharedCacheService.Clear(store);
        }

        [HttpDelete]
        [Route("ClearAll")]
        public void Clear()
        {
            _sharedCacheService.Clear();
        }
    }
}