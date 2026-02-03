using DependencyInjection.Enums;

namespace DependencyInjection.Models;

public class ActivityEntry(string tenantId, Operation operation)
{
    public string TenantId { get; } = tenantId;
    public Operation Operation { get; } = operation;
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}