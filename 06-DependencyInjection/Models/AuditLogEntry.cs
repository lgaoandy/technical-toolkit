using DependencyInjection.Enums;

namespace DependencyInjection.Models;

public class AuditLogEntry()
{
    public required string TenantId { get; init; }
    public required AuditEvent AuditEvent { get; init; }
    public required string Description { get; init; }
    public required DateTime Timestamp { get; init; }
}