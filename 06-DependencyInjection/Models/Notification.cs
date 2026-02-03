using DependencyInjection.Enums;

namespace DependencyInjection.Models;

public class Notification
{
    public string Id = string.Empty;
    public string TenantId = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int TaskId {get; set;}
    public DateTime Timestamp { get; set; }

    public Notification(string id, string tenantId, int taskId, string message)
    {
        Id = id;
        TenantId = tenantId;
        TaskId = taskId;
        Message = message;
        Timestamp = DateTime.UtcNow;
    }
}