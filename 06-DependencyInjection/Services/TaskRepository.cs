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

    public Task<int> CreateAsync(TaskItem task)
    {
        // Assign task to next id (simulating database) and add to task item list
        int id;
        lock (_lock)
        {
            var now = DateTime.UtcNow; // Takes current timestamp
            id = _nextId++;

            task.Id = id;
            task.CreatedAt = now;
            task.LastUpdatedAt = now;
            _taskStore[_currentTenantId].Add(task);
        }

        return Task.FromResult(id);
    }

    public Task<TaskItem?> GetByIdAsync(int id)
    {
        List<TaskItem> tasks = _taskStore[_currentTenantId];

        foreach (TaskItem task in tasks)
            if (task.Id == id)
                return Task.FromResult<TaskItem?>(task);

        return Task.FromResult<TaskItem?>(null);
    }

    public Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<TaskItem>>(_taskStore[_currentTenantId]);
    }

    public Task<TaskItem?> UpdateAsync(TaskItem toBeUpdatedTask)
    {
        lock (_lock)
        {
            // Gets tenant tasks
            List<TaskItem> tasks = _taskStore[_currentTenantId];

            // Check tasks until id is found, then replace
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Id == toBeUpdatedTask.Id)
                {
                    TaskItem oldTask = tasks[i];

                    toBeUpdatedTask.CreatedAt = oldTask.CreatedAt; // Preserve createdAt
                    toBeUpdatedTask.LastUpdatedAt = DateTime.UtcNow; // Update lastUpdatedAt

                    tasks[i] = toBeUpdatedTask;
                    return Task.FromResult<TaskItem?>(oldTask);
                }
            }

            // Throws exception not found
            throw new KeyNotFoundException($"Task with ID {toBeUpdatedTask.Id} not found");
        }
    }

    public Task<TaskItem?> DeleteAsync(int id)
    {
        lock (_lock)
        {
            // Gets tenant tasks
            List<TaskItem> tasks = _taskStore[_currentTenantId];

            // Check tasks until found
            foreach (TaskItem task in tasks)
            {
                if (task.Id == id)
                {
                    TaskItem deletedTask = task;
                    tasks.Remove(task);
                    return Task.FromResult<TaskItem?>(deletedTask);
                }
            }
        }

        // Throws except if not found
        throw new KeyNotFoundException($"Task with ID {id} not found");
    }
}