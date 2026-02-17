namespace DependencyInjection.Models;

public class AuditLoggerSettings
{
    public string LogLevel { get; set; } = "Summary"; // "Detailed" or "Summary"
    public bool IncludeTimestamp { get; set; } = true;
    public bool IncludeTenantId { get; set; } = true;
    public int MaxLogEntries { get; set; } = 1000;
}