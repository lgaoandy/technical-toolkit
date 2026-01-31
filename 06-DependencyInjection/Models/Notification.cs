using DependencyInjection.Enums;

namespace DependencyInjection.Models;

public class Notification(Operation op, string message)
{
    public Operation operation = op;
    public string Message { get; set; } = message;
    public DateTime SendAt { get; set; } = DateTime.UtcNow;
}