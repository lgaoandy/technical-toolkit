using DependencyInjection.Interfaces;
using DependencyInjection.Models;
using DependencyInjection.Enums;

namespace DependencyInjection.Services;

public class TaskRepository : ITaskRepository
{
    // Static storage - shared across all instance (simulates database)
    // Key: tenantId, Value: list of tasks for that tenant
    private static readonly Dictionary<string, List<TaskItem>> _taskStore = [];
    private static int _nextId = 1;
    private static readonly Lock _lock = new();

    private readonly IAudioLogger _audioLogger;
    private readonly string _currentTenantId;

    public TaskRepository(IAudioLogger audioLogger, ITenantProvider tenantProvider)
    {
        _audioLogger = audioLogger;
        _currentTenantId = tenantProvider.GetTenantId();

        // Ensure the current tenant has a list in the store
        lock (_lock)
            _taskStore.TryAdd(_currentTenantId, []);
    }

    public async Task<int> CreateAsync(TaskItem task)
    {
        // Assign task to next id (simulating database) and add to task item list
        int id;
        lock (_lock)
        {
            // Takes current timestamp
            var now = DateTime.UtcNow;
            id = _nextId++;

            task.Id = id;
            task.CreatedAt = now;
            task.LastUpdatedAt = now;
            _taskStore[_currentTenantId].Add(task);

            // Log activity
            _audioLogger.Log(_currentTenantId, AuditEvent.TaskCreated);
        }

        return id;
    }

    public Task<TaskItem?> GetByIdAsync(int id)
    {
        List<TaskItem> tasks = _taskStore[_currentTenantId];

        foreach (TaskItem task in tasks)
        {
            if (task.Id == id)
            {
                // Log activity
                _audioLogger.Log(_currentTenantId, AuditEvent.TaskRetrieved);
                return Task.FromResult<TaskItem?>(task);
            }
        }

        // Log activity
        _audioLogger.Log(_currentTenantId, AuditEvent.NotFound);
        return Task.FromResult<TaskItem?>(null);
    }

    public Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        // Log activity
        _audioLogger.Log(_currentTenantId, AuditEvent.TaskRetrieved);

        return Task.FromResult<IEnumerable<TaskItem>>(_taskStore[_currentTenantId]);
    }

    public Task<TaskItem> UpdateAsync(TaskItem toBeUpdatedTask)
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

                    // Log activity
                    _audioLogger.Log(_currentTenantId, AuditEvent.TaskUpdated);

                    return Task.FromResult<TaskItem>(oldTask);
                }
            }

            // Throws exception not found
            _audioLogger.Log(_currentTenantId, AuditEvent.NotFound);
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

                    // Log activity
                    _audioLogger.Log(_currentTenantId, AuditEvent.TaskDeleted);

                    return Task.FromResult<TaskItem?>(deletedTask);
                }
            }
        }

        // Throws except if not found
        _audioLogger.Log(_currentTenantId, AuditEvent.NotFound);
        throw new KeyNotFoundException($"Task with ID {id} not found");
    }
}