using superecommere.Models.Store;
using superecommere.Services;

namespace superecommere.Middleware
{

    public class StoreMiddleware
    {
        private readonly RequestDelegate _next;

        public StoreMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IStoreService storeService)
        {
            var host = context.Request.Host.Host.ToLower();
            TblStore store = null;
            var url = "https://localtest.me:5000/api/Store/by-subdomain/test12";
            var uri = new Uri(url);
            var segments = uri.Segments;
            var storeName = context.Request.Path.Value.Split('/').Last(); // "test12"

            var request = context.Request;

            var scheme = request.Scheme;                 // "https"
            var host1 = request.Host.Value;               // "localhost:5000"
            var path = request.Path;                     // "/swagger/index.html"
            var queryString = request.QueryString.Value; // e.g. "?search=abc"

            var fullUrl = $"{scheme}://{host1}{path}{queryString}";

            uri = new Uri(fullUrl);
            segments = uri.Segments;
            if (fullUrl.IndexOf("by-subdomain") != -1)
            {
                storeName = segments.Last().Trim('/');
            }
            else
            {
                storeName = null;
            }

            if (storeName != null || host.EndsWith("yourdomain.com"))
            {
                var parts = host.Split('.');
                if (storeName!=null)
                    store = await storeService.GetStoreBySubdomainAsync(storeName);
                else
                    store = await storeService.GetStoreByCustomDomainAsync(host);
            }

            if (store == null && storeName!=null)
            {
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Store not found");
                return;
            }

            context.Items["Store"] = store;
            await _next(context);
        }
    }
}
