namespace DependencyInjection.Models;

public class Notification
{
    public string Id { get; } = string.Empty;
    public string TenantId { get; } = string.Empty;
    public string Message { get; } = string.Empty;
    public DateTime Timestamp { get; }

    public Notification(string id, string tenantId, string message)
    {
        Id = id;
        TenantId = tenantId;
        Message = message;
        Timestamp = DateTime.UtcNow;
    }
}