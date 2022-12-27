// See https://aka.ms/new-console-template for more information
using CacheService.CacheHttpClient;

Console.WriteLine("Hello, World!");
HttpClientHandler handler = new HttpClientHandler();
CacheHttpClient httpClient = new CacheHttpClient("https://localhost:7267/SharedCache", handler);
httpClient.AddInCache("store1", "test1","string 1"); ;
Console.WriteLine("test get method ",httpClient.GetValueByStoreAndKey("store1", "key1"));