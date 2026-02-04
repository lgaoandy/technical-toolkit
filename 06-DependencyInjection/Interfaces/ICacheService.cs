namespace DependencyInjection.Interfaces;

public interface ICacheService
{
    string GenKey(string tenantId, int secondaryId);
    void Set<T>(string key, T? value);
    T Get<T>(string key);
    bool TryGet<T>(string key, out T? value);
    void Remove(string key);
    int GetCacheHits();
    int GetCacheMisses();
}