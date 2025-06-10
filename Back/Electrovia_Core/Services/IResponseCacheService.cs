
namespace Electrovia_Core.Services
{
    public  interface IResponseCacheService
    {
        Task CacheResponse(string CacheKey, object Respone, TimeSpan Time);
        Task<string> GetCacheResponse(string CacheKey);
    }
}
