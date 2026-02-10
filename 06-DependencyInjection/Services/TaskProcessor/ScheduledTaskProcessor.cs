using DependencyInjection.Enums;
using DependencyInjection.Interfaces;
using DependencyInjection.Models;

namespace DependencyInjection.Services;

public class ScheduledTaskProcessor : ITaskProcessor
{
    private readonly IAuditLogger _logger;
    private readonly ITenantProvider _tenantProvider;

    public ScheduledTaskProcessor(IAuditLogger auditLogger, ITenantProvider tenantProvider)
    {
        _logger = auditLogger;
        _tenantProvider = tenantProvider;
    }

    public void Process(TaskItem task)
    {
        string tenantId = _tenantProvider.GetTenantId();

        Console.WriteLine($"SCHEDULE: Processing task '{task.Title}' queued for execution");
        _logger.Log(tenantId, AuditEvent.TaskScheduled);
    }

    public TaskType GetProcessorType() => TaskType.Scheduled;
}