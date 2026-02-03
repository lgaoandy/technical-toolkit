using DependencyInjection.Enums;

namespace DependencyInjection.Models;

public class ActivityEntry
{
    string TenantId { get; } = string.Empty;
    Operation OperationType { get; }
    DateTime Timestamp { get; }
}