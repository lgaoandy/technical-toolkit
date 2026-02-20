using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class RecurringTaskProcessor(IAuditLogger auditLogger, ITenantProvider tenantProvider) : ITaskProcessor
{
    public TaskType ProcessorType => TaskType.Recurring;
    private readonly IAuditLogger _logger = auditLogger;
    private readonly string _tenantId = tenantProvider.GetTenantId();

    public void Process(TaskItem task)
    {
        string description = $"RECURRING: Processing task '{task.Title}' queued for execution";
        _logger.Log(AuditEvent.TaskProcessed, description, _tenantId);
    }
}