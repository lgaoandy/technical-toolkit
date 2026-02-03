using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class AuditLogger : IAudioLogger
{
    private string _currentTenantId;
    private object _lock = new();
    private static readonly Dictionary<string, List<ActivityEntry>> _activityEntries = new();

    public AuditLogger(ITenantProvider tenantProvider)
    {
        _currentTenantId = tenantProvider.GetTenantId();

        lock (_lock)
            _activityEntries.TryAdd(_currentTenantId, []);
    }

    public Task Log(Operation operation)
    {
        // Create new entry
        ActivityEntry entry = new(_currentTenantId, operation);

        // Add entry to dictionary organized by tenant
        _activityEntries[_currentTenantId].Add(entry);
        return Task.CompletedTask;
    }
}