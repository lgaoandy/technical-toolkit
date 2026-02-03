using DependencyInjection.Enums;

namespace DependencyInjection.Interfaces;

public interface IAudioLogger
{
    public void Log(string tenantId, AuditEvent audioEvent);
    public Dictionary<AuditEvent, int> GetActivityCount(string tenantId);
}