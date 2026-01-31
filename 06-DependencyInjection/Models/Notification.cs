namespace DependencyInjection.Models;

public class Notification
{
    public string Type { get; set; } = String.Empty; // "success" or "error"
    public string Message { get; set; } = String.Empty;
    public DateTime SendAt { get; set; }
}