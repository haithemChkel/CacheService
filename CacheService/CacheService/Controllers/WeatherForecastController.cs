using CacheService.Services;
using Microsoft.AspNetCore.Mvc;

namespace CacheService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SharedCacheController : ControllerBase //,ISharedCacheService
    {
        private readonly ISharedCacheService _sharedCacheService;

        public SharedCacheController(ISharedCacheService sharedCacheService)
        {
            _sharedCacheService = sharedCacheService;
        }

        [HttpPost]
        public void Add([FromQuery] string store, [FromQuery] string key, [FromBody] string value)
        {
            _sharedCacheService.Add(store, key, value);
        }

        [HttpGet]
        [Route("GetAll")]
        public string Get([FromQuery] string store, [FromQuery] string key)
        {
            return _sharedCacheService.Get(store, key);
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<string> GetAll([FromQuery] string store)
        {
            return _sharedCacheService.GetAll(store);
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<string> GetByKeys([FromQuery] string store, [FromQuery] string[] keys)
        {
            return _sharedCacheService.GetByKeys(store, keys);
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<string> GetStoreNames()
        {
            return _sharedCacheService.GetStoreNames();
        }

        [HttpDelete]
        [Route("GetAll")]
        public void Remove([FromQuery] string store, [FromQuery] string key)
        {
            _sharedCacheService.Remove(store, key);
        }

        [HttpDelete]
        [Route("GetAll")]
        public void Clear([FromQuery] string store)
        {
            _sharedCacheService.Clear(store);
        }

        [HttpDelete]
        [Route("GetAll")]
        public void Clear()
        {
            _sharedCacheService.Clear();
        }
    }
}