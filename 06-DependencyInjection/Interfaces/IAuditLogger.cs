using DependencyInjection.Enums;

namespace DependencyInjection.Interfaces;

public interface IAudioLogger
{
    public Task Log(string tenantId, AuditEvent audioEvent);
    public Task<Dictionary<AuditEvent, int>> GetActivityCount(string tenantId);
}