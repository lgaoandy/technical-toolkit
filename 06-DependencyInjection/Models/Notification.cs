using DependencyInjection.Enums;

namespace DependencyInjection.Models;

public class Notification
{
    public string Id = string.Empty;
    public Operation Operation { get; set; }
    public string Message { get; set; } = string.Empty;
    public string Description {get; set;} = string.Empty;
    public DateTime Timestamp { get; set; }

    public Notification(Operation op, string message, string description)
    {
        Id = Guid.NewGuid().ToString()[0..8];
        Operation = op;
        Message = message;
        Description = description;
        Timestamp = DateTime.UtcNow;
    }

    public Notification(Operation op, string message) 
    : this(op, message, string.Empty)
    {
    }
}