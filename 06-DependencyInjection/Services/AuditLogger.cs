using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;
using Microsoft.Extensions.Options;

namespace DependencyInjection.Services;

public class AuditLogger : IAuditLogger
{
    private static readonly Dictionary<string, List<AuditLogEntry>> _logs = [];
    private readonly AuditLoggerSettings _settings;
    private readonly Lock _lock = new();

    // Constructor
    public AuditLogger(IOptions<AuditLoggerSettings> options)
    {
        _settings = options.Value;
    }

    public void Log(AuditEvent auditEvent, string description, string tenantId)
    {
        lock (_lock)
        {
            // Ensure tenant exists as a key
            _logs.TryAdd(tenantId, []);

            // If logs of current tenant exceeds max entries, remove the first entry
            if (_logs[tenantId].Count > _settings.MaxLogEntries)
            {
                _logs[tenantId].RemoveAt(0);
            }

            // Create new entry
            var entry = new AuditLogEntry
            {
                TenantId = tenantId,
                AuditEvent = auditEvent,
                Description = description,
                Timestamp = DateTime.UtcNow
            };

            _logs[tenantId].Add(entry);

            // Console log messages
            if (_settings.LogLevel == "Detailed")
                LogDetailed(entry);
            if (_settings.LogLevel == "Summary")
                LogSummary(entry);
        }
    }

    public Dictionary<AuditEvent, int> GetTotalOperations(string tenantId)
    {
        // Retrieve entries from tenant
        List<AuditLogEntry> entries = _logs[tenantId];

        // Build counter
        Dictionary<AuditEvent, int> counter = new();
        foreach (AuditLogEntry entry in entries)
        {
            counter.TryAdd(entry.AuditEvent, 0);
            counter[entry.AuditEvent]++;
        }

        return counter;
    }

    private void LogDetailed(AuditLogEntry entry)
    {
        var message = $"[AUDIT]";

        if (_settings.IncludeTenantId)
            message += $" [{entry.Timestamp:yyyy-MM-dd HH:mm:ss}]";

        message += $" {entry.AuditEvent} - {entry.Description}";

        if (_settings.IncludeTenantId)
            message += $" for {entry.TenantId}";

        Console.WriteLine(message);
    }

    private void LogSummary(AuditLogEntry entry)
    {
        Console.WriteLine($"[AUDIT] {entry.AuditEvent}");
    }
}