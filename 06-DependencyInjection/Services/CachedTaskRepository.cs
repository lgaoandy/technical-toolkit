using DependencyInjection.Interfaces;
using DependencyInjection.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace DependencyInjection.Services;

public class CachedTaskRepository : ITaskRepository
{
    private readonly ITaskRepository _innerRepository;
    private readonly ICacheService _cache;
    private readonly string _tenantId;

    public CachedTaskRepository(
        ITaskRepository innerRepository,
        ICacheService cache,
        ITenantProvider tenantProvider
    )
    {
        _innerRepository = innerRepository;
        _cache = cache;
        _tenantId = tenantProvider.GetTenantId();
    }

    public async Task<TaskItem?> GetByIdAsync(int taskId)
    {
        // Build cache key (include tenant ID!)
        string cacheKey = _cache.GenKey(_tenantId, taskId);

        // Try to get from cache, if not found, get from inner repository
        if (!_cache.TryGet(cacheKey, out TaskItem? task))
        {
            task = await _innerRepository.GetByIdAsync(taskId);
        }

        // Store cache and return result
        if (task != null)
        {
            _cache.Set(cacheKey, task);
        }

        return task;
    }

    public async Task<int> CreateAsync(TaskItem task)
    {
        // Cache the newly created task
        _cache.Set(_cache.GenKey(_tenantId, task.Id), task);
        return await _innerRepository.CreateAsync(task);
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return await _innerRepository.GetAllAsync();
    }

    public async Task<TaskItem> UpdateAsync(TaskItem updatedTask)
    {
        // To avoid stale data, update
        _cache.Set(_cache.GenKey(_tenantId, updatedTask.Id), updatedTask);
        return await _innerRepository.UpdateAsync(updatedTask);
    }

    public async Task<TaskItem?> DeleteAsync(int taskId)
    {
        // Remove from cache
        _cache.Remove(_cache.GenKey(_tenantId, taskId));
        return await _innerRepository.DeleteAsync(taskId);
    }
}