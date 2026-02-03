using DependencyInjection.Enums;

namespace DependencyInjection.Models;

public class ActivityEntry(string tenantId, AuditEvent auditEvent)
{
    public string TenantId { get; } = tenantId;
    public AuditEvent AuditEvent { get; } = auditEvent;
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}