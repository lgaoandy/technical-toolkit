using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class TaskRepository : ITaskRepository
{
    // Static storage - shared across all instance (simulates database)
    // Key: tenantId, Value: list of tasks for that tenant
    private static readonly Dictionary<string, List<TaskItem>> _taskStore = new();
    private static int _nextId = 1;
    private static readonly object _lock = new object();

    private readonly ITenantProvider _tenantProvider;
    private readonly string _currentTenantId;

    public TaskRepository(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
        _currentTenantId = _tenantProvider.GetTenantId();

        // Ensure the current tenant has a list in the store
        lock (_lock)
        {
            _taskStore.TryAdd(_currentTenantId, []);
        }
    }

    public Task CreateAsync(TaskItem task)
    {
        // Assign task to next id (simulating database) and add to task item list
        lock (_lock)
        {
            task.Id = _nextId++;
            task.CreatedAt = DateTime.UtcNow;
            _taskStore[_currentTenantId].Add(task);
        }

        return Task.CompletedTask;
    }

    public Task<TaskItem?> GetByIdAsync(int id)
    {

        throw new NotImplementedException();
    }
}