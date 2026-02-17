using DependencyInjection.Enums;

namespace DependencyInjection.Interfaces;

public interface IAuditLogger
{
    public void Log(AuditEvent audioEvent, string description, string tenantId);
    public Dictionary<AuditEvent, int> GetTotalOperations(string tenantId);
}