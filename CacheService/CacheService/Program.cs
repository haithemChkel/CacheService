using CacheService.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSingleton<ISharedCacheService,InMemoryCacheService>();
//builder.Services.AddStackExchangeRedisCache(options =>
//{ 
//    options.Configuration = builder.Configuration.GetSection("Redis")["ConnectionString"];
//});
//builder.Services.Add(ServiceDescriptor.Singleton<IDistributedCache, RedisCache>());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
