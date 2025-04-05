using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

public class TrackingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IMemoryCache _cache;
    private const int Limit = 10;

    public TrackingMiddleware(RequestDelegate next, IMemoryCache cache)
    {
        _next = next;
        _cache = cache;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var ip = context.Connection.RemoteIpAddress?.ToString();

        if (string.IsNullOrEmpty(ip) || context.User.Identity.IsAuthenticated)
        {
            await _next(context);
            return;
        }

        var cacheKey = $"visitor_{ip}_{DateTime.UtcNow.Date:yyyyMMdd}";
        int count = _cache.GetOrCreate(cacheKey, entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24);
            return 0;
        });

        count++;
        _cache.Set(cacheKey, count);
        context.Items["VisitorLimitReached"] = count >= Limit;

        await _next(context);
    }
}

