using System.Text;
using Electrovia_Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Electrovia.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        #region Constructor 
        private readonly int _time;
        public CachedAttribute(int time)
        {
            _time = time;
        }

        #endregion

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cachedReponse = await cacheService.GetCacheResponse(cacheKey);

            if (!string.IsNullOrEmpty(cachedReponse))
            {
                var contentResult = new ContentResult()
                {
                    Content = cachedReponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
                context.Result = contentResult;
                return;
            }
            var ExecutedEndPoint = await next.Invoke();
            if (ExecutedEndPoint.Result is OkObjectResult okObjectResult)
                await cacheService.CacheResponse(cacheKey, okObjectResult.Value!, TimeSpan.FromSeconds(_time));
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            
            keyBuilder.Append(request.Path);
            
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
