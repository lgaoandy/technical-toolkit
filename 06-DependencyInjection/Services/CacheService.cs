using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class CacheService : ICacheService
{
    private Dictionary<string, object> _cache = new();
    private int _cacheHits = 0;
    private int _cacheMisses = 0;

    public void Set<T>(string key, T? value)
    {
        _cache[key] = value!;
    }

    public T Get<T>(string key)
    {
        if (!_cache.Keys.Contains(key))
        {
            _cacheMisses++;
            throw new ArgumentOutOfRangeException(nameof(key), "Key does not exist in cache"));
        }

        _cacheHits++;
        return (T)_cache[key];
    }

    public bool TryGet<T>(string key, out T? value)
    {
        if (_cache.TryGetValue(key, out var obj))
        {
            // Found it - cast and return true
            _cacheHits++;
            value = (T)obj;
            return true;
        }

        // Not found - set value to null/default and return false
        _cacheMisses++;
        value = default;
        return false;
    }

    public void Remove(string key)
    {
        _cache.Remove(key);
    }

    public int GetCacheHits()
    {
        return _cacheHits;
    }

    public int GetCacheMisses()
    {
        return _cacheMisses;
    }
}