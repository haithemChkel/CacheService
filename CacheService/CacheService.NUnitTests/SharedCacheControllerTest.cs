using CacheService.Controllers;
using CacheService.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace CacheService.NUnitTests
{
    public class SharedCacheControllerTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task SharedCacheController_Get_Should_Return_Non_Empty_String()
        {
            // ARRANGE
            var sharedCacheService = new Mock<ISharedCacheService>();
            var logger = new Mock<ILogger<ISharedCacheService>>();
            var expectedResult = "storing redis object in cache";
            sharedCacheService.Setup(item => item.Get(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(expectedResult);
            var controller = new SharedCacheController(sharedCacheService.Object, logger.Object);
            // ACT
            var result = await controller.Get("testRedis1", "testv1-redis");
            // ASSERT
            Assert.NotNull(result);
            Assert.That(expectedResult, Is.EqualTo(result));
        }
        [Test]
        public async Task SharedCacheController_Get_Should_Return_Empty_String_if_Cache_Does_Not_Contains_Store()
        {
            // ARRANGE
            var sharedCacheService = new Mock<ISharedCacheService>();
            var logger = new Mock<ILogger<ISharedCacheService>>();
            var expectedResult = "";
            sharedCacheService.Setup(item => item.Get(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(expectedResult);
            var controller = new SharedCacheController(sharedCacheService.Object, logger.Object);
            // ACT
            var result = await controller.Get("test", "test");
            // ASSERT
            Assert.NotNull(result);
            Assert.That(expectedResult, Is.EqualTo(result));
        }
        [Test]
        public async Task SharedCacheController_Get_Should_Return_Empty_String_if_Cache_Does_Not_Contains_Key()
        {
            // ARRANGE
            var sharedCacheService = new Mock<ISharedCacheService>();
            var logger = new Mock<ILogger<ISharedCacheService>>();
            var expectedResult = "";
            sharedCacheService.Setup(item => item.Get(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(expectedResult);
            var controller = new SharedCacheController(sharedCacheService.Object, logger.Object);
            // ACT
            var result = await controller.Get("testRedis1", "test");
            // ASSERT
            Assert.NotNull(result);
            Assert.That(expectedResult, Is.EqualTo(result));
        }
        [Test]
        public async Task SharedCacheController_GetAll_Should_Return_Empty_List_if_Cache_Does_Not_Contains_Store()
        {
            // ARRANGE
            var sharedCacheService = new Mock<ISharedCacheService>();
            var logger = new Mock<ILogger<ISharedCacheService>>();
            var expectedResult = Enumerable.Empty<string>();
            sharedCacheService.Setup(item => item.GetAll(It.IsAny<string>())).ReturnsAsync(expectedResult);
            var controller = new SharedCacheController(sharedCacheService.Object, logger.Object);
            // ACT
            var result = await controller.GetAll("testRed");
            // ASSERT
            Assert.IsEmpty(result);
            Assert.That(expectedResult, Is.EqualTo(result));
               
        }
        [Test]
        public async Task SharedCacheController_GetAll_Should_Return_Values_if_Cache_Does_Contains_Store()
        {
            // ARRANGE
            var sharedCacheService = new Mock<ISharedCacheService>();
            var logger = new Mock<ILogger<ISharedCacheService>>();
            var expectedResult = new List<string>()
            {
                "storing redis object in cache",
                "storing redis object in cache"
            };
            sharedCacheService.Setup(item => item.GetAll(It.IsAny<string>())).ReturnsAsync(expectedResult);
            var controller = new SharedCacheController(sharedCacheService.Object, logger.Object);
            // ACT
            var result = await controller.GetAll("testRedis");
            // ASSERT
            Assert.IsNotEmpty(result);
            Assert.That(expectedResult, Is.EqualTo(result));
        }
        [Test]
        public async Task SharedCacheController_GetStoreNames_Should_Return_Empty_Values_if_Cache_Is_Empty()
        {
            // ARRANGE
            var sharedCacheService = new Mock<ISharedCacheService>();
            var logger = new Mock<ILogger<ISharedCacheService>>();
            var expectedResult = new List<string>() { };
            sharedCacheService.Setup(item => item.GetStoreNames()).ReturnsAsync(expectedResult);
            var controller = new SharedCacheController(sharedCacheService.Object, logger.Object);
            // ACT
            var result = await controller.GetStoreNames();
            // ASSERT
            Assert.IsEmpty(result);
            Assert.That(expectedResult, Is.EqualTo(result));
        }
        [Test]
        public async Task SharedCacheController_GetStoreNames_Should_Return_Stores_Name_if_Cache_Is_Not_Empty()
        {
            // ARRANGE
            var sharedCacheService = new Mock<ISharedCacheService>();
            var logger = new Mock<ILogger<ISharedCacheService>>();
            var expectedResult = new List<string>() { "testRedis1", "testRedis2" };
            sharedCacheService.Setup(item => item.GetStoreNames()).ReturnsAsync(expectedResult);
            var controller = new SharedCacheController(sharedCacheService.Object, logger.Object);
            // ACT
            var result = await controller.GetStoreNames();
            // ASSERT
            Assert.IsNotEmpty(result);
            Assert.That(expectedResult, Is.EqualTo(result));
        }
        [Test]
        public async Task SharedCacheController_GetByKeys_Should_Return_Values_if_Cache_Is_Not_Empty()
        {
            // ARRANGE
            var sharedCacheService = new Mock<ISharedCacheService>();
            var logger = new Mock<ILogger<ISharedCacheService>>();
            var keys = new string[] { "testv1-redisv2", "testv2+redis" };
            var expectedResult = new List<string>() { "storing redis object in cache", "testing redis v2" };
            sharedCacheService.Setup(item => item.GetByKeys("testRedis2", keys)).ReturnsAsync(expectedResult);
            var controller = new SharedCacheController(sharedCacheService.Object, logger.Object);
            // ACT
            var result = await controller.GetByKeys("testRedis2", keys);
            // ASSERT
            Assert.IsNotEmpty(result);
            Assert.That(expectedResult, Is.EqualTo(result));
        }
    }
}