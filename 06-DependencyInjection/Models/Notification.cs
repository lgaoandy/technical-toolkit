using DependencyInjection.Enums;

namespace DependencyInjection.Models;

public class Notification
{
    public string Id = string.Empty;
    public Operation Operation { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Description {get; set;} = string.Empty;
    public DateTime Timestamp { get; set; }

    public Notification(string id, Operation op, string message, string description)
    {
        Id = id;
        Operation = op;
        Message = message;
        Description = description;
        Timestamp = DateTime.UtcNow;
    }
}