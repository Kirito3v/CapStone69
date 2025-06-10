using Electrovia_Core.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace Electrovia_Services
{
    public class ResponseCacheServices : IResponseCacheService
    {
        private readonly IDatabase _database;
        public ResponseCacheServices(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }

        public async Task CacheResponse(string CacheKey, object Respone, TimeSpan time)
        {
            if (Respone == null) return;
            var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var SerializeResponse = JsonSerializer.Serialize(Respone, options);
            await _database.StringSetAsync(CacheKey, SerializeResponse, time);
        }

        public async Task<string> GetCacheResponse(string CacheKey)
        {
            var CacheReponse = await _database.StringGetAsync(CacheKey);
            if (CacheReponse.IsNullOrEmpty) return null!;
            return CacheReponse!;
        }
    }
}
