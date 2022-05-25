using Microsoft.Extensions.Caching.Memory;
using ZeissTest.Interface;

namespace ZeissTest.Repository;

public class CacheService:ICacheService
{
    private readonly IMemoryCache _cache;

    public CacheService(IMemoryCache cache)
    {
        _cache = cache;
    }
    
    public async Task<object> GetAsync(string key)
    {
        return await Task.Run(() => _cache.Get<object>(key));
    }

    public async void SetAsync(string key, object value)
    {
        await Task.Run(() => _cache.Set(key, value));
    }
}