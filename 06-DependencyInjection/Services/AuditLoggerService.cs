using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class AuditLogger : IAudioLogger
{
    private static readonly Dictionary<string, List<ActivityEntry>> _activityEntries = new();

    public Task Log(string tenantId, Operation operation)
    {
        // Ensure tenant exists as a key
        _activityEntries.TryAdd(tenantId, []);

        // Create new entry
        ActivityEntry entry = new(tenantId, operation);

        // Add entry to dictionary organized by tenant
        _activityEntries[tenantId].Add(entry);
        return Task.CompletedTask;
    }

    public Task<Dictionary<Operation, int>> ActivityCount(string tenantId)
    {
        // Retrieve entries from tenant
        List<ActivityEntry> entries = _activityEntries[tenantId];

        // Build counter
        Dictionary<Operation, int> counter = new();
        foreach (ActivityEntry entry in entries)
        {
            counter.TryAdd(entry.Operation, 0);
            counter[entry.Operation]++;
        }

        return Task.FromResult(counter);
    }
}