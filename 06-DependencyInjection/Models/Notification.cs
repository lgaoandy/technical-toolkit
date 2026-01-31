namespace DependencyInjection.Models;

public class Notification(string message)
{
    public string Message { get; set; } = message;
    public DateTime SendAt { get; set; } = DateTime.UtcNow;
}