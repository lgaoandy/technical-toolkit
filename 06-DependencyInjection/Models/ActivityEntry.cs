using DependencyInjection.Enums;

namespace DependencyInjection.Models;

public class ActivityEntry(string tenantId, Operation operation)
{
    string TenantId { get; } = tenantId;
    Operation OperationType { get; } = operation;
    DateTime Timestamp { get; } = DateTime.UtcNow;
}