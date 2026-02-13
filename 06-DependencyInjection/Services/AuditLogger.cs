using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;
using Microsoft.Extensions.Options;

namespace DependencyInjection.Services;

public class AuditLogger : IAuditLogger
{
    private static readonly Dictionary<string, List<AuditLogEntry>> _logs = [];
    private readonly string _tenantId;
    private readonly AuditLoggerSettings _settings;

    public AuditLogger(IOptions<AuditLoggerSettings> options, ITenantProvider tenantProvider)
    {
        _settings = options.Value;
        _tenantId = tenantProvider.GetTenantId();
    }

    public void Log(AuditEvent auditEvent)
    {
        // Ensure tenant exists as a key
        _logs.TryAdd(_tenantId, []);

        // Create new entry
        AuditLogEntry entry = new(_tenantId, auditEvent);

        // Add entry to dictionary organized by tenant
        _logs[_tenantId].Add(entry);
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
}