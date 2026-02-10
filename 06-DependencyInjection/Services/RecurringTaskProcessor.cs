using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class RecurringTaskProcessor : ITaskProcessor
{
    private readonly IAuditLogger _logger;
    private readonly ITenantProvider _tenantProvider;

    public RecurringTaskProcessor(IAuditLogger auditLogger, ITenantProvider tenantProvider)
    {
        _logger = auditLogger;
        _tenantProvider = tenantProvider;
    }

    public void Process(TaskItem task)
    {
        string tenantId = _tenantProvider.GetTenantId();

        Console.WriteLine($"RECURRING: Processing task '{task.Title}' queued for execution");
        _logger.Log(tenantId, AuditEvent.TaskProcessed);
    }

    public TaskType GetProcessorType() => TaskType.Recurring;
}