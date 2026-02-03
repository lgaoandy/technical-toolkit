using DependencyInjection.Enums;

namespace DependencyInjection.Models;

public class Notification
{
    public string Id = string.Empty;
    public string TenantId = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Description {get; set;} = string.Empty;
    public DateTime Timestamp { get; set; }

    public Notification(string id, string tenantId, string message, string description)
    {
        Id = id;
        TenantId = tenantId;
        Message = message;
        Description = description;
        Timestamp = DateTime.UtcNow;
    }
}