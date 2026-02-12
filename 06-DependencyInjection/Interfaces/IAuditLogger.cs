using DependencyInjection.Enums;

namespace DependencyInjection.Interfaces;

public interface IAuditLogger
{
    public void Log(string tenantId, AuditEvent audioEvent);
    public Dictionary<AuditEvent, int> GetTotalOperations(string tenantId);
}