using DependencyInjection.Enums;

namespace DependencyInjection.Interfaces;

public interface IAuditLogger
{
    public void Log(AuditEvent audioEvent, string description);
    public Dictionary<AuditEvent, int> GetTotalOperations(string tenantId);
}